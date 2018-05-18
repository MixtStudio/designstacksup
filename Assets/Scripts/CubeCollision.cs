using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollision : MonoBehaviour {

	private Rigidbody rb;
	private AudioManager audioManager;

	void Awake() {
		rb = GetComponent<Rigidbody>();
		rb.AddForce(-Vector3.up * 9f, ForceMode.Impulse);
		StartCoroutine(TurnOnCollider());
	}

	IEnumerator TurnOnCollider()
    {
        yield return new WaitForSeconds(.01f);
        GetComponent<Collider>().enabled = true;
    }

	private void OnCollisionEnter(Collision collision) {
		AudioManager.Instance.NowPlay(AudioManager.Audio.BlockLanding);
		StartCoroutine(Freeze());
	}

	IEnumerator Freeze() {
		yield return new WaitForSeconds(6f);
		rb.constraints = RigidbodyConstraints.FreezeAll;
		rb.isKinematic = true;
		rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
	}

	public void Unfreeze() {
		rb.constraints = RigidbodyConstraints.None;
		rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ
						  | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
		rb.isKinematic = false;
	}
}
