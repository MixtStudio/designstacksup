using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grow : MonoBehaviour {

	public float scaleSpeed = 1.0f;

	private float delta = 0.0f;
	private Vector3 startScale;
	private Vector3 endScale;
	private bool scaling = false;
	private bool scalingDone = false;

	void OnEnable() {
		//startScale = new Vector3(transform.localScale.x, 0.0f, transform.localScale.z);
		startScale = new Vector3(0.0f, 0.0f, 0.0f);
		endScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

		transform.localScale = startScale;
		
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.K))
			BeginGrowing();

		if (scaling && !scalingDone)
			Scale();
	}

	public void BeginGrowing() {
		scaling = true;
	}

	private void Scale() {
		delta += Time.deltaTime * scaleSpeed;
		transform.localScale = Vector3.Lerp(startScale, endScale, delta);

		if (delta >= 1.0f) {
			scalingDone = true;
		}
	}
}
