using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ascend : MonoBehaviour {

	public float ascendSpeed = 1.0f;
	public GameObject CloudEffect;

	private int cloudTrigger = 2;
	private int ascendCount;

	private float delta = 0.0f;
	private Vector3 startPos;
	private Vector3 endPos;
	private bool ascending = false;

	void OnEnable() {
		CloudEffect.SetActive(false);
	}
	
	public void StartAscending() {
		startPos = transform.position;
		endPos = startPos + (Vector3.up * 10);
		ascending = true;
		delta = 0.0f;

		ascendCount++;
		if (ascendCount == cloudTrigger)
			CloudEffect.SetActive(true);
	}

	void Update () {
		if (ascending)
			Ascending();
	}

	private void Ascending() {
		delta += Time.deltaTime * ascendSpeed;
		transform.position = Vector3.Lerp(startPos, endPos, delta);

		if (delta >= 1.0f) {
			ascending = false;
		}
	}
}
