using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour {

	public GameObject floor;
	public GameObject platform;

	private GameObject barsHolder;
	private bool fallCheck = false;
	private List<Transform> columns;
	private static Random rnd;

	public void SetBarsHolder(GameObject obj) { barsHolder = obj; }
	public GameObject GetBarsHolder() { return barsHolder; }

	// Use this for initialization
	void Start () {
		//barsHolder = GameObject.FindObjectOfType<SpawnFallingBlocks>().GetBarsHolder();
		rnd = new Random();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.K))
			BeginFalling();
	}

	public void BeginFalling() {
		if (fallCheck) return;

		Debug.Log("Begin Falling");

		if(floor!=null)
			floor.GetComponent<MeshCollider>().enabled = false;

		if (platform!=null)
			platform.GetComponent<MeshCollider>().enabled = false;

		columns = new List<Transform>();

		foreach(Transform trans in barsHolder.transform) {
			//ColumnFall(trans);
			columns.Add(trans);
		}

		fallCheck = true;
		StartCoroutine(FallingColumns());
	}

	private IEnumerator FallingColumns() {
		Transform column = columns[Random.Range(0,columns.Count-1)];
		ColumnFall(column);
		columns.Remove(column);		

		yield return new WaitForSeconds(0.5f);
		if (columns.Count != 0)
			StartCoroutine(FallingColumns());
	}

	private void ColumnFall(Transform trans) {
		foreach (CubeCollision cc in trans.GetComponentsInChildren<CubeCollision>()) {
			cc.Unfreeze();
			cc.enabled = false;
			Destroy(cc);
		}
	}
}
