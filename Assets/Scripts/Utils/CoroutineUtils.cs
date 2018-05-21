using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class CoroutineUtils {

	/// <summary>
	/// MonoBehaviour that we can use as an easy Coroutine container for GameObjects that are disabled, and also as a callback to
	/// Unity's Main Thread.
	/// </summary>
	private class Handler : MonoBehaviour {

		public static Handler Instance { get; private set; }
		private Queue<Action> __eventPump = new Queue<Action>();

		private Thread unityThread;

		public int ActionsAllowedPerFrame { get; set; } = INFINITE_ACTIONS;

		public bool IsRunningOnMain => Thread.CurrentThread == unityThread;

		public Handler() {
			Instance = this;
		}

		private void Awake() {
			unityThread = Thread.CurrentThread;
			DontDestroyOnLoad(this);
		}

		/// <summary>
		/// Performs the requested action on the Unity Main Thread. If this is called already on the Unity Main Thread it will 
		/// perform the action immediately. Otherwise, the action is added to an event pump and will be executed in the Update 
		/// cycle at the next available time.
		/// NOTE: the number of actions performed per Update cycle is limited to ActionsAllowedPerFrame. Modify this value to 
		/// limit/increase the number performed.
		/// </summary>
		/// <param name="action"></param>
		public void RunOnMain(Action action) {
			if (action == null) {
				return;
			}
			if (IsRunningOnMain) {
				action?.Invoke();
			} else {
				__eventPump.Enqueue(action);
			}
		}

		private void Update() {
			if (__eventPump.Count == 0) {
				return;
			}

			if (ActionsAllowedPerFrame == INFINITE_ACTIONS) {
				// perform all the actions queued in the eventPump
				// NOTE: this can be detrimental to performance. Use with caution!
				while (__eventPump.Count > 0) {
					__eventPump.Dequeue()?.Invoke();
				}
			} else {
				// loops through the queued actions up to the limit of ActionAllowedPerFrame
				for (int count = ActionsAllowedPerFrame; count >= 0 && __eventPump.Count > 0; count--) {
					__eventPump.Dequeue()?.Invoke();
				}
			}
		}

	}

	private static Handler handler;
	/// <summary>
	/// Constant value used to allow infinite actions performed on the main thread in a single Update cycle.
	/// </summary>
	public const int INFINITE_ACTIONS = -1;

	/// <summary>
	/// Creates the handler object that handles all 'GameObject-independant' Coroutines.
	/// </summary>
	static CoroutineUtils() {
		GameObject go = new GameObject("Handler Object");
		handler = go.AddComponent<Handler>();
		go.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable;
	}

	/// <summary>
	/// Sets the maximum number of actions allowed to be performed in a single frame Update cycle on the Main Thread event pump 
	/// for performance reasons. To allow for an infinite number of actions per cycle set this to GameObjectUtils.INFINITE_ACTIONS.
	/// </summary>
	public static int ActionsAllowedOnMainThreadPerFrame {
		get { return handler.ActionsAllowedPerFrame; }
		set {
			if (value <= INFINITE_ACTIONS) {
				Debug.LogWarning("ActionsAllowedOnMainThreadPerFrame has been set to Infinite. This can have a severe impact " +
					"on application performance. Use this setting with caution!");
				value = INFINITE_ACTIONS;
			}
			handler.ActionsAllowedPerFrame = value;
		}
	}

	/// <summary>
	/// Returns true if this method is called from Unity's Main Thread, otherwise returns false.
	/// </summary>
	/// <returns></returns>
	public static bool IsRunningOnUnityMainThread() => handler?.IsRunningOnMain ?? false;

	/// <summary>
	/// Assigns that action to be executed on UnityMainThread in the next Update cycle.
	/// </summary>
	/// <param name="action">The action to perform</param>
	public static void InvokeOnMain(Action action) => handler?.RunOnMain(action);

	/// <summary>
	/// Delays the requested action until the next frame.
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	private static IEnumerator __NextFrame(Action action) {
		yield return null;
		action?.Invoke();
	}

	/// <summary>
	/// Delays the requested action until the IEnumerator returns.
	/// </summary>
	/// <param name="yield"></param>
	/// <param name="action"></param>
	/// <returns></returns>
	private static IEnumerator __Delay(IEnumerator yield, Action action) {
		yield return yield;
		action?.Invoke();
	}

	/// <summary>
	/// Delays the requested action until the Coroutine returns.
	/// </summary>
	/// <param name="co"></param>
	/// <param name="action"></param>
	/// <returns></returns>
	private static IEnumerator __Delay(Coroutine co, Action action) {
		yield return co;
		action?.Invoke();
	}

	/// <summary>
	/// Delays the requested action until the YieldInstruction returns, ie. new WaitForSecondsRealtime(2).
	/// </summary>
	/// <param name="instruction">A YieldInstruction indicating how to wait until the action is performed</param>
	/// <param name="action">The action to perform</param>
	/// <returns></returns>
	private static IEnumerator __Delay(YieldInstruction instruction, Action action) {
		yield return instruction;
		action?.Invoke();
	}

	/// <summary>
	/// Delays the requested action until the CustomYieldInstruction returns.
	/// </summary>
	/// <param name="instruction">A CustomYieldInstruction indicating how to wait until the action is performed</param>
	/// <param name="action">The action to perform</param>
	/// <returns></returns>
	private static IEnumerator __Delay(CustomYieldInstruction instruction, Action action) {
		yield return instruction;
		action?.Invoke();
	}

	/// <summary>
	/// Repeatadly performs the action on a loop while loopWhile == true with a custom delay of YieldInstruction.
	/// </summary>
	/// <param name="instruction">A YieldInstruction indicating how to wait until the action is performed</param>
	/// <param name="action">The action to perform</param>
	/// <param name="loopWhile">A bool indication whether to keep looping</param>
	/// <returns></returns>
	private static IEnumerator __Loop(YieldInstruction instruction, Action action, Func<bool> loopWhile) {
		while (loopWhile.Invoke()) {
			yield return instruction;
			action?.Invoke();
		}
	}

	/// <summary>
	/// Waits for the given amount of seconds, then performs the action.
	/// </summary>
	/// <param name="secondsToDelay">The number of seconds to delay the action</param>
	/// <param name="action">The action to perform</param>
	public static Coroutine Delay(float secondsToDelay, Action action) => handler?.StartCoroutine(__Delay(new WaitForSeconds(secondsToDelay), action));

	/// <summary>
	/// Waits for the next frame, then perfoms the action.
	/// </summary>
	/// <param name="action">The action to perform</param>
	public static Coroutine NextFrame(Action action) => handler?.StartCoroutine(__NextFrame(action));

	/// <summary>
	/// Repeatadly performs the action on a loop while loopWhile == true with a custom delay of YieldInstruction.
	/// </summary>
	/// <param name="instruction">A YieldInstruction indicating how to wait until the action is performed</param>
	/// <param name="action">The action to perform</param>
	/// <param name="loopWhile">A bool indication whether to keep looping</param>
	/// <returns></returns>
	public static Coroutine LoopWhile(YieldInstruction instruction, Action action, Func<bool> loopWhile) => handler?.StartCoroutine(__Loop(instruction, action, loopWhile));

	/// <summary>
	/// Waits until the supplied IEnumerator completes in a Unity Coroutine before invoking the supplied action.
	/// </summary>
	/// <param name="yield"></param>
	/// <param name="action"></param>
	/// <returns></returns>
	public static Coroutine Wait(IEnumerator yield, Action action) => handler?.StartCoroutine(__Delay(yield, action));

	/// <summary>
	/// Waits until the supplied Unity Coroutine completes before invoking the supplied action.
	/// </summary>
	/// <param name="co"></param>
	/// <param name="action"></param>
	/// <returns></returns>
	public static Coroutine Wait(Coroutine co, Action action) => handler?.StartCoroutine(__Delay(co, action));

	/// <summary>
	/// Delays the requested action until the YieldInstruction returns, ie. new WaitForSecondsRealtime(2).
	/// </summary>
	/// <param name="instruction">A YieldInstruction indicating how to wait until the action is performed</param>
	/// <param name="action">The action to perform</param>
	/// <returns></returns>
	public static Coroutine Wait(YieldInstruction instruction, Action action) => handler?.StartCoroutine(__Delay(instruction, action));

	/// <summary>
	/// Delays the requested action until the CustomYieldInstruction returns
	/// </summary>
	/// <param name="instruction">A CustomYieldInstruction indicating how to wait until the action is performed</param>
	/// <param name="action">The action to perform</param>
	/// <returns></returns>
	public static Coroutine Wait(CustomYieldInstruction instruction, Action action) => handler?.StartCoroutine(__Delay(instruction, action));

	/// <summary>
	/// Stops the requested Coroutine and returns a boolean value on it's succession
	/// </summary>
	/// <param name="coroutine">The Coroutine to stop</param>
	/// <returns></returns>
	public static bool StopCoroutine(Coroutine coroutine) {
		if (coroutine == null) {
			return true;
		}
		try {
			InvokeOnMain(() => handler.StopCoroutine(coroutine));
		} catch {
			// this should never happen. EVER!
			return false;
		}
		return true;
	}

	/// <summary>
	/// Starts the requested IEnumerator as a Coroutine handled on the Handler object
	/// </summary>
	/// <param name="co">The IEnumerator to start as a Coroutine</param>
	/// <returns></returns>
	public static Coroutine AsCoroutine(this IEnumerator co) {
		return handler.StartCoroutine(co);
	}
}
