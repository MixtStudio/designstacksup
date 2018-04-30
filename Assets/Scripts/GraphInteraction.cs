using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace HoloToolkit.Unity.InputModule.Tests {

	public class GraphInteraction:MonoBehaviour,IFocusable {

		public string industryName { get; set; }			
		public TMP_Text TextPrefab { get; set; }
		private TMP_Text textComp;
		private Color normalColor;
		private Color highlightColor;

		
		private void Start() {
			normalColor = transform.GetChild(0).GetComponent<Renderer>().material.color;
			highlightColor = SpawnObjectsController.instance.HighlightColor;
			textComp = Instantiate(TextPrefab);
			textComp.transform.parent = SpawnObjectsController.instance.textContainer.transform;
			textComp.name = "Text " + industryName;
			//Sets the initial Text Transform
			Ray direction = new Ray(Vector3.zero, transform.position.normalized);
			textComp.transform.position = direction.GetPoint(SpawnObjectsController.instance.Radius * .8f);
			Quaternion rot = transform.rotation;
			rot *= Quaternion.Euler(0, 90, 0);
			textComp.transform.rotation = rot;
			textComp.text = industryName;
			textComp.gameObject.SetActive(false);
		}


		private void Update() {

			if (!textComp.gameObject.activeInHierarchy) {
				return;
			}
			UpdateTextPosition();
		}
		
		private void UpdateTextPosition() {
			Vector3 hit = GazeManager.Instance.HitInfo.point;
			Vector3 position = textComp.transform.position;
			textComp.transform.position = new Vector3(position.x, hit.y, position.z);
		}

		private void ChangeMaterial(Color color) {
			foreach (Transform child in transform) {
				Debug.Log(child.name);
				child.GetComponent<Renderer>().material.color = color;
			}
		}

		public void OnFocusEnter() {
			ChangeMaterial(highlightColor);
			textComp.gameObject.SetActive(true);
		}

		public void OnFocusExit() {
			ChangeMaterial(normalColor);
			textComp.gameObject.SetActive(false);
		}
	}
}
