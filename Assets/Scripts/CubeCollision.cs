using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollision : MonoBehaviour {

	Rigidbody rb;

	void Awake() {
		rb = GetComponent<Rigidbody>();
		StartCoroutine(TurnOnCollider());
	}

	IEnumerator TurnOnCollider()
    {
        yield return new WaitForSeconds(.05f);
        GetComponent<Collider>().enabled = true;
        yield break;
    }

	private void OnCollisionEnter(Collision collision) {
		StartCoroutine(Freeze());
	}

	IEnumerator Freeze() {
		yield return new WaitForSeconds(4.5f);
		rb.constraints = RigidbodyConstraints.FreezeAll;
		rb.isKinematic = true;
		rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		yield break;
	}

	public void Unfreeze() {
		rb.constraints = RigidbodyConstraints.None;
		rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ
						  | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
		rb.isKinematic = false;
	}
}
