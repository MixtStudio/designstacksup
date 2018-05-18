using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ascend : MonoBehaviour {

	public float ascendSpeed = 1.0f;
	public GameObject CloudEffect;

	private int cloudTrigger = 2;
	private int ascendCount;

	private float delta = 0.0f;
	private Vector3 startPos;
	private Vector3 endPos;
	private bool ascending = false;
	private DesignDial designDial;
	private AudioManager audioManager;

	void OnEnable() {
		CloudEffect.SetActive(false);
		designDial = FindObjectOfType<DesignDial>();
		GameObject.FindGameObjectWithTag("MainCamera").transform.SetParent(transform, true);
		audioManager = FindObjectOfType<AudioManager>();
	}
	
	public void StartAscending() {
		ascendCount++;
		float height;
		
		switch(ascendCount){
			case 1:
				height = 10.0f;
				audioManager.NowPlay(AudioManager.Audio.AscendingTone, true, false);
				break;
				
			case 2:
				height = 50.0f;
				break;
				
			case 3: 
				height = 100.0f;
				break;
				
			case 4:
				height = 1000.0f;
				break;
				
			default:
				return;
		}
		
		audioManager.NowPlay(AudioManager.Audio.RisingPlatform, true, true);
		startPos = transform.position;
		endPos = startPos + (Vector3.up * height);
		ascending = true;
		delta = 0.0f;
		
		designDial.Despawn();
		designDial.gameObject.SetActive(false);
		
		if (ascendCount == cloudTrigger)
			CloudEffect.SetActive(true);
	}

	void Update () {
		if (ascending)
			Ascending();
	}

	private void Ascending() {
		delta += Time.deltaTime * ascendSpeed;
		transform.position = Vector3.Lerp(startPos, endPos, delta);

		if (delta >= 1.0f) {
			ascending = false;
			audioManager.NowStop(AudioManager.Audio.RisingPlatform);
			designDial.gameObject.SetActive(true);
			designDial.Respawn();
		}
	}
}
