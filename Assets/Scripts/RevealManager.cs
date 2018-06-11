using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class RevealManager : Mixt.Singleton<RevealManager> {

	public int revealNumThreshold = 3;
	public Color[] FloorColourArray;
	public Color[] BackgroundColourArray;
	public GameObject forestFloor;
	public float revealRadius = 3.0f;

	private RatTrapSpawner ratTrapSpawner;
	private GameObject[] hiddenObjs;
	List<GameObject> treeList;
	private GameObject floor;
	private float colourChangeSpeed = 1.0f; 
	private float delta;
	private bool colourTransition = false;
	private Color startFloorColour;
	private Color startBackgroundColour;
	private int treesCountCurrent = 0;
	private int treesCountTotal;
	private bool loaded = false;	

	protected override void Init() {}
	
	private void Start() {
		floor = GameObject.FindGameObjectWithTag("Floor");
		ratTrapSpawner = RatTrapSpawner.Instance;
	}

	public IEnumerator ScanHidden () {
		yield return new WaitForSeconds(0.1f);
		hiddenObjs = GameObject.FindGameObjectsWithTag("Tree");
		treeList = new List<GameObject>(hiddenObjs);
		treesCountTotal = hiddenObjs.Length;
		Debug.Log(treesCountTotal + " Trees detected");
	}	
	
	public GameObject[] GetHiddenObjects() { return hiddenObjs; }

	private void Update() {
		if (colourTransition) {
			ColourTransition();
		}		
	}

	public void IncrementRevealNum(Vector3 revealPosition) {
		ColourChange();
		ThresholdCheck(revealPosition);
	}	

	private void ThresholdCheck(Vector3 revealPosition) {
		if (RatTrapSpawner.interactiveRatTrapsCount == 1) {
			Debug.Log("Reveal Closest 24");
			for (int i = 0; i < 24; i++) {
				GameObject closest = FindClosest(treeList, revealPosition);
				closest.GetComponent<Grow>().BeginGrowing(i);
				if (!AudioManager.Instance.IsAudioPlaying(AudioManager.Audio.TreesGrowing))
					AudioManager.Instance.NowPlay(AudioManager.Audio.TreesGrowing);
				treeList.Remove(closest);
				treesCountCurrent++;
			}
		}
		
		else if(RatTrapSpawner.interactiveRatTrapsCount >= revealNumThreshold) {
			Debug.Log("Revealing Everything");
			floor.SetActive(false);
			Instantiate(forestFloor);
			int i = 0;
			while(treeList.Count != 0) { 
				GameObject closest = FindClosest(treeList, revealPosition);
				closest.GetComponent<Grow>().BeginGrowing(i);
				if (!AudioManager.Instance.IsAudioPlaying(AudioManager.Audio.TreesGrowing))
					AudioManager.Instance.NowPlay(AudioManager.Audio.TreesGrowing);
				treeList.Remove(closest);
				i++;
				treesCountCurrent++;
			}
		}
		
		else {
			Debug.Log("Reveal Nearby");
			AudioManager.Instance.NowPlay(AudioManager.Audio.Birds);
			for (int i = 0; i < treeList.Count; i++) {
				if (Vector3.Distance(revealPosition, treeList[i].transform.position) <= revealRadius) {
					Debug.Log("Revealing Obj");
					treeList[i].GetComponent<Grow>().BeginGrowing(i);
					if (!AudioManager.Instance.IsAudioPlaying(AudioManager.Audio.TreesGrowing))
						AudioManager.Instance.NowPlay(AudioManager.Audio.TreesGrowing);
					treeList.RemoveAt(i);
					treesCountCurrent++;
				}
			}
		}

		if (treesCountCurrent == treesCountTotal && !loaded) {
			Debug.Log("Begin loading Scene3");
			loaded = true;
			CoroutineUtils.Wait(LoadManager.Instance.AdditiveLoadByName("Scene3additive", 7.0f),
				 () => CoroutineUtils.Wait(new WaitUntil(() => FindObjectOfType<Ascend>() != null), () => {
					ShoeSpawner.SpawnInteractiveShoe();
				})
			);
		}
	}

	private void ColourChange() {
		startFloorColour = floor.GetComponent<MeshRenderer>().material.color;
		startBackgroundColour = Camera.main.backgroundColor;
		colourTransition = true;
		delta = 0.0f;
	}

	private void ColourTransition() {
		delta += Time.deltaTime * colourChangeSpeed;

		floor.GetComponent<MeshRenderer>().material.color = Color.Lerp(startFloorColour, FloorColourArray[RatTrapSpawner.interactiveRatTrapsCount - 1], delta);

		Camera.main.backgroundColor = Color.Lerp(startBackgroundColour, BackgroundColourArray[RatTrapSpawner.interactiveRatTrapsCount - 1], delta);

		if (delta >= 1.0f) {
			colourTransition = false;
		}
	}

	private GameObject FindClosest(List<GameObject> objList, Vector3 revealPosition) {
		float minDist = Mathf.Infinity;
		GameObject minObj =  new GameObject();

		foreach(GameObject obj in objList) {
			float distance = Vector3.Distance(revealPosition, obj.transform.position);
			if (distance < minDist) {
				minDist = distance;
				minObj = obj;
			}
		}

		return minObj;
	}

	public void RevealArea(Transform transform) {

		if (hiddenObjs == null) {
			hiddenObjs = RevealManager.Instance.GetHiddenObjects();
		}

		IncrementRevealNum(transform.position);

		RatTrapSpawner.Instance.Spawn_GN_FACT();

		if (RatTrapSpawner.interactiveRatTrapsCount == 2) {
			StartCoroutine(RatTrapSpawner.Instance.SpawnMultiple(50, transform.position, transform.rotation));
		}

		if (RatTrapSpawner.interactiveRatTrapsCount < RevealManager.Instance.revealNumThreshold) {
			ratTrapSpawner.SpawnInteractiveRatTrap();
		}
	}
}
