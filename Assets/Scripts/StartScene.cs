using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using HoloToolkit.Unity;

public class StartScene : MonoBehaviour {

	private InteractionSourceState[] interactionSourceStates;
	private bool begin;
	private DynamicTextController PRESS_ANY_BUTTON;
	private Vector3 offset = new Vector3(0,2,1.5f);

	private Action Temp1;
	private Action Temp2;
	
	void Start () {
		FadeManager.Instance.DoFade(0.0f, 0.0f, Temp1, Temp2);
		PRESS_ANY_BUTTON = Prompts.GetPrompt(new Vector3(transform.position.x, transform.position.y + offset.y, transform.position.z+offset.z),Quaternion.identity,Prompts.PromptName.PRESS_ANY_BUTTON_TO_START,.4f);
		PRESS_ANY_BUTTON.transform.rotation = TransformUtils.GetLookAtRotation(PRESS_ANY_BUTTON.transform);

	}
	
	void Update () {

		if (PRESS_ANY_BUTTON != null) {
			PRESS_ANY_BUTTON.transform.rotation = TransformUtils.GetLookAtRotation(PRESS_ANY_BUTTON.transform);
		}

		if (!begin) {
			interactionSourceStates = InteractionManager.GetCurrentReading();
			foreach (InteractionSourceState interactionSourceState in interactionSourceStates) {
				if (interactionSourceState.anyPressed) {
					MediaPlayerManager.Instance.Begin(true);
					AudioManager.Instance.NowPlay(AudioManager.Audio.IntroText, false, true);
					Prompts.DestroyPrompt(PRESS_ANY_BUTTON);
					begin = true;
					break;
				}
			}
		}
	}
}
