using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NZReveal : MonoBehaviour {

	public float delayTime = 3.0f;
	private float timeCheck;
	
	void Start () {
		timeCheck = Time.time;
	}
	
	
	void Update () {
		if((Time.time - timeCheck) >= delayTime)
			MediaPlayerManager.Instance.Begin();
	}
}
