using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ascend : MonoBehaviour {

	public float ascendSpeed = 1.0f;
	public GameObject CloudEffect;

	private int cloudTrigger = 2;
	private int ascendCount;

	private float delta = 0.0f;
	private Vector3 startPos;
	private Vector3 endPos;
	private bool ascending = false;
	private DesignDial designDial;


	private DynamicTextController AB_FACT;

	private Vector3 offset = new Vector3(.2f,1f,1f);


	void OnEnable() {
		CloudEffect.SetActive(false);
		designDial = FindObjectOfType<DesignDial>();
		GameObject.FindGameObjectWithTag("MainCamera").transform.SetParent(transform, true);
	}
	
	public void StartAscending() {
		ascendCount++;
		float height = 0.0f;
		Prompts.PromptName prompt;
		
		switch (ascendCount){
			case 1:
				height = 10.0f;
				prompt = Prompts.PromptName.AB_FACT_1;
				break;
				
			case 2:
				height = 50.0f;
				prompt = Prompts.PromptName.AB_FACT_2;
				break;
				
			case 3: 
				height = 100.0f;
				prompt = Prompts.PromptName.AB_FACT_3;
				break;
				
			case 4:
				prompt = Prompts.PromptName.AB_FACT_1;
				height = 1000.0f;
				break;
				
			case 5:
				prompt = Prompts.PromptName.AB_FACT_1;
				Transition();
				return;
				
			default:
				prompt = Prompts.PromptName.AB_FACT_1;
				return;
		}

		AB_FACT = Prompts.GetPrompt(new Vector3(transform.position.x+offset.x, transform.position.y+ offset.y, transform.position.z+offset.z),prompt,transform);
		
		AudioManager.Instance.NowPlay(AudioManager.Audio.AscendingTone, true, true);
		startPos = transform.position;
		endPos = startPos + (Vector3.up * height);
		ascending = true;
		delta = 0.0f;
		
		designDial.Despawn();
		designDial.gameObject.SetActive(false);
		
		if (ascendCount == cloudTrigger) {
			AudioManager.Instance.NowPlay(AudioManager.Audio.Wind);
			AudioManager.Instance.NowStop(AudioManager.Audio.Birds);
			AudioManager.Instance.NowStop(AudioManager.Audio.ForestAmbience);
			CloudEffect.SetActive(true);
		}
	}

	void Update () {
		if (ascending)
			Ascending();
	}

	private void Ascending() {
		delta += Time.deltaTime * ascendSpeed;
		transform.position = Vector3.Lerp(startPos, endPos, delta);

		if (delta >= 1.0f) {
			ascending = false;
			AudioManager.Instance.NowStop(AudioManager.Audio.AscendingTone);
			designDial.gameObject.SetActive(true);
			designDial.Respawn();
		}
	}
	
	private void Transition() {
		AudioManager.Instance.StopAll();
		FindObjectOfType<LoadManager>().LoadByName("NZReveal");
	}


}
