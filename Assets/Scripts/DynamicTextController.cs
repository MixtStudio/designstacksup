using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DynamicTextController : MonoBehaviour {

	[SerializeField]
	private DynamicTextContainer textContainer;

	private List<DynamicTextContainer> containerList = new List<DynamicTextContainer>();

	private string rawText;

	public string text {
		get { return rawText; }
		set {
			if (rawText == value) {
				return;
			}
			rawText = value;
			SetText(rawText);
		}
	}

	private void SetText(string text) {
	
		string[] lines = text.Split('\n');
		//DynamicTextContainer lineContainer;
		for (int line = 0; line < lines.Length; line++) {
			if (containerList.Count < line + 1) {
				containerList.Add(Instantiate(textContainer, this.transform));
			}
			containerList[line].gameObject.SetActive(true);
			containerList[line].text_obj.text = lines[line];
		}

		for (int extraLines = lines.Length; extraLines < containerList.Count; extraLines++) {
			containerList[extraLines].gameObject.SetActive(false);
		}
	}


}
