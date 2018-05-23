using System.Collections;
using System;
using System.ComponentModel;
using UnityEngine;
using System.Collections.Generic;
using TMPro;


public static class Prompts {

	public static DynamicTextController dynamicTextPrefab;

	public enum PromptName {
		PRESS_ANY_BUTTON_TO_START,
		GRAB_ME,
		SCN1_DIAL_UP_DESIGN,
		GN_INTRO,
		GN_DIAL_UP_DESIGN,
		GN_FACT_1,
		GN_FACT_2,
		GN_FACT_3,
		AB_INTRO,
		AB_DIAL_UP_DESIGN,
		AB_FACT_1,
		AB_FACT_2,
		AB_FACT_3
	}

	private static Dictionary<PromptName, DynamicTextController> promptObjects = new Dictionary<PromptName, DynamicTextController>();
	private static Dictionary<PromptName, string> prompts;


	static Prompts() {
		FillPromptList();

		CoroutineUtils.InvokeOnMain(() => {
			dynamicTextPrefab = Resources.Load<DynamicTextController>("Prefabs/DynamicTextController");
			Debug.LogFormat("Prompts::StaticConstructor | Loaded text controller prefab[{0}]", dynamicTextPrefab);
		});
	}

	private static void FillPromptList() {

		string downArrow = "\u3003";
		string upArrow = "\u3001";
		string headerSize = "<size=180%>";
		//string bodySize = "<size=30%>";

		prompts = new Dictionary<PromptName, string>() {
			{PromptName.PRESS_ANY_BUTTON_TO_START,"Press any button to start" },

			{PromptName.GRAB_ME,string.Format("Grab me {0}", downArrow)},

			{ PromptName.SCN1_DIAL_UP_DESIGN,string.Format("{0} Dial up\n"+
											 "Design",upArrow)},

			{ PromptName.GN_INTRO,"Goodnature applied design principles\n"+
								  "to make a humane, toxin-free,\n" +
								  "targeted, ergonomically easy-to-use\n"+
								  "and labour-saving trapping system.\n"+
								  "Their vision is a natural environment\n" +
								  "in which native species survive\n"+
								  "and thrive, free from the threat\n"+
								  "of introduced pests."},

			{PromptName.GN_DIAL_UP_DESIGN, "Pick up and throw this Goodnature trap\n" +
											"to discover its impact"},

			{ PromptName.GN_FACT_1, string.Format("{0}34 visits\n "+
									"{0}reduced to 2\n" +
									"Goodnature’s design reduced the number of\n"+
									"annual visits by a Department of Conservation (DOC)\n"+
									"person required to maintain traps by a factor\n"+
									"of 34 visits to just 2.",headerSize)},

			{ PromptName.GN_FACT_2,string.Format("{0}Cost {1} 90%\n" +
								  "Goodnature’s trapping system has allowed\n"+
								  "DOC to eliminate all trace of rats, and slashed\n"+
								  "the cost of trapping by an astonishing 90%.",headerSize,downArrow)},

			{ PromptName.GN_FACT_3,string.Format ("{0}From backyards\n"+
									"{0}to 10,000ha\n" +
									"Goodnature’s designers have developed a system\n" +
									"that works equally as well in a person’s backyard\n"+
									"as in a 10,000 hectare conservation site allowing\n"+
									"all New Zealanders to engage in conservation efforts,\n"+
									"and DOC to improve the health of whole forests.",headerSize)},

			{PromptName.AB_INTRO, "Design has made Allbirds the world’s\n"+
								  "largest direct-to-consumer shoe brand.\n"+
								  "This New Zealand firm only sells\n"+
								  "its merino wool fabric shoes online."},

			{PromptName.AB_DIAL_UP_DESIGN, "Place this dial on the shoe to\n"+
											"see Allbirds success"},

			{PromptName.AB_FACT_1,string.Format("{0}1 product\n"+
								  "Allbirds produces just one product, perfectly done.\n"+
								  "These aren’t shoes, these are Allbirds.\n"+
								  "Design was used to strip away the unneccessary —\n"+
								  "this is Allbirds’ philosophy across everything it does,\n"+
								  "including product, brand, marketing and communication.",headerSize)},

			{PromptName.AB_FACT_2,string.Format("{0}2015 – 2 staff\n"+
								  "{0}2016 – 30 staff\n"+
								  "{0}2018 – 100+ staff\n"+
								  "Allbirds started with two staff at the end of 2015,\n"+
								  "30 in 2016, and now has over 100.",headerSize)},

			{PromptName.AB_FACT_3,string.Format("{0}1 million pairs\n"+
								  "{0}of shoes\n"+
								  "{0}sold in 2 years\n"+
								  "In two years Allbirds have sold 1 million shoes.\n"+
								  "Allbirds is the world’s largest direct-to-consumer shoe\n"+
								  "brand that makes its own products and sells them.\n"+
								  "Design has been an effective way to overcome the\n"+
								  "small size of the New Zealand market.",headerSize)}
		};
	}

	private static DynamicTextController CreatePrompt(string text) {
		DynamicTextController obj = GameObject.Instantiate(dynamicTextPrefab);
		obj.text = text;
		return obj;
	}

	public static DynamicTextController GetPrompt(PromptName prompt) {
		string text;
		if (!prompts.TryGetValue(prompt, out text)) {
			Debug.LogError("No Prompt found for type: " + prompt);
			return null;
		}

		DynamicTextController obj;
		if (!promptObjects.TryGetValue(prompt, out obj) || obj == null) {
			// no text object exists for the prompt, so we create it now
			obj = CreatePrompt(text);
			promptObjects[prompt] = obj;
		}
		return obj;
	}

	////With position
	//public static DynamicTextController GetPrompt(Vector3 position, PromptName promptName, Transform parent = null) {
	//	DynamicTextController prompt = GetPrompt(promptName);
	//	prompt.transform.SetParent(parent);
	//	prompt.name = promptName.ToString();
	//	Debug.Log("Placing at position: " + position);
	//	prompt.transform.localPosition = position;
	//	//prompt.transform.rotation = rotation;
	//	return prompt;
	//}

	//With scale
	public static DynamicTextController GetPrompt(Vector3 position, Quaternion rotation,PromptName promptName, float scale = 1, Transform parent = null) {
		DynamicTextController prompt = GetPrompt(promptName);
		prompt.transform.SetParent(parent);
		prompt.name = promptName.ToString();
		Debug.Log("Placing at position: " + position);
		prompt.transform.localPosition = position;
		prompt.transform.rotation = rotation;
		prompt.transform.localScale *= scale;
		return prompt;
	}

	//Restart text
	public static DynamicTextController GetPrompt(DynamicTextController textObject, Vector3 position,Quaternion rotation, PromptName promptName, float scale = 1,Transform parent = null) {
		if (textObject == null) {
			return GetPrompt(position, rotation,promptName, scale,parent);
		}

		string text = null;
		if (!prompts.TryGetValue(promptName, out text)) {
			Debug.LogError("No Prompt found for type: " + promptName);
			return textObject;
		}
		textObject.text = text;
		textObject.name = promptName.ToString();
		textObject.transform.position = position;
		//prompt.transform.rotation = rotation;
		return textObject;
	}

	public static void DestroyPrompt(DynamicTextController dynamicTextController) {
		if (dynamicTextController == null || dynamicTextController.gameObject == null) {
			return;
		}
		UnityEngine.Object.Destroy(dynamicTextController.gameObject);
	}
}
