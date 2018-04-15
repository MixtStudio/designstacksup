using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChanger : MonoBehaviour {
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.I)) {
			IncrementScale();
		}
		if (Input.GetKeyDown(KeyCode.K)) {
			DecrementScale();
		}
	}

	public void IncrementScale() {
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + 1, transform.localScale.z);
	}

	public void DecrementScale() {
		if(transform.localScale.y > 1)
			transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - 1, transform.localScale.z);
	}
}
