using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class TransitionManager : MonoBehaviour {

	public GameObject floor;
	public GameObject platform;

	private GameObject barsHolder;
	private bool fallCheck = false;
	private List<Transform> columns;
	private static Random rnd;
	private Pedastal pedastal;
	private SliderHandDrag sliderHandDrag;
	private LoadManager loadManager;
	private RevealManager revealManager;

	public void SetBarsHolder(GameObject obj) { barsHolder = obj; }
	public GameObject GetBarsHolder() { return barsHolder; }

	// Use this for initialization
	void Start () {
		rnd = new Random();
		pedastal = FindObjectOfType<Pedastal>();
		sliderHandDrag = FindObjectOfType<SliderHandDrag>();
		loadManager = FindObjectOfType<LoadManager>();
		revealManager = FindObjectOfType<RevealManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.K))
			BeginFalling();
	}

	public void RaisePedastal() {
		Debug.Log("Raise the Pedastal");
		pedastal.BeginRaising();
	}

	public void BeginFalling() {
		if (fallCheck) return;

		Debug.Log("Begin Falling");
		
		sliderHandDrag.BeginFalling();

		columns = new List<Transform>();

		foreach(Transform trans in barsHolder.transform)
			columns.Add(trans);		

		fallCheck = true;
		StartCoroutine(FallingColumns());
		loadManager.AdditiveLoadByName("Scene2additive");
		StartCoroutine(revealManager.ScanHidden());
	}

	private IEnumerator FallingColumns() {
		Transform column = columns[Random.Range(0,columns.Count-1)];
		ColumnFall(column);
		columns.Remove(column);		

		yield return new WaitForSeconds(0.1f);
		if (columns.Count != 0)
			StartCoroutine(FallingColumns());
	}

	private void ColumnFall(Transform trans) {
		foreach (CubeCollision cc in trans.GetComponentsInChildren<CubeCollision>()) {
			cc.Unfreeze();
			cc.enabled = false;
			BoxCollider bc = cc.GetComponent<BoxCollider>();
			Destroy(bc);
			Destroy(cc);
		}
	}
}
