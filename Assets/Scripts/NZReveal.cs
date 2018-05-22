using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NZReveal : MonoBehaviour {

	public float delayTime = 3.0f;
	private float timeCheck;
	private bool trigger = false;
	
	void Start () {
		timeCheck = Time.time;
	}	
	
	void Update () {
		if( ((Time.time - timeCheck) >= delayTime) && !trigger ){
			MediaPlayerManager.Instance.Begin();
			AudioManager.Instance.NowPlay(AudioManager.Audio.EndText);
			trigger = true;
		}
	}
}
