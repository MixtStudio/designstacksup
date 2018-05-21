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
	private HandDraggable handDraggable;
	private bool fallingCheck = false;
	private Rigidbody rg;
	private bool allowMultipleSpawn = true;
	//private bool canRotate = true;
	private DynamicTextController textComp;
	

	public void SetSpawnPosition(Vector3 pos) { spawnPos = pos; }

	void OnEnable() {
		number_of_Spawns = RevealManager.Instance.revealNumThreshold;
		handDraggable = GetComponent<HandDraggable>();
		handDraggable.StartedDragging += DraggingStart;
		spawnRot = transform.rotation;
		rg = GetComponent<Rigidbody>();
		textComp = Prompts.GetPrompt(Prompts.PromptName.GN_INTRO);
	
		textComp.transform.position = new Vector3(2.055f,.691f,.492f);
	}

	//private void Update() {
	//	if (canRotate)
	//		transform.Rotate(0, rotationSpeed, 0);
	//}

	private void DraggingStart() {
		//Debug.Log("DraggingStart");
		AudioManager.Instance.NowPlay(AudioManager.Audio.TrapRattle);
		if (!fallingCheck) {
			TransitionManager.Instance.BeginFalling();
			fallingCheck = true;
			//canRotate = false;
		}
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
		if (SpawnCount < number_of_Spawns)
			Respawn();
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
		//canRotate = true;
	}

	private void SpawnMultiple(Vector3 spawnPoint, Quaternion spawnRotation) {
		for (int i = 0; i < 100; i++) {
			GameObject copyObj = Instantiate(gameObject);
			copyObj.transform.position = spawnPoint;
			copyObj.transform.rotation = spawnRotation;
			copyObj.GetComponent<Renderer>().material.color = Color.red;
			//float vx = Random.Range(.1f,.3f);
			//float vy = Random.Range(.1f, .3f);
			//float vz = Random.Range(.1f, .3f);
			//float vw = Random.Range(20,30);
			Rigidbody copyRG = copyObj.GetComponent<Rigidbody>();
			copyRG.AddForce(Vector3.up*30);
			//copyRG.useGravity = false;
			//copyRG.velocity= new Vector3(0,-0.5f,0); 

			//rg.velocity = new Vector3(vx, vy, vz);

			//rg.velocity = new Vector3(vx, vy, vz);
			//rg.angularVelocity = new Vector3(vx, vy, vz);
			Destroy(copyObj.GetComponent<HandDraggable>());
			Destroy(copyObj.GetComponent<RatTrap>());
		}
		allowMultipleSpawn = false;
	}

}
