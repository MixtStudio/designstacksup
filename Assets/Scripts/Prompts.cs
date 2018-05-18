using System.Collections;
using System;
using System.ComponentModel;
using UnityEngine;
using System.Collections.Generic;
using TMPro;


public static class Prompts {

	public enum PromptName {
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

	private static Dictionary<PromptName, DynamicTextController> promptObjects = new Dictionary<PromptName ,DynamicTextController>();
	private static Dictionary<PromptName, string> prompts;
	//private TMP_Text TextPrefab;
	
	static Prompts() {
		FillPromptList();
	}

	private static void FillPromptList() {

		//TextPrefab = SpawnObjectsController.instance.TextPrefabBlack;
		//string bgColor = "#FFFFFF";
		//string bgColor = "#FF0000";
		//string fontColor ="#000000";
		
		prompts = new Dictionary<PromptName, string>() {
			{ PromptName.SCN1_DIAL_UP_DESIGN,"<size=20%>\u3001 Dial up\n"+
											 "Design</size>"},

			{ PromptName.GN_INTRO,"<size=40%>Goodnature applied design principles\n"+
								  "to make a humane, toxin-free,\n" +
								  "targeted, ergonomically easy-to-use\n"+
								  "and labour-saving trapping system.\n"+
								  "Their vision is a natural environment\n" +
								  "in which native species survive\n"+
								  "and thrive,free from the threat\n"+
								  "of introduced pests.</size>"},

			{PromptName.GN_DIAL_UP_DESIGN, "Throw this Goodnature trap/n" +
											"to discover its impact </size>" },

			{ PromptName.GN_FACT_1, "<size=200%>34 visits\n reduced to 2\n" +
									"</size>Goodnature’s design reduced the number of\n"+
									"annual visits by a Department of Conservation (DOC)/n"+
									"person required to maintain traps by a factor"+
									"of 34 visits to just 2."},

			{ PromptName.GN_FACT_2,"Cost ↓90%\n" +
								  "Goodnature’s trapping system has allowed\n"+
								  "DOC to eliminate all trace of rats, and slashed\n"+
								  "the cost of trapping by an astonishing 90%."},

			{ PromptName.GN_FACT_3, "From backyards\n"+
									"to 10,000ha\n" +
									"Goodnature’s designers have developed a system\n" +
									"that works equally as well in a person’s backyard\n"+
									"as in a 10,000 hectare conservation site allowing\n"+
									"all New Zealanders to engage in conservation efforts,\n"+
									"and DOC to improve the health of whole forests."},

			{PromptName.AB_INTRO, "Design has made Allbirds the world’s\n"+
								  "largest direct-to-consumer shoe brand.\n"+
								  "This New Zealand firm only sells\n"+
								  "its merino wool fabric shoes online."},

			{PromptName.AB_DIAL_UP_DESIGN, "Dial up this shoe design to\n"+
											"see Allbirds success" },

			{PromptName.AB_FACT_1,"1 product\n"+
								  "Allbirds produces just one product, perfectly done.\n"+
								  "These aren’t shoes, these are Allbirds.\n"+
								  "Design was used to strip away the unneccessary —\n"+
								  "this is Allbirds’ philosophy across everything it does,\n"+
								  "including product, brand, marketing and communication."},

			{PromptName.AB_FACT_2,"2015 – 2 staff\n"+
								  "2016 – 30 staff\n"+
								  "2018 – 100+ staff\n"+
								  "Allbirds started with two staff at the end of 2015,\n"+
								  "30 in 2016, and now has over 100."},

			{PromptName.AB_FACT_3,"1 million pairs/n"+
								  "of shoes/n"+
								  "sold in 2 years/n"+
								  "In two years Allbirds have sold 1 million shoes./n"+
								  "Allbirds is the world’s largest direct-to-consumer shoe/n"+
								  "brand that makes its own products and sells them./n"+
								  "Design has been an effective way to overcome the/n"+
								  "small size of the New Zealand market."}
		};
	}

	private static DynamicTextController CreatePrompt(string text) {
		DynamicTextController obj = GameObject.Instantiate(SpawnObjectsController.Instance.DynamicTextPrefab);
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
		if (!promptObjects.TryGetValue(prompt, out obj)) {
			// no text object exists for the prompt, so we create it now
			obj = CreatePrompt(text);
			promptObjects[prompt] = obj;
		}
		return obj;
	}

	//public TMP_Text GetMyText(int promptNumber) {
	//	TMP_Text textComp;
	//	textComp = Instantiate(TextPrefab);
	//	//textComp.name = "Text " + c.Nam;
	//	textComp.text = prompts[promptNumber];
	//	return textComp;
	//}
	
}
