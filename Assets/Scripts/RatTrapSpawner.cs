using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatTrapSpawner : Mixt.Singleton<RatTrapSpawner> {

	[HideInInspector]
	public static Transform RatTrapSpawnPoint;

	public static int interactiveRatTrapsCount=0;	

	[HideInInspector]
	public DynamicTextController GN_FACT;
	//private GameObject[] hiddenObjs;


	protected override void Init() {
		RatTrapSpawnPoint = this.transform;
		RatTrapSpawnPoint.position =new Vector3(0.5f, 1.2f, 0.5f);
		RatTrapSpawnPoint.rotation = Quaternion.identity;
	}

	public IEnumerator SpawnMultiple(int numberToSpawn, Vector3 spawnPoint, Quaternion spawnRotation) {
		Debug.Log("I am being called");
		float upForce = 8;
		float sideForce = 5f;

		// numberToSpawn = ObjectPooler.Instance.poolDictionary["AB"].Count;
		var wait = new WaitForSeconds(.05f);

		for (int i = 0; i < numberToSpawn; i++) {
			GameObject copyObj = ObjectPooler.Instance.SpawnFromPool("GN", true, false, spawnPoint, spawnRotation);

			float xForce = Random.Range(-sideForce, sideForce);
			float yForce = Random.Range(upForce, upForce);
			float zForce = Random.Range(-sideForce, sideForce);

			Vector3 force = new Vector3(xForce, yForce, zForce);
			copyObj.GetComponent<Rigidbody>().velocity = force;
			yield return wait;
		}

		yield break;
	}


	public void Spawn_GN_FACT() {
		Vector3 GN_FACT_POSITION = TransformUtils.GetLookAtPosition(2);
		GN_FACT_POSITION = new Vector3(GN_FACT_POSITION.x, GN_FACT_POSITION.y + 1.5f, GN_FACT_POSITION.z);

		Prompts.PromptName prompt;
		switch (interactiveRatTrapsCount) {
			case 1:
				prompt = Prompts.PromptName.GN_FACT_1;
				break;

			case 2:
				prompt = Prompts.PromptName.GN_FACT_2;
				break;

			case 3:
				prompt = Prompts.PromptName.GN_FACT_3;
				break;

			default:
				prompt = Prompts.PromptName.GN_FACT_1;
				return;
		}

		GN_FACT = Prompts.GetPrompt(GN_FACT, GN_FACT_POSITION, Quaternion.identity, prompt, .5f);
		GN_FACT.transform.rotation = TransformUtils.GetLookAtRotation(GN_FACT.transform);
	}

	public GameObject SpawnInteractiveRatTrap() {
		GameObject ratTrapCopy = ObjectPooler.Instance.SpawnFromPool("GN", false, true, RatTrapSpawnPoint.position, RatTrapSpawnPoint.rotation);
		Rigidbody rg = ratTrapCopy.GetComponent<Rigidbody>();
		//rg.isKinematic = true;
		rg.velocity = Vector3.zero;
		rg.angularVelocity = Vector3.zero;
		return ratTrapCopy;
	}
}

