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
	private static int SpawnCount = 1;
	private Vector3 spawnPos = Vector3.zero;
	private Quaternion spawnRot;
	private GameObject[] hiddenObjs;
	private HandDraggable handDraggable;
	private bool fallingCheck = false;
	private Rigidbody rg;
	private static bool allowMultipleSpawn = true;
	private static bool canRotate;
	

	private DynamicTextController GN_INTRO;
	private DynamicTextController GN_DIAL_UP;
	private static bool scale_flag=true;
	private DynamicTextController GN_FACT;

	private Vector3 offset = new Vector3(.4f,.2f,0);
	private static bool rotateLookCamera = false;
	public void SetSpawnPosition(Vector3 pos) { spawnPos = pos; }


	//private bool spawn_GN_INTRO = true;

	void OnEnable() {
		canRotate = true;
		number_of_Spawns = RevealManager.Instance.revealNumThreshold;
		handDraggable = GetComponent<HandDraggable>();
		handDraggable.StartedDragging += DraggingStart;
		spawnRot = transform.rotation;
		rg = GetComponent<Rigidbody>();

		if (SpawnCount == 1) {
			GN_INTRO = Prompts.GetPrompt(new Vector3(transform.position.x+ offset.x, transform.position.y + offset.y, transform.position.z), Quaternion.identity, Prompts.PromptName.GN_INTRO, .15f);
			GN_INTRO.transform.rotation = TransformUtils.GetLookAtRotation(GN_INTRO.transform);
		}

		Vector3 GN_DIAL_POSITION = new Vector3(transform.position.x + offset.x, transform.position.y - .2f + offset.y, transform.position.z);
		GN_DIAL_UP = Prompts.GetPrompt(GN_DIAL_UP,GN_DIAL_POSITION, Quaternion.identity, Prompts.PromptName.GN_DIAL_UP_DESIGN);
		GN_DIAL_UP.transform.rotation = TransformUtils.GetLookAtRotation(GN_DIAL_UP.transform);

		if (scale_flag) {
			GN_DIAL_UP.transform.localScale *=.15f;
			scale_flag = false;
		}

		AudioManager.Instance.NowPlay(AudioManager.Audio.TrapSpawn);
	}

	private void Update() {
		if (canRotate) {
			transform.Rotate(Vector3.up, 20 * Time.deltaTime);
		}

		Update_GN_PROMPTS();
	}

	private void GetSpawnPos() {
		Vector3 spawnPos = Camera.main.transform.position + Camera.main.transform.forward * 3;
	}

	private void DraggingStart() {
		rotateLookCamera = true;
		canRotate = false;
		AudioManager.Instance.NowPlay(AudioManager.Audio.TrapRattle);
		if (!fallingCheck) {
			TransitionManager.Instance.BeginFalling();
			fallingCheck = true;
		}

		Prompts.DestroyPrompt(GN_INTRO);
	}

	void OnCollisionEnter(Collision col) {
		if(col.collider.gameObject.tag == "Floor") {
			AudioManager.Instance.NowPlay(AudioManager.Audio.TrapImpact);
			if (hiddenObjs == null) {
				hiddenObjs = RevealManager.Instance.GetHiddenObjects();
			}
			if (SpawnCount == 2 && allowMultipleSpawn) {

				SpawnMultiple(transform.position, transform.rotation);
			}
			//Prompts.DestroyPrompt(GN_DIAL_UP);
			Spawn_GN_FACT();
			RevealArea();
		}
	}

	void OnCollisionExit(Collision col) {
		if (col.collider.gameObject.tag == "Pedastal") {
			TransitionManager.Instance.BeginFalling();
			fallingCheck = true;
		}		
	}

	private void RevealArea() {
		Debug.Log("Revealing Area");
		RevealManager.Instance.IncrementRevealNum(transform.position, revealRadius);
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
		SpawnCount++;
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
		Debug.Log("SpawnCount: "+SpawnCount);
		AudioManager.Instance.NowPlay(AudioManager.Audio.TrapSpawn);
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
		Vector3 GN_FACT_POSITION = TransformUtils.GetLookAtPosition(2);
		GN_FACT_POSITION = new Vector3(GN_FACT_POSITION.x, GN_FACT_POSITION.y + 1.5f, GN_FACT_POSITION.z);
		
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
				Prompts.DestroyPrompt(GN_DIAL_UP);
				break;

			default:
				prompt = Prompts.PromptName.GN_FACT_1;
				return;
		}

		GN_FACT = Prompts.GetPrompt(GN_FACT, GN_FACT_POSITION,Quaternion.identity, prompt,.5f);
		GN_FACT.transform.rotation= TransformUtils.GetLookAtRotation(GN_FACT.transform);
	}


	private void Update_GN_PROMPTS() {
	
		if (GN_DIAL_UP != null) {
			GN_DIAL_UP.transform.position = new Vector3(transform.position.x + offset.x, transform.position.y - .2f + offset.y, transform.position.z);
			GN_DIAL_UP.transform.rotation = TransformUtils.GetLookAtRotation(GN_DIAL_UP.transform);
		}


		if (GN_INTRO != null) {
			//.transform.position = new Vector3(transform.position.x + offset.x, transform.position.y - .2f + offset.y, transform.position.z);
			GN_INTRO.transform.rotation = TransformUtils.GetLookAtRotation(GN_INTRO.transform);
		}
	}

	public void DestroGN_FACT() {
		Prompts.DestroyPrompt(GN_FACT);
	}
}
