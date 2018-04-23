using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace HoloToolkit.Unity.InputModule.Tests {

	public class TextDisplay : MonoBehaviour, IFocusable {

		public string info { get; set; }
		public IPointingSource Pointer { get; }
	

		public GameObject _textContainer;
		public float distance = 1;
		private GameObject textContainer;
		private Text text;
		public Transform textContainerTransform;
		
		private void Start() {
			textContainer = Instantiate(_textContainer, transform, false);
			//Getting the text
			Canvas canvasText = textContainer.GetComponentInChildren<Canvas>();
			text = canvasText.GetComponentInChildren<Text>();
			text.text = info;
			textContainer.gameObject.SetActive(false);
		}

		
		private void Update() {
			textContainer.transform.rotation= textContainerTransform.rotation;
			textContainer.transform.position = textContainerTransform.position;
		}


		public void OnFocusEnter() { textContainer.gameObject.SetActive(true); }
		public void OnFocusExit() { textContainer.gameObject.SetActive(false); }

	}
}
