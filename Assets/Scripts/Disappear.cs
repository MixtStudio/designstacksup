using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappear : MonoBehaviour {

	public float scaleSpeed = 1.0f;

	private float delta = 0.0f;
	private Vector3 startScale;
	private Vector3 endScale;
	private bool disappearing = false;
	private bool disappearDone = false;

	void OnEnable() {
		endScale = new Vector3(transform.localScale.x, 0.0f, transform.localScale.z);
		startScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

		transform.localScale = startScale;
	}

	void Update() {
		if (disappearing && !disappearDone)
			ScaleDown();
	}

	public void BeginDisappearing() {
		disappearing = true;
	}

	private void ScaleDown() {
		delta += Time.deltaTime * scaleSpeed;
		transform.localScale = Vector3.Lerp(startScale, endScale, delta);

		if (delta >= 1.0f) {
			disappearDone = true;
			Destroy(gameObject);
		}
	}

}
