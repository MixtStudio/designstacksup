using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule.Tests;
using System;

/// <summary>
/// Creates the positions for the blocks and text to spawn at, and spawns them in.
/// </summary>
public class SpawnFallingBlocks {
	private static SpawnObjectsController SOC;
	private float radius, prefabSize;
	private GameObject FallingBlock;
	private GameObject TextContainer;
	private GameObject TextContainerTransform;
	private float maxCategorySum;
	private Color[] colors;

	/// <summary>
	/// Constructor to bring in the variables from SpawnObjectsController.
	/// </summary>
	public SpawnFallingBlocks() {
		SOC = SpawnObjectsController.instance;
		radius = SOC.Radius;
		prefabSize = SOC.PrefabSize;
		FallingBlock = SOC.FallingBlock;
		TextContainer = SOC.TextContainer;
		maxCategorySum = SOC.MaxCategorySum;
		TextContainerTransform = SOC.TextContainerTransform;
		colors = new Color[] { SOC.Color1, SOC.Color2 };
	}

	public delegate void GraphCompletedEventHandler(object source, EventArgs args);

	public event GraphCompletedEventHandler GraphCompleted;

	/// <summary>
	/// Sets the position for each category in categoryList, equally spaced around a circle.
	/// </summary>
	public void BlockCategoryPositions(List<Category> categoryList) {
		int count = categoryList.Count;
		float spacing = 360f / count;

		prefabSize = 2 * radius * Mathf.Tan(Mathf.PI / count);

		for (int i = 0; i < count; i++) {
			float a = i * spacing;
			categoryList[i].Angle = a;

			float x = Mathf.Cos(a * Mathf.Deg2Rad) * (radius + 0.2794239f / 2 * prefabSize);
			float z = Mathf.Sin(a * Mathf.Deg2Rad) * (radius + 0.2794239f / 2 * prefabSize);

			categoryList[i].Position = new Vector3(x, 0, z);
		}
	}

	/// <summary>
	/// Starts the CreateBlocks coroutine using SpawnObjectsController because you can't start a coroutine without a
	/// MonoBehavior.
	/// </summary>
	public void CreateBlocks(List<Category> categoryList) {
		BlockCategoryPositions(categoryList);
		SOC.StartCoroutine(CreateBlocksCR(categoryList));
	}

	/// <summary>
	/// Creates the blocks according to their category's position, and spreads 'em out a bit.
	/// </summary>
	private IEnumerator CreateBlocksCR(List<Category> categoryList) {
		var wait = new WaitForSeconds(.05f);

		GameObject barsHolder = new GameObject() {
			name = "BarsHolder",
			tag = "Gaze"
		};

		GameObject.FindObjectOfType<TransitionManager>().SetBarsHolder(barsHolder);

		int index = categoryList.Count;

		//Allows a random fall of blocks
		while (index != 0) {
			Category c = categoryList[UnityEngine.Random.Range(0, categoryList.Count)];
			if(c.Exists != true) {
				c.CategoryContainer = new GameObject() {
					name = c.Name,
				};

				c.CategoryContainer.transform.parent = barsHolder.transform;
				c.CategoryContainer.transform.localPosition = c.Position;
				c.CategoryContainer.transform.localRotation = Quaternion.Euler(0, -c.Angle, 0);

				float sum = c.Sum * 20 / maxCategorySum;

				for (var i = 0; i < sum; i++) {
					SpawnBlock(c, 20 + i, index);
					yield return wait;
				}

				AddTextDisplay(c);
				c.Exists = true;
				index--;
			}
		}
		OnGraphCompleted();
		yield break;
	}

	protected virtual void OnGraphCompleted() {
		if (GraphCompleted != null)
			GraphCompleted(this, EventArgs.Empty);
	}


	private void AddTextDisplay(Category c) {
		var focusEvt = c.CategoryContainer.AddComponent<OnFocusEvent>();
		TextDisplay textDisp = c.CategoryContainer.AddComponent<TextDisplay>();
		textDisp.textContainerTransform = TextContainerTransform.transform;
		textDisp._textContainer = TextContainer;
		string categoryInfo = c.Name + " " + c.Sum.ToString();
		textDisp.info = categoryInfo;
	}

/// <summary>
/// Instantiates a block of the defined size at the defined position, and sets its color.
/// </summary>
/// <param name="position">Position of the block.</param>
/// <param name="size">Size of the block.</param>
void SpawnBlock(Category c, float posY,int index) {
		GameObject o = UnityEngine.Object.Instantiate(FallingBlock, c.CategoryContainer.transform, false);
		o.transform.localEulerAngles = Vector3.zero;
		o.transform.localPosition = Vector3.up * posY;
		o.transform.localScale *= prefabSize;
		if (index % 2 == 0) 
			 o.GetComponent<Renderer>().material.color=colors[0];
		else 
			o.GetComponent<Renderer>().material.color = colors[1];
	}
}
