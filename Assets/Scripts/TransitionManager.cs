using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class TransitionManager : MonoBehaviour {

	private GameObject barsHolder;
	private bool fallCheck = false;
	private List<Transform> columns;
	private Pedastal pedastal;
	private SliderHandDrag sliderHandDrag;
	private LoadManager loadManager;
	private RevealManager revealManager;
	private AudioManager audioManager;

	public void SetBarsHolder(GameObject obj) { barsHolder = obj; }
	public GameObject GetBarsHolder() { return barsHolder; }

	// Use this for initialization
	void Start () {
		pedastal = FindObjectOfType<Pedastal>();
		sliderHandDrag = FindObjectOfType<SliderHandDrag>();
		loadManager = FindObjectOfType<LoadManager>();
		revealManager = FindObjectOfType<RevealManager>();
		audioManager = FindObjectOfType<AudioManager>();
	}

	public void RaisePedastal() {
		Debug.Log("Raise the Pedastal");
		pedastal.BeginRaising();
		audioManager.NowPlay(AudioManager.Audio.Warping02);
	}

	public void BeginFalling() {
		if (fallCheck) return;

		Debug.Log("Begin Falling");

		audioManager.NowPlay(AudioManager.Audio.TrapSection, true, false);

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
