using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class NZReveal : MonoBehaviour {

	public float delayTime = 7.0f;
	private float timeCheck;
	private bool trigger = false;
	
	private Action Temp1;
	private Action Temp2;
	
	void Start () {
		timeCheck = Time.time;
		FadeManager.Instance.DoFade(0.0f, 5.0f, Temp1, Temp2);
	}	
	
	void Update () {
		if( ((Time.time - timeCheck) >= delayTime) && !trigger ){
			MediaPlayerManager.Instance.Begin();
			AudioManager.Instance.NowPlay(AudioManager.Audio.EndText);
			trigger = true;
		}
	}
}
