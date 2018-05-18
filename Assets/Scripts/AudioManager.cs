using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Mixt.Singleton<AudioManager> {

	public AudioSource[] audioSources;
	
	public AudioSource[] blockLanding;
	
	public AudioSource[] statStep;
	
	public AudioSource[] trapRattle;
	
	public AudioSource[] trapImpact;	

	protected override void Init() {}	
	
	void Start () {
		NowPlay(Audio.HubAmbience, true);
	}
	
	public bool IsAudioPlaying(Audio audio){return Audio_To_AudioSource(audio).isPlaying;}

	public void NowPlay(Audio audio, bool toLoop = false, bool toAdd = true) {
		AudioSource audioToPlay = Audio_To_AudioSource(audio);
		if (!toAdd)
			StopAll();

		audioToPlay.loop = toLoop;
		audioToPlay.Play();
	}

	public void NowPlay(int audioIndex, bool toLoop = false, bool toAdd = true) {
		if (!toAdd)
			StopAll();

		audioSources[audioIndex].loop = toLoop;
		audioSources[audioIndex].Play();
	}
	
	public void NowStop(Audio audio) {
		AudioSource audioToPlay = Audio_To_AudioSource(audio);
		
		if (audioToPlay.isPlaying)
			audioToPlay.Stop();
	}

	public void NowStop(int audioIndex) {
		if (audioSources[audioIndex].isPlaying)
				audioSources[audioIndex].Stop();
	}

	private AudioSource Audio_To_AudioSource(Audio audio) {
		switch(audio)
		{
			case Audio.HubAmbience:
				return audioSources[0];
				
			case Audio.FallingImpact:
				return audioSources[1];

			case Audio.StatSlider:
				return audioSources[2];

			case Audio.TextPopUp:
				return audioSources[3];

			case Audio.RisingPlatform:
				return audioSources[4];
		
			case Audio.UserControl:
				return audioSources[5];
		
			case Audio.UserControlLong:
				return audioSources[6];
		
			case Audio.ForestAmbience:
				return audioSources[7];
		
			case Audio.TreesGrowing:
				return audioSources[8];
				
			case Audio.AscendingTone:
				return audioSources[9];
				
			case Audio.ShoeTheme:
				return audioSources[10];
				
			case Audio.Wind:
				return audioSources[11];
				
			case Audio.BlockLanding:
				return RandomPick(blockLanding);
			
			case Audio.TrapRattle:
				return RandomPick(trapRattle);
			
			case Audio.TrapImpact:
				return RandomPick(trapImpact);
				
			case Audio.StatStep1:
				return statStep[0];
			
			case Audio.StatStep2:
				return statStep[1];
				
			case Audio.StatStep3:
				return statStep[2];
				
			case Audio.StatStep4:
				return statStep[3];
				
			case Audio.StatStep5:
				return statStep[4];
				
			case Audio.StatStepFinal:
				return statStep[5];			
			
			default:
				return new AudioSource();
		}		
	}
	
	private AudioSource RandomPick(AudioSource[] audioSources){
		int randInt = Random.Range(0, audioSources.Length);
		//Debug.Log("audioSources.Length: "+audioSources.Length);
		//Debug.Log("RandomPick: "+randInt);
		return audioSources[ randInt ];
	}

	private void StopAll() {
		foreach(AudioSource source in audioSources) {
			if (source.isPlaying)
				source.Stop();
		}
	}

	public enum Audio {
		HubAmbience,
		FallingImpact,
		StatSlider,
		TextPopUp,
		RisingPlatform,
		UserControl,
		UserControlLong,
		ForestAmbience,
		TreesGrowing,
		AscendingTone,
		ShoeTheme,
		Wind,
		
		BlockLanding,
		TrapRattle,
		TrapImpact,
		
		StatStep1,
		StatStep2,
		StatStep3,
		StatStep4,
		StatStep5,
		StatStepFinal,
		
	}
}
