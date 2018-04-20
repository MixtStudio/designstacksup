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

	public void SetSpawnPosition(Vector3 pos) { spawnPos = pos; }

	// Use this for initialization
	void OnEnable() {
		revealManager = GameObject.FindObjectOfType<RevealManager>();
		number_of_Spawns = revealManager.revealNumThreshold;
		transitionManager = FindObjectOfType<TransitionManager>();
		handDraggable = GetComponent<HandDraggable>();
		handDraggable.StartedDragging += DraggingStart;
		spawnRot = transform.rotation;
		rg = GetComponent<Rigidbody>();
	}

	private void DraggingStart() {
		Debug.Log("DraggingStart");
		if (!fallingCheck) {
			transitionManager.BeginFalling();
			fallingCheck = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col) {
		if(col.collider.gameObject.tag == "Floor") {
			hiddenObjs = revealManager.GetHiddenObjects();
			RevealArea();
		}
	}

	private void RevealArea() {
		Debug.Log("Revealing Area");
		foreach(GameObject obj in hiddenObjs) {
			if (Vector3.Distance(transform.position, obj.transform.position) <= revealRadius) {
				Debug.Log("Revealing Obj");
				obj.GetComponent<MeshRenderer>().enabled = true;
			}
		}
		revealManager.IncrementRevealNum();

		if(SpawnCount < number_of_Spawns)
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
	}
}
