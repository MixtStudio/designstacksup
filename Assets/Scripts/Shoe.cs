using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoe : MonoBehaviour, IPooledObjects {

	private Ascend ascend;
	public bool interactiveShoe { get; set; }
	private DynamicTextController AB_INTRO;
	private DynamicTextController AB_DIAL_UP;

	private Vector3 offset = new Vector3(.4f, .2f, 0);

	public void OnObjectSpawn(bool interactive) {
		interactiveShoe = interactive;
		AssignShoeMaterial();
		MeshCollider mc = this.GetComponent<MeshCollider>();
	
		if (interactiveShoe) {
		
			Debug.Log(" I am the ascend" + ascend);
			AB_INTRO = Prompts.GetPrompt(new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z), Quaternion.identity, Prompts.PromptName.AB_INTRO, .15f);
			AB_INTRO.transform.rotation = TransformUtils.GetLookAtRotation(AB_INTRO.transform);
			mc.enabled = true;
			ascend = FindObjectOfType<Ascend>();
			AudioManager.Instance.NowPlay(AudioManager.Audio.ShoeSpawn);
		} 
		
		else {
			mc.enabled = false;
			Destroy(this.GetComponent<Shoe>());
		}
	}

	private void AssignShoeMaterial() {
		//gameObject.GetComponent<Renderer>().material;
	}

	private void OnTriggerEnter(Collider col) {
	
		Debug.Log("Collsion with: " + col);
		if (col.gameObject.tag == "DesignDial") {
			AudioManager.Instance.NowPlay(AudioManager.Audio.StatStepFinal);
			ascend.StartAscending();
		}
	}

	private void Update() {
		transform.Rotate(Vector3.up, 20 * Time.deltaTime);
	}
}

	
