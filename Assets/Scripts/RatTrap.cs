using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using TMPro;

public class RatTrap : MonoBehaviour {

	public float revealRadius = 3.0f;
	[SerializeField]
	private float rotationSpeed;
	
	private int number_of_Spawns;
	private int SpawnCount = 1;
	private Vector3 spawnPos = Vector3.zero;
	private Quaternion spawnRot;
	private GameObject[] hiddenObjs;
	private RevealManager revealManager;
	private TransitionManager transitionManager;
	private HandDraggable handDraggable;
	private bool fallingCheck = false;
	private Rigidbody rg;
	private AudioManager audioManager;
	private bool allowMultipleSpawn = true;
	private bool canRotate = true;

	private DynamicTextController GN_INTRO;
	private DynamicTextController GN_DIAL_UP;
	private DynamicTextController GN_FACT;

	private Vector3 offset = new Vector3(.4f,.2f,0);



	public void SetSpawnPosition(Vector3 pos) { spawnPos = pos; }

	// Use this for initialization
	void OnEnable() {
		audioManager = FindObjectOfType<AudioManager>();
		revealManager = FindObjectOfType<RevealManager>();
		number_of_Spawns = revealManager.revealNumThreshold;
		transitionManager = FindObjectOfType<TransitionManager>();
		handDraggable = GetComponent<HandDraggable>();
		handDraggable.StartedDragging += DraggingStart;
		spawnRot = transform.rotation;
		rg = GetComponent<Rigidbody>();
		

		GN_INTRO = Prompts.GetPrompt(new Vector3(transform.position.x+offset.x, transform.position.y + offset.y, transform.position.z), Prompts.PromptName.GN_INTRO);
		GN_INTRO.transform.localScale *= .15f;

		GN_DIAL_UP = Prompts.GetPrompt(new Vector3(transform.position.x + offset.x+.05f, transform.position.y + offset.y, transform.position.z), Prompts.PromptName.GN_DIAL_UP_DESIGN);
		GN_DIAL_UP.transform.localScale *= .15f;

	}

	private void Update() {
		if (GN_DIAL_UP != null) {
			GN_DIAL_UP.transform.position = new Vector3(transform.position.x + offset.x + .2f, transform.position.y + offset.y, transform.position.z);
		}
		
	}

	private void DraggingStart() {
		audioManager.NowPlay(AudioManager.Audio.TrapRattle);
		if (!fallingCheck) {
			if (transitionManager != null)
				transitionManager.BeginFalling();
			else
				GameObject.FindObjectOfType<TransitionManagerScaling>().BeginDisappear();
			fallingCheck = true;
			canRotate = false;
		}

		Prompts.DestroyPrompt(GN_INTRO);
	}

	void OnCollisionEnter(Collision col) {
		if(col.collider.gameObject.tag == "Floor") {
			audioManager.NowPlay(AudioManager.Audio.TrapImpact);
			if (hiddenObjs == null) {
				hiddenObjs = revealManager.GetHiddenObjects();
			}
			if (SpawnCount == 2 && allowMultipleSpawn) {
				SpawnMultiple(transform.position, transform.rotation);
			}
			Prompts.DestroyPrompt(GN_DIAL_UP);
			Spawn_GN_FACT();
			RevealArea();
		}
	}

	void OnCollisionExit(Collision col) {
		if (col.collider.gameObject.tag == "Pedastal") {
			if (!fallingCheck) {
				if (transitionManager != null)
					transitionManager.BeginFalling();
				else
					FindObjectOfType<TransitionManagerScaling>().BeginDisappear();

				fallingCheck = true;
			}
		}
	}

	private void RevealArea() {
		Debug.Log("Revealing Area");
		revealManager.IncrementRevealNum(transform.position, revealRadius);
		if (SpawnCount < number_of_Spawns) {
			Respawn();
		}
			
		else {
			rg.isKinematic = true;
			handDraggable.IsDraggingEnabled = false;
		}
	}

	private void Respawn() {
		//instantiate a copy
	
		GameObject copyObj = Instantiate(gameObject);
		copyObj.transform.position = transform.position;
		copyObj.transform.rotation = transform.rotation;
		copyObj.GetComponent<Rigidbody>().isKinematic = true;
		Destroy(copyObj.GetComponent<HandDraggable>());
		Destroy(copyObj.GetComponent<RatTrap>());
		rg.velocity = Vector3.zero;
		rg.angularVelocity = Vector3.zero;
		transform.position = spawnPos;
		transform.rotation = spawnRot;
		SpawnCount++;
		Debug.Log("SpawnCount: "+SpawnCount);
		canRotate = true;
	}

	private void SpawnMultiple(Vector3 spawnPoint, Quaternion spawnRotation) {

		for (int i = 0; i < 100; i++) {
			GameObject copyObj = Instantiate(gameObject);
			copyObj.transform.position = spawnPoint;
			copyObj.transform.rotation = spawnRotation;
			Rigidbody copyRG = copyObj.GetComponent<Rigidbody>();
			copyRG.AddForce(Vector3.up*30);
		
			Destroy(copyObj.GetComponent<HandDraggable>());
			Destroy(copyObj.GetComponent<RatTrap>());
		}

		allowMultipleSpawn = false;
	}

	private void Spawn_GN_FACT() {
		Vector3 GN_FACT_POSITION = new Vector3(.558f, 1.827f, 1.137f);
		//Vector3 GN_FACT_POSITION = new Vector3(4f, 3, 1.137f);
		Prompts.PromptName prompt;
		switch (SpawnCount) {
			case 1:
				prompt = Prompts.PromptName.GN_FACT_1;
				break;

			case 2:
				prompt = Prompts.PromptName.GN_FACT_2;
				break;

			case 3:
				prompt = Prompts.PromptName.GN_FACT_3;
				break;

			default:
				prompt = Prompts.PromptName.GN_FACT_1;
				return;
		}

		GN_FACT = Prompts.GetPrompt(GN_FACT, GN_FACT_POSITION, prompt);
		GN_FACT.transform.localScale *= .8f;
	}

	private void Update_GN_PROMPTS() {
		//Vector3 direction;

		//if (GN_INTRO != null) {
		//	direction= GN_INTRO.transform.position - Camera.main.transform.position;
		//	GN_INTRO.transform.rotation = Quaternion.LookRotation(direction.normalized);
		//}

		//if (GN_DIAL_UP!= null) {
		//	direction = GN_DIAL_UP.transform.position - Camera.main.transform.position;
		//	GN_DIAL_UP.transform.rotation = Quaternion.LookRotation(direction.normalized);
		//}


		//if (GN_FACT != null) {
		//	direction = GN_FACT.transform.position - Camera.main.transform.position;
		//	GN_FACT.transform.rotation = Quaternion.LookRotation(direction.normalized);
		//}
	}

}
