using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class StartScene : MonoBehaviour {

	private MediaPlayerManager mpManager;
	private LoadManager loadManager;
	private InteractionSourceState[] interactionSourceStates;
	private bool begin;
	private DynamicTextController PRESS_ANY_BUTTON;
	private float offset = .5f;


	// Use this for initialization
	void Start () {
		mpManager = FindObjectOfType<MediaPlayerManager>();
		loadManager = FindObjectOfType<LoadManager>();
		PRESS_ANY_BUTTON = Prompts.GetPrompt(new Vector3(transform.position.x, transform.position.y + offset, transform.position.z),Prompts.PromptName.PRESS_ANY_BUTTON_TO_START);
		Vector3 direction = transform.position - Camera.main.transform.position;
		PRESS_ANY_BUTTON.transform.rotation = Quaternion.LookRotation(direction.normalized);
		PRESS_ANY_BUTTON.transform.localScale *= .4f;
	}
	
	// Update is called once per frame
	void Update () {
		if (!begin) {
			interactionSourceStates = InteractionManager.GetCurrentReading();
			foreach (InteractionSourceState interactionSourceState in interactionSourceStates) {
				if (interactionSourceState.anyPressed) {
					Prompts.DestroyPrompt(PRESS_ANY_BUTTON);
					mpManager.Begin();
					begin = true;
					break;
				}
			}
		}
	}

	public void LoadScene1() {
		loadManager.AdditiveLoadByName("Scene1additive");
	}
}
