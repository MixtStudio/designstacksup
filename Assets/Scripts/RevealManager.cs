﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealManager : MonoBehaviour {

	public int revealNumThreshold = 3;
	public Color targetColour;

	private int revealNum = 0;
	private GameObject[] hiddenObjs;
	private Color startColour;
	private GameObject floor;

	public IEnumerator ScanHidden () {
		yield return new WaitForSeconds(0.1f);
		hiddenObjs = GameObject.FindGameObjectsWithTag("Hidden");
		//foreach (GameObject obj in hiddenObjs)
		//	obj.SetActive(false);
		Debug.Log(hiddenObjs.Length + " Hidden objects detected");
	}
	
	
	public GameObject[] GetHiddenObjects() { return hiddenObjs; }

	public void IncrementRevealNum() {
		revealNum++;
		ColourChange();
		ThresholdCheck();
	}	

	private void ThresholdCheck() {
		if(revealNum >= revealNumThreshold) {
			Debug.Log("Revealing Everything");
			foreach (GameObject obj in hiddenObjs)
				obj.GetComponent<MeshRenderer>().enabled = true;
		}
	}

	private void ColourChange() {
		//Debug.Log("Colour Change");
		if (floor == null)
			floor = GameObject.FindGameObjectWithTag("Floor");

		if (startColour == null)
			startColour = floor.GetComponent<MeshRenderer>().material.color;

		Debug.Log("Colour Before: " + floor.GetComponent<MeshRenderer>().material.color);
		floor.GetComponent<MeshRenderer>().material.color = Color.Lerp(startColour, targetColour, ((float)revealNum / (float)revealNumThreshold) );
		Debug.Log("Colour After: " + floor.GetComponent<MeshRenderer>().material.color);
	}
}
