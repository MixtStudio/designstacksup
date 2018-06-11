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

	public static Vector2 GetPivotToCenterOffset(RectTransform transform) {
		Vector2 newPosition = new Vector2();
		newPosition.x = .5f - transform.pivot.x;
		newPosition.y = .5f - transform.pivot.y;
		return newPosition;
	}

	public static Quaternion GetRandomYRotation() {
		float value = Random.Range(0, 360);
		return Quaternion.Euler(0, value, 0);
	}

	public static Vector3 GetRandomScale(float min, float max) {
		float value = Random.Range(min, max);
		return new Vector3(value,value,value);
	}

}
