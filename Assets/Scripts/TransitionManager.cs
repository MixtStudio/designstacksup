using System.Collections;
using System.Collections.Generic;
//using System.Collections;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using DG.Tweening;

public class TransitionManager : Mixt.Singleton<TransitionManager> {

	private GameObject barsHolder;
	private bool fallCheck = false;
	private SortedList<string, Transform> columns2;
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

		AudioManager.Instance.NowPlay(AudioManager.Audio.ForestAmbience, true, false);
		sliderHandDrag.BeginFalling();
		columns2 = new SortedList<string, Transform>();
		columns = new List<Transform>();
		int i = 0;

		foreach (Transform trans in barsHolder.transform) {
			columns2.Add(string.Format("col{0}",i), trans);
			i++;
			//columns.Add(trans);
		}
				

		fallCheck = true;
		StartCoroutine(FallingColumns());
		LoadManager.Instance.AdditiveLoadByName("Scene2additive");
		StartCoroutine(RevealManager.Instance.ScanHidden());
	}

	//private IEnumerator FallingColumns() {
	//	Transform column = columns[Random.Range(0,columns.Count-1)];
	//	ColumnFall(column);
	//	columns.Remove(column);		

	//	yield return new WaitForSeconds(0.1f);
	//	if (columns.Count != 0)
	//		StartCoroutine(FallingColumns());
	//}

	//private void ColumnFall(Transform trans) {
	//	foreach (CubeCollision cc in trans.GetComponentsInChildren<CubeCollision>()) {
	//		cc.Unfreeze();
	//		cc.enabled = false;
	//		BoxCollider bc = cc.GetComponent<BoxCollider>();
	//		Destroy(bc);
	//		Destroy(cc);
	//	}
	//}

	private IEnumerator FallingColumns() {

		var wait = new WaitForSeconds(.1f);

		List<Category> categories = SpawnObjectsController.CategoryList;
		for (int i= categories.Count-1; i >= 0; i--) {
			Transform column = categories[i].CategoryContainer.transform;
			//Transform column = columns[i];
			column.DOScaleY(0, 1.5f);
			column.DOMoveY(-.001f, 1.5f);
			yield return wait;
		}

		wait = new WaitForSeconds(2.5f);
		yield return wait;
		Destroy(barsHolder);
		Destroy(GameObject.Find("TextContainer"));
		yield break;
	}
}
