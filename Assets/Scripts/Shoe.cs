using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoe : MonoBehaviour {

	private Ascend ascend;

	private DynamicTextController AB_INTRO;
	private DynamicTextController AB_DIAL_UP;

	private Vector3 offset = new Vector3(.4f, .2f, 0);

	private void OnTriggerEnter(Collider col) {
		Debug.Log("Collsion with: "+col);
		if(col.gameObject.tag == "DesignDial") {
			AudioManager.Instance.NowPlay(AudioManager.Audio.StatStepFinal);
			ascend.StartAscending();
		}
	}

	void OnEnable () {

		AB_INTRO = Prompts.GetPrompt(new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z), Prompts.PromptName.AB_INTRO);
		AB_INTRO.transform.localScale *= .15f;

		AB_DIAL_UP = Prompts.GetPrompt(new Vector3(transform.position.x + offset.x + .05f, transform.position.y + offset.y, transform.position.z), Prompts.PromptName.AB_DIAL_UP_DESIGN);
		AB_DIAL_UP.transform.localScale *= .15f;

		ascend = FindObjectOfType<Ascend>();
		AudioManager.Instance.NowPlay(AudioManager.Audio.ShoeSpawn);
	}
}
