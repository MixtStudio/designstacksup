using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour {

	public void AdditiveLoadByName(string scenename) {
		Debug.Log("sceneName to load: " + scenename);
		SceneManager.LoadScene(scenename, LoadSceneMode.Additive);
	}

	public void AdditiveLoadByIndex(int sceneANumber) {
		Debug.Log("sceneBuildIndex to load: " + sceneANumber);
		SceneManager.LoadScene(sceneANumber, LoadSceneMode.Additive);
	}
}
