using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollision : MonoBehaviour {

	private Rigidbody rb;
	private AudioManager audioManager;

	void Awake() {
		rb = GetComponent<Rigidbody>();
		rb.AddForce(-Vector3.up * 9f, ForceMode.Impulse);
		audioManager = FindObjectOfType<AudioManager>();
		StartCoroutine(TurnOnCollider());
	}

	IEnumerator TurnOnCollider()
    {
        yield return new WaitForSeconds(.01f);
        GetComponent<Collider>().enabled = true;
        //yield break;
    }

	private void OnCollisionEnter(Collision collision) {
		//float randNum = Random.Range(0.0f, 1.0f);
		//if(randNum <= 0.75f)
		audioManager.NowPlay(AudioManager.Audio.BlockLanding);
		StartCoroutine(Freeze());
	}

	IEnumerator Freeze() {
		yield return new WaitForSeconds(6f);
		rb.constraints = RigidbodyConstraints.FreezeAll;
		rb.isKinematic = true;
		rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		//yield break;
	}

	public void Unfreeze() {
		rb.constraints = RigidbodyConstraints.None;
		rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ
						  | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
		rb.isKinematic = false;
	}
}
