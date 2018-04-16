using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reveal : MonoBehaviour {

	public float revealRadius = 5.0f;

	private GameObject[] hiddenObjs;
	private RevealManager revealManager;

	// Use this for initialization
	void OnEnable() {
		revealManager = GameObject.FindObjectOfType<RevealManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col) {
		if(col.collider.gameObject.tag == "Floor") {
			hiddenObjs = revealManager.GetHiddenObjects();
			RevealArea();
		}
	}

	private void RevealArea() {
		Debug.Log("Revealing Area");
		foreach(GameObject obj in hiddenObjs) {
			if (Vector3.Distance(transform.position, obj.transform.position) <= revealRadius) {
				Debug.Log("Revealing Obj");
				obj.SetActive(true);
			}
		}
		revealManager.IncrementRevealNum();
	}
}
