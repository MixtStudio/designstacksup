using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using TMPro;

public class RatTrap : MonoBehaviour, IPooledObjects {

	[SerializeField]
	private float rotationSpeed;
	public bool interactiveTrap {
		get { return true; }set { }}
	private Vector3 spawnPos { get; set; } 
	private Quaternion spawnRot { get; set; }

	private DynamicTextController GN_INTRO;
	private DynamicTextController GN_DIAL_UP;
	private static bool scale_flag=true;
	private Vector3 offset = new Vector3(.4f,.2f,0);
	private HandDraggable handDraggable;
	private Rigidbody rb;

	public void OnObjectSpawn(bool interactive) {
		interactiveTrap = interactive;
		AudioManager.Instance.NowPlay(AudioManager.Audio.TrapSpawn);
	

		if (interactiveTrap) {
			handDraggable = GetComponent<HandDraggable>();
			rb = GetComponent<Rigidbody>();
			RatTrapSpawner.interactiveRatTrapsCount++;
			handDraggable.StartedDragging += DraggingStart;
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
		
		else {
			Destroy(this.GetComponent<HandDraggable>());
			Destroy(this.GetComponent<RatTrap>());
		}
	}


	private void Update() {
		transform.Rotate(Vector3.up, 20 * Time.deltaTime);
		Update_GN_PROMPTS();
		Debug.Log("Interactive traps " + RatTrapSpawner.interactiveRatTrapsCount);
	}


	private void DraggingStart() {
		Debug.Log("I AM DRAG");

		AudioManager.Instance.NowPlay(AudioManager.Audio.TrapRattle);
		Prompts.DestroyPrompt(GN_INTRO);
		Debug.Log("Is RatTrap interactive: " + interactiveTrap);

		if (RatTrapSpawner.interactiveRatTrapsCount == 1) {
			TransitionManager.Instance.BeginFalling();
		}
	}

	void OnCollisionEnter(Collision col) {
		if (col.collider.gameObject.tag == "Floor") {

			AudioManager.Instance.NowPlay(AudioManager.Audio.TrapImpact);

			rb.isKinematic = false;
			RatTrapSpawner.Instance.Spawn_GN_FACT();
			RevealManager.Instance.RevealArea(gameObject.transform);
				
			switch (RatTrapSpawner.interactiveRatTrapsCount) {
				case 2:
					Debug.Log("I NEVER ENTER SHOULD SPAWN MULTIPLE");
					RatTrapSpawner.Instance.SpawnMultiple(50, transform.position, transform.rotation);
					break;
				case 3:
					Debug.Log("I NEVER ENTER ");
					Prompts.DestroyPrompt(GN_DIAL_UP);
					break;
				}

			Destroy(gameObject.GetComponent<HandDraggable>());
			Destroy(gameObject.GetComponent<RatTrap>());
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
