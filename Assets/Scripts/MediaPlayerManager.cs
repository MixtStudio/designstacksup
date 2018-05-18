using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;

public class MediaPlayerManager : Mixt.Singleton<MediaPlayerManager> {

	private MediaPlayer mediaPlayer;
	
	protected override void Init() {}
	
	void Start () {
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
				LoadManager.Instance.AdditiveLoadByName("Scene1additive");
				gameObject.SetActive(false);
				break;
		}
	}
}
