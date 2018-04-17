using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour {

	public GameObject floor;
	public GameObject platform;

	private GameObject barsHolder;
	private bool fallCheck = false;

	public void SetBarsHolder(GameObject obj) { barsHolder = obj; }
	public GameObject GetBarsHolder() { return barsHolder; }

	// Use this for initialization
	void Start () {
		//barsHolder = GameObject.FindObjectOfType<SpawnFallingBlocks>().GetBarsHolder();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.K))
			Fall();
	}

	public void Fall() {
		if (fallCheck) return;

		Debug.Log("Begin Falling");

		if(floor!=null)
			floor.GetComponent<MeshCollider>().enabled = false;

		if (platform!=null)
			platform.GetComponent<MeshCollider>().enabled = false;
	
		//falls in unison
		foreach (CubeCollision cc in barsHolder.GetComponentsInChildren<CubeCollision>(barsHolder)) {
			cc.Unfreeze();
			cc.enabled = false;			
		}
		fallCheck = true;
	}
}
