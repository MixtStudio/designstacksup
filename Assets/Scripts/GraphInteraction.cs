using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class GraphInteraction : MonoBehaviour {

	public TMP_Text TextPrefab { get; set; }
	private string industryInfo { get; set; }
	public Category c { get; set; }
	private TMP_Text textComp;
	private GazeFinder gazeFinder;

	private void Start() {
		textComp = Instantiate(TextPrefab, SpawnObjectsController.instance.TextContainer.transform);
		textComp.name = "Text " + c.Name;
		gazeFinder = FindObjectOfType<GazeFinder>();

		//Sets the initial Text Transform
		Ray direction = new Ray(Vector3.zero, transform.position.normalized);
		textComp.transform.position = direction.GetPoint(SpawnObjectsController.instance.Radius * .8f);
		Quaternion rot = transform.rotation;
		rot *= Quaternion.Euler(0, 90, 0);
		textComp.transform.rotation = rot;
		textComp.text = GetSumString(c);
		textComp.gameObject.SetActive(false);
	}

	private string GetSumString(Category c) {
		//return string.Format("<size=220%><font=\"Helvetica\">${0}m</size>\n<line-height=130%><mark=#FF0000><size=30%> {1} </line-height></size>\n {2} ", c.CurrentSum, "Design-related economic activity within the", c.Name);
		return string.Format("<size=220%><font=\"Helvetica\">${0}m</size>\n<size=30%>{1}</size>\n{2}", c.CurrentSum, "Design-related economic activity within the", c.Name);
	}

	private void Update() {

		if (!textComp.gameObject.activeInHierarchy) {
			return;
		}

		textComp.text = GetSumString(c);
		UpdateTextPosition();
	}

	private void UpdateTextPosition() {
		Vector3 hit = gazeFinder.GetRayCastHit().point;
		Vector3 position = textComp.transform.position;
		textComp.transform.position = new Vector3(position.x, hit.y, position.z);
	}

	private void ChangeMaterial(Color color) {
		foreach (Renderer rend in GetComponentsInChildren<Renderer>()) {
			rend.material.color = color;
		}
	}

	//private IEnumerator ChangeMaterial(Color color) {

	//	float duration = 5; // This will be your time in seconds.
	//	float smoothness = 0.1f; // This will determine the smoothness of the lerp. Smaller values are smoother. Really it's the time between updates.

	//	float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
	//	float increment = smoothness / duration; //The amount of change to apply.
	//	while (progress < 1) {
	//		foreach (Renderer rend in GetComponentsInChildren<Renderer>()) {

	//			rend.material.color = Color.Lerp(rend.material.color, color, progress);
	//		}
	//		progress += increment;
	//		yield return new WaitForSeconds(smoothness);
	//	}

	//	yield return null;
	//}



	public void OnGazeEnter() {
		//StartCoroutine(ChangeMaterial(SpawnObjectsController.instance.HighlightColor));
		Debug.Log("OnGazeEnter");
		ChangeMaterial(SpawnObjectsController.instance.HighlightColor);
		textComp.gameObject.SetActive(true);
	}

	public void OnGazeExit() {
		Debug.Log("OnGazeExit");
		ChangeMaterial(SpawnObjectsController.instance.Color1);
		//StartCoroutine(ChangeMaterial(SpawnObjectsController.instance.Color1));

		foreach (GameObject IB in SpawnObjectsController.instance.InvestBlocks) {
			Renderer rend = IB.GetComponentInChildren<Renderer>();
			rend.material.color = SpawnObjectsController.instance.InvestColor;
		}

		textComp.gameObject.SetActive(false);
	}
}

