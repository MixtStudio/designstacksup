using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour {

	void OnEnable() {
		GetComponent<MeshRenderer>().enabled = false;
	}

}
