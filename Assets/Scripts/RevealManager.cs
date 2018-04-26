using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class RevealManager : MonoBehaviour {

	public int revealNumThreshold = 3;
	public Color[] FloorColourArray;
	public Color[] BackgroundColourArray;

	private int revealNum = 0;
	private GameObject[] hiddenObjs;
	List<GameObject> treeList;
	private GameObject floor;

	private void Start() {
		floor = GameObject.FindGameObjectWithTag("Floor");
	}

	public IEnumerator ScanHidden () {
		yield return new WaitForSeconds(0.1f);
		hiddenObjs = GameObject.FindGameObjectsWithTag("Tree");
		treeList = new List<GameObject>(hiddenObjs);
		Debug.Log(hiddenObjs.Length + " Trees detected");
	}	
	
	public GameObject[] GetHiddenObjects() { return hiddenObjs; }

	public void IncrementRevealNum(Vector3 revealPosition, float revealRadius) {
		revealNum++;
		//ColourLerp();
		ColourChange();
		ThresholdCheck(revealPosition, revealRadius);
	}	

	private void ThresholdCheck(Vector3 revealPosition, float revealRadius) {
		Debug.Log("Reveal Number "+revealNum);
		if (revealNum == 1) {
			Debug.Log("First Reveal");
			for (int i = 0; i < 24; i++) {
				GameObject closest = FindClosest(treeList, revealPosition);
				closest.GetComponent<Grow>().BeginGrowing(i);
				treeList.Remove(closest);
			}
		} else if(revealNum >= revealNumThreshold) {
			Debug.Log("Revealing Everything");
			int i = 0;
			foreach (GameObject obj in treeList) {
				//obj.GetComponent<MeshRenderer>().enabled = true;
				GameObject closest = FindClosest(treeList, revealPosition);
				closest.GetComponent<Grow>().BeginGrowing(i);
				treeList.Remove(closest);
				i++;
			}
		} else {
			Debug.Log("Reveal");
			int i = 0;
			foreach (GameObject obj in treeList) {
				if (Vector3.Distance(revealPosition, obj.transform.position) <= revealRadius) {
					Debug.Log("Revealing Obj");
					//obj.GetComponent<MeshRenderer>().enabled = true;
					obj.GetComponent<Grow>().BeginGrowing(i);
					i++;
				}
			}
		}
	}

	private void ColourChange() {
		floor.GetComponent<MeshRenderer>().material.color = FloorColourArray[revealNum-1];
		Camera.main.backgroundColor = BackgroundColourArray[revealNum - 1];
	}

	/*
	private void ColourLerp() {
		//Debug.Log("Colour Change");
		if (floor == null)
			floor = GameObject.FindGameObjectWithTag("Floor");

		if (startColour == null)
			startColour = floor.GetComponent<MeshRenderer>().material.color;

		//Debug.Log("Colour Before: " + floor.GetComponent<MeshRenderer>().material.color);
		floor.GetComponent<MeshRenderer>().material.color = Color.Lerp(startColour, targetColour, ((float)revealNum / (float)revealNumThreshold) );
		//Debug.Log("Colour After: " + floor.GetComponent<MeshRenderer>().material.color);
	}
	*/
	GameObject FindClosest(List<GameObject> objList, Vector3 revealPosition) {
		float minDist = Mathf.Infinity;
		GameObject minObj =  new GameObject();

		foreach(GameObject obj in objList) {
			float distance = Vector3.Distance(revealPosition, obj.transform.position);
			if (distance < minDist) {
				minDist = distance;
				minObj = obj;
			}
		}

		return minObj;
	}
}
