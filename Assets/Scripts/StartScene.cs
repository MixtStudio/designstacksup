using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class StartScene : MonoBehaviour {

	private MediaPlayerManager mpManager;
	private InteractionSourceState[] interactionSourceStates;
	private bool begin;

	// Use this for initialization
	void Start () {
		mpManager = FindObjectOfType<MediaPlayerManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!begin) {
			interactionSourceStates = InteractionManager.GetCurrentReading();
			foreach (InteractionSourceState interactionSourceState in interactionSourceStates) {
				if (interactionSourceState.anyPressed) {
					mpManager.Begin();
					begin = true;
					break;
				}
			}
		}
	}
}
