using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;

public class MediaPlayerManager : MonoBehaviour {

	private MediaPlayer mediaPlayer;
	private StartScene startScene;
	
	void Start () {
		startScene = FindObjectOfType<StartScene>();
		mediaPlayer = GetComponent<MediaPlayer>();
		mediaPlayer.Events.AddListener(OnVideoEvent);
	}

	public void Begin() {
		mediaPlayer.Play();
	}

	public void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode) {
		Debug.Log("Event:   " + et.ToString());
		switch (et) {
			case MediaPlayerEvent.EventType.FinishedPlaying:
				startScene.LoadScene1();
				gameObject.SetActive(false);
				break;
		}
	}
}
