using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;

public class MediaPlayerManager : Mixt.Singleton<MediaPlayerManager> {

	private MediaPlayer mediaPlayer;
	private bool loadScene;
	
	protected override void Init() {}
	
	void Start () {
		mediaPlayer = GetComponent<MediaPlayer>();
		mediaPlayer.Events.AddListener(OnVideoEvent);
	}

	public void Begin(bool load) {
		transform.GetChild(0).gameObject.SetActive(true);
		mediaPlayer.Play();
		loadScene = load;
	}

	public void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode) {
		Debug.Log("Event:   " + et.ToString());
		if(et == MediaPlayerEvent.EventType.FinishedPlaying && loadScene) {
			LoadManager.Instance.AdditiveLoadByName("Scene1additive");
			AudioManager.Instance.NowStop(AudioManager.Audio.IntroText);
			gameObject.SetActive(false);
		}
	}
}
