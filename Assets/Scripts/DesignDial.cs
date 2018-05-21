using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class DesignDial : MonoBehaviour {

	private Vector3 spawnPos;
	private HandDraggable handDraggable;
	private DynamicTextController DIAL_UP_DESIGN;
	private Vector3 DIAL_UP_DESIGN_POS;
	private float offset = .5f;


	public void SetSpawnPos(Vector3 pos) { spawnPos = pos; }
	public Vector3 GetSpawnPos() { return spawnPos; }

	public void Despawn() {
		handDraggable.SetDragging(false);
	}
	
	public void Respawn() {
		transform.localPosition = spawnPos;
		handDraggable.SetDragging(true);
	}

	void Start() {
		spawnPos = transform.localPosition;
		handDraggable = GetComponent<HandDraggable>();
		handDraggable.StartedDragging += PlayAudio;
		handDraggable.StoppedDragging += StopAudio;
	}
	
	private void PlayAudio() {
		AudioManager.Instance.NowPlay(AudioManager.Audio.UserControl, true, true);
	}
	
	private void StopAudio() {
		AudioManager.Instance.NowStop(AudioManager.Audio.UserControl);
		//DIAL_UP_DESIGN_POS = new Vector3(this.position.x, HostTransform.position.y + offset, HostTransform.position.z);
		//DIAL_UP_DESIGN = Prompts.Spawn_Prompt(DIAL_UP_DESIGN_POS, Prompts.PromptName.SCN1_DIAL_UP_DESIGN);
	}
	
}
