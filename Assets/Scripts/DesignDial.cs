using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignDial : MonoBehaviour {

	private Vector3 spawnPos;

	public void SetSpawnPos(Vector3 pos) { spawnPos = pos; }
	public Vector3 GetSpawnPos() { return spawnPos; }

	public void Respawn() {
		transform.localPosition = spawnPos;
	}

	void OnEnable() {
		spawnPos = transform.position;
	}
	
}
