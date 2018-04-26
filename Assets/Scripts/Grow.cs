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

		//random varience in size
		//endScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		endScale = new Vector3(transform.localScale.x + Random.Range(-0.2f * transform.localScale.x, 0.2f * transform.localScale.x),
							   transform.localScale.y + Random.Range(-0.2f * transform.localScale.y, 0.2f * transform.localScale.y), 
							   transform.localScale.z + Random.Range(-0.2f * transform.localScale.z, 0.2f * transform.localScale.z) );

		//random varience in colour
		foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>()) {
			Material newMat = new Material(mr.material);
			Color colour = mr.material.color;
			colour = new Color(colour.r + Random.Range(0.0f, 0.5f),
							   colour.g + Random.Range(-0.3f, 0.0f),
						       colour.b + Random.Range(0.0f, 0.5f));
			newMat.color = colour;
			mr.material = newMat;
		}

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
