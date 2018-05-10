using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ascend : MonoBehaviour {

	public float ascendSpeed = 1.0f;

	private float delta = 0.0f;
	private Vector3 startPos;
	private Vector3 endPos;
	private bool ascending = false;
	private bool ascendDone = false;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		endPos = startPos + (Vector3.up * 10);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.K))
			ascending = true;

		if (ascending && !ascendDone)
			Ascending();
	}

	private void Ascending() {
		delta += Time.deltaTime * ascendSpeed;
		transform.position = Vector3.Lerp(startPos, endPos, delta);

		if (delta >= 1.0f) {
			ascendDone = true;
		}
	}
}
