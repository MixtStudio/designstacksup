using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class TransitionManagerScaling : MonoBehaviour {

	private GameObject barsHolder;
	private bool fallCheck = false;
	private List<Transform> columns;
	private Pedastal pedastal;
	private SliderHandDrag sliderHandDrag;
	private LoadManager loadManager;
	private RevealManager revealManager;

	public void SetBarsHolder(GameObject obj) { barsHolder = obj; }
	public GameObject GetBarsHolder() { return barsHolder; }

	// Use this for initialization
	void Start () {
		pedastal = FindObjectOfType<Pedastal>();
		sliderHandDrag = FindObjectOfType<SliderHandDrag>();
		loadManager = FindObjectOfType<LoadManager>();
		revealManager = FindObjectOfType<RevealManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.K))
			BeginDisappear();
	}

	public void RaisePedastal() {
		Debug.Log("Raise the Pedastal");
		pedastal.BeginRaising();
	}

	public void BeginDisappear() {
		if (fallCheck) return;

		Debug.Log("Begin Disappear");
		
		sliderHandDrag.BeginFalling();

		columns = new List<Transform>();

		foreach(Transform trans in barsHolder.transform)
			columns.Add(trans);		

		fallCheck = true;
		StartCoroutine(DisappearingColumns());
		loadManager.AdditiveLoadByName("Scene2additive");
		StartCoroutine(revealManager.ScanHidden());
	}

	private IEnumerator DisappearingColumns() {
		Transform column = columns[Random.Range(0,columns.Count-1)];
		ColumnDisappear(column);
		columns.Remove(column);		

		yield return new WaitForSeconds(0.1f);
		if (columns.Count != 0)
			StartCoroutine(DisappearingColumns());
	}

	private void ColumnDisappear(Transform trans) {
		Disappear dis = trans.gameObject.AddComponent<Disappear>();
		dis.BeginDisappearing();
	}
}
