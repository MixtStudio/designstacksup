using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformUtils : MonoBehaviour {

	public static Vector3 GetLookAtPosition(float distance) {
		Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * distance;
		return pos;
	}

	public static Quaternion GetLookAtRotation(Transform transform) {
		Vector3 direction = transform.position - Camera.main.transform.position;
		Quaternion rotation = Quaternion.LookRotation(direction.normalized);
		return rotation;
	}
}
