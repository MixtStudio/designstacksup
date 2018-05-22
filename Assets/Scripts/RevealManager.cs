using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class RevealManager : Mixt.Singleton<RevealManager> {

	public int revealNumThreshold = 3;
	public Color[] FloorColourArray;
	public Color[] BackgroundColourArray;
	public GameObject forestFloor;

	private int revealNum = 0;
	private GameObject[] hiddenObjs;
	List<GameObject> treeList;
	private GameObject floor;
	private float colourChangeSpeed = 1.0f; // may need to implement a failsafe if the user triggers the reveals too fast (the colour change speed is too fast atm for that to happen)
	private float delta;
	private bool colourTransition = false;
	private Color startFloorColour;
	private Color startBackgroundColour;
	private int revealCount = 0;
	private int revealTotal;
	private bool loaded = false;	

	protected override void Init() {}
	
	private void Start() {
		floor = GameObject.FindGameObjectWithTag("Floor");		
	}

	public IEnumerator ScanHidden () {
		yield return new WaitForSeconds(0.1f);
		hiddenObjs = GameObject.FindGameObjectsWithTag("Tree");
		treeList = new List<GameObject>(hiddenObjs);
		revealTotal = hiddenObjs.Length;
		Debug.Log(revealTotal + " Trees detected");
	}	
	
	public GameObject[] GetHiddenObjects() { return hiddenObjs; }

	private void Update() {
		if (colourTransition)
			ColourTransition();
	}

	public void IncrementRevealNum(Vector3 revealPosition, float revealRadius) {
		revealNum++;
		ColourChange();
		ThresholdCheck(revealPosition, revealRadius);
	}	

	private void ThresholdCheck(Vector3 revealPosition, float revealRadius) {
		Debug.Log("Reveal Number "+revealNum);
		if (revealNum == 1) {
			Debug.Log("Reveal Closest 24");
			for (int i = 0; i < 24; i++) {
				GameObject closest = FindClosest(treeList, revealPosition);
				closest.GetComponent<Grow>().BeginGrowing(i);
				if (!AudioManager.Instance.IsAudioPlaying(AudioManager.Audio.TreesGrowing))
					AudioManager.Instance.NowPlay(AudioManager.Audio.TreesGrowing);
				treeList.Remove(closest);
				revealCount++;
			}
		} else if(revealNum >= revealNumThreshold) {
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
				revealCount++;
			}
		} else {
			Debug.Log("Reveal Nearby");
			AudioManager.Instance.NowPlay(AudioManager.Audio.Birds);
			for (int i = 0; i < treeList.Count; i++) {
				if (Vector3.Distance(revealPosition, treeList[i].transform.position) <= revealRadius) {
					Debug.Log("Revealing Obj");
					treeList[i].GetComponent<Grow>().BeginGrowing(i);
					if (!AudioManager.Instance.IsAudioPlaying(AudioManager.Audio.TreesGrowing))
						AudioManager.Instance.NowPlay(AudioManager.Audio.TreesGrowing);
					treeList.RemoveAt(i);
					revealCount++;
				}
			}
		}
		if (revealCount == revealTotal && !loaded) {
			Debug.Log("Begin loading Scene3");
			StartCoroutine(LoadManager.Instance.AdditiveLoadByName("Scene3additive", 7.0f));
			loaded = true;
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

		floor.GetComponent<MeshRenderer>().material.color = Color.Lerp(startFloorColour, FloorColourArray[revealNum - 1], delta);

		Camera.main.backgroundColor = Color.Lerp(startBackgroundColour, BackgroundColourArray[revealNum - 1], delta);

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
}
