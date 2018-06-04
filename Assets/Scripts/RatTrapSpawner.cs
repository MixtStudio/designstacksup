using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatTrapSpawner : Mixt.Singleton<RatTrapSpawner> {

	public static int interactiveRatTrapsCount;
	public float revealRadius = 3.0f;
	[HideInInspector]
	public DynamicTextController GN_FACT;
	private GameObject[] hiddenObjs;

	protected override void Init() {
	}

	public IEnumerator SpawnMultiple(int numberToSpawn, Vector3 spawnPoint, Quaternion spawnRotation) {

		float upForce = 8;
		float sideForce = 5f;

		// numberToSpawn = ObjectPooler.Instance.poolDictionary["AB"].Count;
		var wait = new WaitForSeconds(.05f);

		for (int i = 0; i < numberToSpawn; i++) {
			GameObject copyObj = ObjectPooler.Instance.SpawnFromPool("GN", false, spawnPoint, spawnRotation);

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

	public void RevealArea() {
		Debug.Log("Revealing Area");

		if (hiddenObjs == null) {
			hiddenObjs = RevealManager.Instance.GetHiddenObjects();
		}

		RevealManager.Instance.IncrementRevealNum(transform.position, revealRadius);
		if (interactiveRatTrapsCount < RevealManager.Instance.revealNumThreshold) {
			SpawnInteractiveRatTrap("GN", transform.position, transform.rotation );
		} 
	}

	public void SpawnInteractiveRatTrap(string tag, Vector3 spawnPoint,Quaternion spawnRotation) {
		interactiveRatTrapsCount++;
		GameObject ratTrapCopy = ObjectPooler.Instance.SpawnFromPool(tag, true, spawnPoint, spawnRotation);
		ratTrapCopy.GetComponent<RatTrap>().interactiveTrap = true;
		Rigidbody rg = ratTrapCopy.GetComponent<Rigidbody>();
		rg.isKinematic = true;
		rg.velocity = Vector3.zero;
		rg.angularVelocity = Vector3.zero;
	}
}

