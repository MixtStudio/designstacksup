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
		endScale = new Vector3(transform.localScale.x + Random.Range(-0.25f * transform.localScale.x, 0.25f * transform.localScale.x),
							   transform.localScale.y + Random.Range(-0.25f * transform.localScale.y, 0.25f * transform.localScale.y), 
							   transform.localScale.z + Random.Range(-0.25f * transform.localScale.z, 0.25f * transform.localScale.z) );

		//varience in scaleSpeed

		//varience in rotation
		transform.rotation = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.up);

		transform.localScale = startScale;
	}

	void Update() {
		//if(Input.GetKeyDown(KeyCode.K))
			//BeginGrowing(0);

		if (scaling && !scalingDone)
			Scale();
	}

	public void BeginGrowing(int indexScale) {
		scaling = true;
		if (indexScale >= 5 && indexScale <= 14)
			scaleSpeed *= 0.5f;
		if (indexScale >= 15)
			scaleSpeed *= 0.25f;

		//varience
		scaleSpeed *= Random.Range(0.75f, 1.25f); 
	}

	private void Scale() {
		delta += Time.deltaTime * scaleSpeed;
		transform.localScale = Vector3.Lerp(startScale, endScale, delta);

		if (delta >= 1.0f) {
			scalingDone = true;
		}
	}
}
