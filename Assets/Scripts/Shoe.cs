using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoe : MonoBehaviour {

	private Ascend ascend;

	private void OnTriggerEnter(Collider col) {
		Debug.Log("Collsion with: "+col);
		if(col.gameObject.tag == "DesignDial") {
			ascend.StartAscending();
		}
	}

	void OnEnable () {
		ascend = FindObjectOfType<Ascend>();
	}
}
