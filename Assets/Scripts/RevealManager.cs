using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealManager : MonoBehaviour {

	public int revealNumThreshold = 3;

	private int revealNum = 0;
	private GameObject[] hiddenObjs;

	// Use this for initialization
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
		ThresholdCheck();
	}	

	private void ThresholdCheck() {
		if(revealNum >= revealNumThreshold) {
			Debug.Log("Revealing Everything");
			foreach (GameObject obj in hiddenObjs)
				obj.GetComponent<MeshRenderer>().enabled = true;
		}
	}
}
