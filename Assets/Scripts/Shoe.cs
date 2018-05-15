using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoe : MonoBehaviour {

	private Ascend ascend;

	private void OnTriggerEnter(Collider col) {
		Debug.Log("Collsion with: "+col);
		if(col.gameObject.tag == "DesignDial") {
			col.gameObject.GetComponent<DesignDial>().Respawn();
			ascend.StartAscending();
		}
	}

	// Use this for initialization
	void OnEnable () {
		ascend = FindObjectOfType<Ascend>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
