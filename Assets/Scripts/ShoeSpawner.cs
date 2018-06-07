using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoeSpawner : Mixt.Singleton<RatTrapSpawner> {

	public static bool canSpawnFlyingShoes=false;

	[HideInInspector]
	public static Transform ShoeSpawnPoint;

	public static Vector3 RandomPointInBox(Vector3 center, Vector3 size) {
		return center + new Vector3(
			(UnityEngine.Random.value - 0.5f) * size.x,
			(UnityEngine.Random.value - 0.5f) * size.y,
			(UnityEngine.Random.value - 0.5f) * size.z
			);
	}

	protected override void Init() {
		ShoeSpawnPoint = this.transform;
		ShoeSpawnPoint.position= new Vector3(.5f, 1.17f, .5f);
		ShoeSpawnPoint.rotation= Quaternion.Euler(14.68f, 270, 0);
	}

	public static GameObject SpawnInteractiveShoe() {
		GameObject shoeCopy = ObjectPooler.Instance.SpawnFromPool("AB", true, ShoeSpawnPoint.position, ShoeSpawnPoint.rotation);
		Rigidbody rg = shoeCopy.GetComponent<Rigidbody>();
		rg.velocity = Vector3.zero;
		rg.angularVelocity = Vector3.zero;
		return shoeCopy;
	}

	public static void SpawnFlyingShoe() {
		float upForce = 3;
		float sideForce = 2;

		Vector3 spawnPoint = RandomPointInBox(ShoeSpawnPoint.position, Vector3.one*2);
		GameObject shoeCopy= ObjectPooler.Instance.SpawnFromPool("AB", false, spawnPoint, Quaternion.identity);
		float xForce = Random.Range(-sideForce, sideForce);
		float yForce = Random.Range(upForce, upForce);
		float zForce = Random.Range(-sideForce, sideForce);

		Vector3 force = new Vector3(xForce, yForce, zForce);
		Rigidbody rb = shoeCopy.GetComponent<Rigidbody>();
		rb.isKinematic = false;
		rb.velocity = force;
	}
}
