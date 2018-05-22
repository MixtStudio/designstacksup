using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class StartScene : MonoBehaviour {

	private InteractionSourceState[] interactionSourceStates;
	private bool begin;
	private DynamicTextController PRESS_ANY_BUTTON;
	private Vector3 offset = new Vector3(0,2,1.5f);

	void Start () {
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
					MediaPlayerManager.Instance.Begin();
					Prompts.DestroyPrompt(PRESS_ANY_BUTTON);
					begin = true;
					break;
				}
			}
		}
	}
}
