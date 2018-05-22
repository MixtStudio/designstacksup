using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class FadeTest : MonoBehaviour {

	public float delayTime = 6.0f;
	private float timeCheck;
	private bool trigger = false;
	
	void Start () {
		timeCheck = Time.time;
	}	
	
	void Update () {
		if( ((Time.time - timeCheck) >= delayTime) && !trigger ){
			Fade();
			trigger = true;
		}
	}
	
	private Action Temp1;
	private Action Temp2;
	
	private void Fade(){
		FadeManager.Instance.DoFade(2.0f, 2.0f, Temp1, Temp2);
	}
}
