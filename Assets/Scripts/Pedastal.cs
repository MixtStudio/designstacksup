using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedastal : MonoBehaviour {

	public GameObject spawnPrefab;
	public GameObject SpawnPoint;
	public float riseSpeed = 1.0f;
	public float spawnWaitTime = 0.5f;

	private float delta = 0.0f;
	private Vector3 startScale;
	private Vector3 endScale;
	private bool rising = false;

	private MeshRenderer meshRenderer;

	void Start () {
		startScale = new Vector3(transform.localScale.x, 0.0f, transform.localScale.z);
		endScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

		transform.localScale = startScale;
		meshRenderer = GetComponentsInChildren<MeshRenderer>()[0];
		meshRenderer.enabled = false;
	}
	
	void Update () {
		if (rising)
			Rise();		
	}

	public void BeginRaising() {
		rising = true;
		meshRenderer.enabled = true;
	}

	private void Rise() {
		delta += Time.deltaTime * riseSpeed;
		transform.localScale = Vector3.Lerp(startScale, endScale, delta);

		if (delta >= 1.0f) {
			rising = false;
			StartCoroutine(WaitSpawnRatTrap(spawnWaitTime));
			AudioManager.Instance.NowStop(AudioManager.Audio.RisingPlatform);
		}
	}

	public IEnumerator WaitSpawnRatTrap(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		GameObject spawnedObj = Instantiate(spawnPrefab);
		spawnedObj.transform.position = SpawnPoint.transform.position;
		spawnedObj.GetComponent<RatTrap>().SetSpawnPosition(SpawnPoint.transform.position);

	}


}
