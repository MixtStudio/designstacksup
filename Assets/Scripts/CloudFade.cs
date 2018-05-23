using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudFade : MonoBehaviour {

	public float fadeStartTime = 0.5f;
	private float timeCheck;
	private bool trigger = false;
	
	void Start () {
		timeCheck = Time.time;
	}	
	
	void Update () {
		if( ((Time.time - timeCheck) >= fadeStartTime) && !trigger ){
			TurnOffParticles();
			trigger = true;
		}
	}
	
	private void TurnOffParticles(){
		foreach(ParticleSystem pSystem in GetComponentsInChildren<ParticleSystem>()){
			pSystem.Stop();
		}
	}
}
