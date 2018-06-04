using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class GraphInteraction : MonoBehaviour {

	public DynamicTextController TextPrefab { get; set; }
	private string industryInfo { get; set; }
	private float minHeight = 1.3f;
	public Category c { get; set; }
	private DynamicTextController textComp;
	private Color currentColor;

	private void Start() {
		textComp = Instantiate(TextPrefab, SpawnObjectsController.Instance.TextContainer.transform);
		textComp.name = "Text " + c.Name;

		//Sets the initial Text Transform
		Ray direction = new Ray(Vector3.zero, transform.position.normalized);
		textComp.transform.position = direction.GetPoint(SpawnObjectsController.Instance.Radius * .8f);
		textComp.text = GetSumString(c);
		textComp.gameObject.SetActive(false);
		currentColor = SpawnObjectsController.Instance.Color1;
	}

	private string GetSumString(Category c) {
		return string.Format("<size=220%><line-height=50%>${0}m</size>\n" +
							"<size=60%><b>{1}</b></size>\n"
							+ "{2}", c.CurrentSum, "Design-related economic activity within the", c.Name);
	}

	private void Update() {

		if (!textComp.gameObject.activeInHierarchy) {
			return;
		}

		textComp.text = GetSumString(c);
		UpdateTextTransform();
	}

	private void UpdateTextTransform() {
		Vector3 hit = GazeFinder.Instance.GetRayCastHit().point;
		Vector3 position = textComp.transform.position;

		textComp.transform.position = new Vector3(position.x, hit.y, position.z);
		if (textComp.transform.position.y < minHeight)
			textComp.transform.position = new Vector3(textComp.transform.position.x, minHeight, textComp.transform.position.z);

		textComp.transform.rotation = TransformUtils.GetLookAtRotation(textComp.transform);
		Vector3 v = textComp.transform.rotation.eulerAngles;
		textComp.transform.rotation = Quaternion.Euler(0, v.y, 0);
	}


	public void OnGazeEnter() {
		currentColor = GetComponentInChildren<Renderer>().material.color;
		foreach (Renderer rend in GetComponentsInChildren<Renderer>()) {
			rend.material.color = SpawnObjectsController.Instance.HighlightColor;
		}

		textComp.gameObject.SetActive(true);
	}

	public void OnGazeExit() {

		foreach (Renderer rend in GetComponentsInChildren<Renderer>()) {
			rend.material.color = currentColor;
		}

		foreach (GameObject IB in SpawnObjectsController.Instance.InvestBlocks) {
			Renderer rend = IB.GetComponentInChildren<Renderer>();
			rend.material.color = SpawnObjectsController.Instance.InvestColor;
		}

		textComp.gameObject.SetActive(false);
	}
}
