using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shoe : MonoBehaviour, IPooledObjects {

	private Ascend ascend;
	public bool interactiveShoe { get; set; }
	private DynamicTextController AB_INTRO;
	private DynamicTextController AB_DIAL_UP;

	private Vector3 offset = new Vector3(.4f, .2f, 0);

	public void OnObjectSpawn(bool interactive) {
		interactiveShoe = interactive;
		ascend = FindObjectOfType<Ascend>();
		gameObject.transform.parent = ascend.gameObject.transform;
		AssignShoeMaterial(interactiveShoe);


		if (interactiveShoe) {
			AB_INTRO = Prompts.GetPrompt(new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, transform.position.z), Quaternion.identity, Prompts.PromptName.AB_INTRO, .15f);
			AB_INTRO.transform.rotation = TransformUtils.GetLookAtRotation(AB_INTRO.transform);
			
			AudioManager.Instance.NowPlay(AudioManager.Audio.ShoeSpawn);
		} else {
			transform.DOShakeRotation(10, 5, 2, 20, false).SetLoops(-1, LoopType.Incremental);
			//gameObject.transform.localScale
		}
	}

	private void AssignShoeMaterial(bool interactive) {
		if (interactive) {
			gameObject.GetComponent<Renderer>().material = ShoeSpawner.Instance.shoeMaterials[0];
		} 
		
		else {
			System.Random rand = new System.Random();
			gameObject.GetComponent<Renderer>().material = rand.Next(2) == 0 ? ShoeSpawner.Instance.shoeMaterials[1] : ShoeSpawner.Instance.shoeMaterials[2];
		}
		
	}

	private void OnTriggerEnter(Collider col) {

		//Debug.Log("Collsion with: " + col);
		if (interactiveShoe) {
			if (col.gameObject.tag == "DesignDial") {
				AudioManager.Instance.NowPlay(AudioManager.Audio.StatStepFinal);
				ascend.StartAscending();
			}
		}
	
		if (col.gameObject.tag == "ShoeKiller") {
			ShoeSpawner.SpawnFlyingShoe(ascend.designDial.transform.position,1);
			ObjectPooler.Instance.AddToPool("AB", gameObject);
		}
	}

	private void Update() {
		if (interactiveShoe) {
			transform.Rotate(Vector3.up, 20 * Time.deltaTime);
		}
	}
}

	
