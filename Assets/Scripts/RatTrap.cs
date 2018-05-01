using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class RatTrap : MonoBehaviour {

	public float revealRadius = 3.0f;

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
	private bool allowMultipleSpawn = true;

	public void SetSpawnPosition(Vector3 pos) { spawnPos = pos; }

	// Use this for initialization
	void OnEnable() {
		revealManager = GameObject.FindObjectOfType<RevealManager>();
		number_of_Spawns = revealManager.revealNumThreshold;
		transitionManager = FindObjectOfType<TransitionManager>();
		handDraggable = GetComponent<HandDraggable>();
		handDraggable.StartedDragging += DraggingStart;
		handDraggable.StoppedDragging += DraggingStopped;
		spawnRot = transform.rotation;
		rg = GetComponent<Rigidbody>();
	}

	private void DraggingStart() {
		Debug.Log("DraggingStart");
		if (!fallingCheck) {
			if (transitionManager != null)
				transitionManager.BeginFalling();
			else
				GameObject.FindObjectOfType<TransitionManagerScaling>().BeginDisappear();

			fallingCheck = true;
			//rg.isKinematic = false;
		}
	}

	
	private void DraggingStopped() {
		if (SpawnCount==2 && allowMultipleSpawn)
			SpawnMultiple();
	}

	void OnCollisionEnter(Collision col) {
		if(col.collider.gameObject.tag == "Floor") {
			hiddenObjs = revealManager.GetHiddenObjects();
			RevealArea();
		}
	}

	void OnCollisionExit(Collision col) {
		if (col.collider.gameObject.tag == "Pedastal") {
			if (!fallingCheck) {
				if (transitionManager != null)
					transitionManager.BeginFalling();
				else
					GameObject.FindObjectOfType<TransitionManagerScaling>().BeginDisappear();

				fallingCheck = true;
			}
		}
	}

	private void RevealArea() {
		Debug.Log("Revealing Area");
		revealManager.IncrementRevealNum(transform.position, revealRadius);
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
		Debug.Log(SpawnCount);
	}

	private void SpawnMultiple() {


		for (int i = 0; i < 100; i++) {
			GameObject copyObj = Instantiate(gameObject);
			copyObj.transform.position = handDraggable.HostTransform.position;
			copyObj.transform.rotation = handDraggable.HostTransform.rotation;
			copyObj.GetComponent<Renderer>().material.color = Color.red;
			//float vx = Random.Range(.1f,.3f);
			//float vy = Random.Range(.1f, .3f);
			//float vz = Random.Range(.1f, .3f);
			//float vw = Random.Range(20,30);
			rg.AddForce(Vector3.up*30);
			rg.useGravity = false;
			rg.velocity= new Vector3(0,-0.5f,0); 

			//rg.velocity = new Vector3(vx, vy, vz);

			//rg.velocity = new Vector3(vx, vy, vz);
			//rg.angularVelocity = new Vector3(vx, vy, vz);
			Destroy(copyObj.GetComponent<HandDraggable>());
			Destroy(copyObj.GetComponent<RatTrap>());
		}

		allowMultipleSpawn = false;
	}

}
