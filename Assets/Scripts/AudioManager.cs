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
			case Audio.IntroText:
				return FindAudio("IntroText");
			
			case Audio.HubAmbience:
				return FindAudio("HubAmbience");
				
			//case Audio.FallingImpact:
			//	return FindAudio("FallingImpact");

			case Audio.StatSlider:
				return FindAudio("StatSlider");

			case Audio.TextPopUp:
				return FindAudio("TextPopUp");

			case Audio.RisingPlatform:
				return FindAudio("RisingPlatform");
		
			case Audio.UserControl:
				return FindAudio("UserControl");
		
			case Audio.UserControlLong:
				return FindAudio("UserControlLong");
		
			case Audio.ForestAmbience:
				return FindAudio("ForestAmbience");
		
			case Audio.TreesGrowing:
				return FindAudio("TreesGrowing");
				
			case Audio.AscendingTone:
				return FindAudio("AscendingTone");
				
			case Audio.ShoeTheme:
				return FindAudio("ShoeTheme");
				
			case Audio.Wind:
				return FindAudio("Wind");
				
			case Audio.AboveNZ:
				return FindAudio("AboveNZ");
				
			case Audio.Birds:
				return FindAudio("Birds");
				
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
	
	private AudioSource FindAudio(string nametoFind){
		foreach(AudioSource audioSource in audioSources) {
			if(audioSource.gameObject.name == nametoFind)
				return audioSource;			
		}
		return new AudioSource();
	}
	
	private AudioSource RandomPick(AudioSource[] audioSources){
		int randInt = Random.Range(0, audioSources.Length);
		//Debug.Log("audioSources.Length: "+audioSources.Length);
		//Debug.Log("RandomPick: "+randInt);
		return audioSources[ randInt ];
	}

	public void StopAll() {
		foreach(AudioSource source in audioSources) {
			if (source.isPlaying)
				source.Stop();
		}
	}

	public enum Audio {
		IntroText,
		HubAmbience,
		//FallingImpact,
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
		AboveNZ,
		Birds,
		
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
