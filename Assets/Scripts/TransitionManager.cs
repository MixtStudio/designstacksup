using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class TransitionManager : Mixt.Singleton<TransitionManager> {

	private GameObject barsHolder;
	private bool fallCheck = false;
	private List<Transform> columns;
	private Pedastal pedastal;
	private SliderHandDrag sliderHandDrag;

	public void SetBarsHolder(GameObject obj) { barsHolder = obj; }
	public GameObject GetBarsHolder() { return barsHolder; }

	protected override void Init() {}

	public void RaisePedastal() {

		if (sliderHandDrag == null)
			sliderHandDrag = FindObjectOfType<SliderHandDrag>();
		sliderHandDrag.DestroyPrompt();
		
		Debug.Log("Raise the Pedastal");
		if(pedastal == null)
			pedastal = FindObjectOfType<Pedastal>();

		pedastal.BeginRaising();
		AudioManager.Instance.NowPlay(AudioManager.Audio.RisingPlatform);
	}

	public void BeginFalling() {
		if (fallCheck) return;

		Debug.Log("Begin Falling");

		AudioManager.Instance.NowPlay(AudioManager.Audio.ForestAmbience, true, false);

		//if (sliderHandDrag == null)
		//	sliderHandDrag = FindObjectOfType<SliderHandDrag>();
		sliderHandDrag.BeginFalling();

		columns = new List<Transform>();

		foreach(Transform trans in barsHolder.transform)
			columns.Add(trans);		

		fallCheck = true;
		StartCoroutine(FallingColumns());
		LoadManager.Instance.AdditiveLoadByName("Scene2additive");
		StartCoroutine(RevealManager.Instance.ScanHidden());
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
