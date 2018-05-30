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
		transform.position = new Vector3(transform.position.x, 1.17f, transform.position.z);
		transform.rotation = Quaternion.Euler(14.68f, 270, 0);
		AB_INTRO = Prompts.GetPrompt(new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z), Quaternion.identity,Prompts.PromptName.AB_INTRO,.15f);
		AB_INTRO.transform.rotation = TransformUtils.GetLookAtRotation(AB_INTRO.transform);

		ascend = FindObjectOfType<Ascend>();
		AudioManager.Instance.NowPlay(AudioManager.Audio.ShoeSpawn);
	}

	private void Update() {
		//transform.Rotate(Vector3.up, 20 * Time.deltaTime);		
	}
}
