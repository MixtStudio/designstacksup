using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using TMPro;

public class RatTrap : HandDraggable, IPooledObjects {


	[SerializeField]
	private float rotationSpeed;
	public bool interactiveTrap {
		get { return IsDraggingEnabled; }
		set { IsDraggingEnabled = value; }
	}
	private Vector3 spawnPos { get; set; } 
	private Quaternion spawnRot { get; set; }

	private DynamicTextController GN_INTRO;
	private DynamicTextController GN_DIAL_UP;
	private static bool scale_flag=true;
	private Vector3 offset = new Vector3(.4f,.2f,0);

	public void OnObjectSpawn(bool interactive) {

		AudioManager.Instance.NowPlay(AudioManager.Audio.TrapSpawn);

		interactiveTrap = interactive;
		HostTransform = gameObject.transform;

		if (interactiveTrap) {

			if (RatTrapSpawner.interactiveRatTrapsCount == 1) {
				GN_INTRO = Prompts.GetPrompt(new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z), Quaternion.identity, Prompts.PromptName.GN_INTRO, .15f);
				GN_INTRO.transform.rotation = TransformUtils.GetLookAtRotation(GN_INTRO.transform);
			}

			Vector3 GN_DIAL_POSITION = new Vector3(transform.position.x + offset.x, transform.position.y - .2f + offset.y, transform.position.z);
			GN_DIAL_UP = Prompts.GetPrompt(GN_DIAL_UP, GN_DIAL_POSITION, Quaternion.identity, Prompts.PromptName.GN_DIAL_UP_DESIGN);
			GN_DIAL_UP.transform.rotation = TransformUtils.GetLookAtRotation(GN_DIAL_UP.transform);

			if (scale_flag) {
				GN_DIAL_UP.transform.localScale *= .15f;
				scale_flag = false;
			}
		} 
		
		//else
			//IsDraggingEnabled = false;
	}


	protected override void Update() {
		base.Update();
		if (!interactiveTrap) {
			return;
		}
		
		transform.Rotate(Vector3.up, 20 * Time.deltaTime);
		Update_GN_PROMPTS();
	}
	
	protected override void UpdateDragging() => base.UpdateDragging();

	protected override void StartDragging(Vector3 initialDraggingPosition) {
		base.StartDragging(initialDraggingPosition);

		AudioManager.Instance.NowPlay(AudioManager.Audio.TrapRattle);

		Debug.Log("Is RatTrap interactive: " + interactiveTrap);

		if (RatTrapSpawner.interactiveRatTrapsCount == 1) {
			TransitionManager.Instance.BeginFalling();
		}
	}

	void OnCollisionEnter(Collision col) {
		if(col.collider.gameObject.tag == "Floor") {

			AudioManager.Instance.NowPlay(AudioManager.Audio.TrapImpact);

			if (!interactiveTrap)
				return;

			else {
				RatTrapSpawner.Instance.Spawn_GN_FACT();
				RatTrapSpawner.Instance.RevealArea();

				switch (RatTrapSpawner.interactiveRatTrapsCount) {
					case 1:
						Prompts.DestroyPrompt(GN_INTRO);
						break;
					case 2:
						RatTrapSpawner.Instance.SpawnMultiple(50, transform.position, transform.rotation);
						break;
					case 3:
						Prompts.DestroyPrompt(GN_DIAL_UP);
						break;
				}
				interactiveTrap = false;
			}	
		}
	}


	private void Update_GN_PROMPTS() {
	
		if (GN_DIAL_UP != null) {
			GN_DIAL_UP.transform.position = new Vector3(transform.position.x + offset.x, transform.position.y - .2f + offset.y, transform.position.z);
			GN_DIAL_UP.transform.rotation = TransformUtils.GetLookAtRotation(GN_DIAL_UP.transform);
			
		}

		if (GN_INTRO != null) {
			GN_INTRO.transform.rotation = TransformUtils.GetLookAtRotation(GN_INTRO.transform);
		
		}
	}
}
