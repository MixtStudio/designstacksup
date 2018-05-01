using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public AudioSource HubStatistics;

	// Use this for initialization
	void Start () {
		HubStatistics.loop = true;
		HubStatistics.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
