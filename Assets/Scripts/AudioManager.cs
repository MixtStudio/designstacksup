using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public AudioSource[] audioSources;

	void Start () {
		NowPlay(Audio.HubStatistics, true);
	}

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

	private AudioSource Audio_To_AudioSource(Audio audio) {
		if (audio == Audio.HubStatistics)
			return audioSources[0];

		else if (audio == Audio.TrapSection)
			return audioSources[1];

		else if (audio == Audio.Warping01)
			return audioSources[2];

		else if (audio == Audio.Warping02)
			return audioSources[3];

		else if (audio == Audio.Warping03)
			return audioSources[4];

		else {
			return new AudioSource();
		}
	}

	private void StopAll() {
		foreach(AudioSource source in audioSources) {
			if (source.isPlaying)
				source.Stop();
		}
	}

	public enum Audio {
		HubStatistics,
		TrapSection,
		Warping01,
		Warping02,
		Warping03
	}
}
