using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAudio : MonoBehaviour {

	public float fadeSpeed = 0.5f;
	
	private float delta = 0.0f;
	private AudioSource audioSource;
	
	void Start () {
		audioSource = GetComponent<AudioSource>();
	}
	
	void Update () {
		delta += Time.deltaTime * fadeSpeed;
		audioSource.volume = Mathf.Lerp(1.0f, 0.0f, delta);
	}
}
