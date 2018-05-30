using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class DesignDial : MonoBehaviour {

	private Vector3 spawnPos;
	private HandDraggable handDraggable;
	private DynamicTextController DIAL_UP_DESIGN;
	private Vector3 DIAL_UP_DESIGN_POS;
	//private float offset = .5f;


	public void SetSpawnPos(Vector3 pos) { spawnPos = pos; }
	public Vector3 GetSpawnPos() { return spawnPos; }

	private void Update() {
		Update_DIAL_UP_DESIGN();
	}

	public void Despawn() {
		handDraggable.SetDragging(false);
	}
	
	public void Respawn() {
		transform.localPosition = spawnPos;
		handDraggable.SetDragging(true);
		AudioManager.Instance.NowPlay(AudioManager.Audio.DesignBallSpawn);
	}

	void Start() {
		spawnPos = transform.localPosition;
		Quaternion rotation = TransformUtils.GetLookAtRotation(transform);
		DIAL_UP_DESIGN = Prompts.GetPrompt(new Vector3(transform.position.x, transform.position.y+.2f, transform.position.z), rotation,Prompts.PromptName.AB_DIAL_UP_DESIGN,.15f);
		handDraggable = GetComponent<HandDraggable>();
		handDraggable.StartedDragging += PlayAudio;
		handDraggable.StoppedDragging += StopAudio;
		AudioManager.Instance.NowPlay(AudioManager.Audio.DesignBallSpawn);
	}
	
	private void PlayAudio() {
		AudioManager.Instance.NowPlay(AudioManager.Audio.UserControl, true, true);
	}


	private void StopAudio() {
		AudioManager.Instance.NowStop(AudioManager.Audio.UserControl);
	}

	private void Update_DIAL_UP_DESIGN() {
		if (DIAL_UP_DESIGN != null) {
			DIAL_UP_DESIGN.transform.position = new Vector3(transform.position.x, transform.position.y+.2f, transform.position.z);
			DIAL_UP_DESIGN.transform.rotation = TransformUtils.GetLookAtRotation(DIAL_UP_DESIGN.transform);
		}
	}

	
}
