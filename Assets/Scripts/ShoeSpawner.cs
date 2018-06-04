using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoeSpawner : MonoBehaviour {

	public static Vector3 RandomPointInBox(Vector3 center, Vector3 size) {
		return center + new Vector3(
			(UnityEngine.Random.value - 0.5f) * size.x,
			(UnityEngine.Random.value - 0.5f) * size.y,
			(UnityEngine.Random.value - 0.5f) * size.z
			);
	}
}
