using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour {

	void OnEnable() {
		foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>()) {
			mr.enabled = false;
		}
	}

}
