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

	public IEnumerator AdditiveLoadByName(string scenename, float delaytime) {
		Debug.Log("waiting to load for: " + delaytime);
		yield return new WaitForSeconds(delaytime);
		Debug.Log("sceneName to load: " + scenename);
		SceneManager.LoadScene(scenename, LoadSceneMode.Additive);
	}

	public IEnumerator AdditiveLoadByIndex(int sceneANumber, float delaytime) {
		yield return new WaitForSeconds(delaytime);
		Debug.Log("sceneBuildIndex to load: " + sceneANumber);
		SceneManager.LoadScene(sceneANumber, LoadSceneMode.Additive);
	}
	
	public void LoadByName(string scenename) {
		Debug.Log("sceneName to load: " + scenename);
		SceneManager.LoadScene(scenename, LoadSceneMode.Single);
	}
	
	public void LoadByIndex(int sceneANumber) {
		Debug.Log("sceneBuildIndex to load: " + sceneANumber);
		SceneManager.LoadScene(sceneANumber, LoadSceneMode.Single);
	}
}
