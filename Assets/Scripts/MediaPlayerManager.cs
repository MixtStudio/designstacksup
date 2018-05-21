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
		AudioManager.Instance.NowPlay(AudioManager.Audio.IntroText, false, true);
	}

	public void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode) {
		Debug.Log("Event:   " + et.ToString());
		if(et == MediaPlayerEvent.EventType.FinishedPlaying && mediaPlayer.m_AutoStart == false) {
			LoadManager.Instance.AdditiveLoadByName("Scene1additive");
			//AudioManager.Instance.NowPlay(AudioManager.Audio.HubAmbience,true, false);
			AudioManager.Instance.NowStop(AudioManager.Audio.IntroText);
			gameObject.SetActive(false);
		}
		else if(et == MediaPlayerEvent.EventType.FinishedPlaying && mediaPlayer.m_AutoStart == true) {
				//LoadManager.Instance.AdditiveLoadByName("Scene1additive");
				//gameObject.SetActive(false);
		}
	}
}
