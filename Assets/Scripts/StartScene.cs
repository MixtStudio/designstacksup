﻿using System;
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
		PRESS_ANY_BUTTON = Prompts.GetPrompt(new Vector3(transform.position.x, transform.position.y + offset.y, transform.position.z+offset.z),Prompts.PromptName.PRESS_ANY_BUTTON_TO_START);
		Vector3 direction = PRESS_ANY_BUTTON.transform.position - Camera.main.transform.position;
		PRESS_ANY_BUTTON.transform.rotation = Quaternion.LookRotation(direction.normalized);
		PRESS_ANY_BUTTON.transform.localScale *= .4f;
	}
	
	void Update () {

		if (PRESS_ANY_BUTTON != null) {
			Vector3 direction = PRESS_ANY_BUTTON.transform.position - Camera.main.transform.position;
			PRESS_ANY_BUTTON.transform.rotation = Quaternion.LookRotation(direction.normalized);
		}

		if (!begin) {
			interactionSourceStates = InteractionManager.GetCurrentReading();
			foreach (InteractionSourceState interactionSourceState in interactionSourceStates) {
				if (interactionSourceState.anyPressed) {
					MediaPlayerManager.Instance.Begin();
					AudioManager.Instance.NowPlay(AudioManager.Audio.IntroText, false, true);
					Prompts.DestroyPrompt(PRESS_ANY_BUTTON);
					begin = true;
					break;
				}
			}
		}
	}
}
