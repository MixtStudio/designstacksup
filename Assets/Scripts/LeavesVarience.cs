using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavesVarience : MonoBehaviour {

	// Use this for initialization
	void OnEnable() {
		MeshRenderer mr = GetComponent<MeshRenderer>();
		Material newMat = new Material(mr.material);
		Color colour = mr.material.color;
		colour = new Color(colour.r + Random.Range(0.0f, 0.5f),
						   colour.g + Random.Range(-0.3f, 0.0f),
						   colour.b + Random.Range(0.0f, 0.5f));
		newMat.color = colour;
		mr.material = newMat;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
