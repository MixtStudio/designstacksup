using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class StartScene : MonoBehaviour {

	private InteractionSourceState[] interactionSourceStates;
	private bool begin;

	void Update () {
		if (!begin) {
			interactionSourceStates = InteractionManager.GetCurrentReading();
			foreach (InteractionSourceState interactionSourceState in interactionSourceStates) {
				if (interactionSourceState.anyPressed) {
					MediaPlayerManager.Instance.Begin();
					begin = true;
					break;
				}
			}
		}
	}
}
