using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;

public class MediaPlayerManager : MonoBehaviour {

	private MediaPlayer mediaPlayer;

	// Use this for initialization
	void Start () {
		mediaPlayer = GetComponent<MediaPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Begin() {
		mediaPlayer.Play();
	}
}
