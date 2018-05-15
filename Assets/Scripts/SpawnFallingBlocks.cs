using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule.Tests;
using System;

/// <summary>
/// Creates the positions for the blocks and text to spawn at, and spawns them in.
/// </summary>
public class SpawnFallingBlocks: MonoBehaviour {
	private static SpawnObjectsController SOC;
	private float radius, prefabSize;
	private GameObject FallingBlock;
	private GameObject InvestBlock;
	private float maxCategorySum;
	private Color[] colors;

	/// <summary>
	/// Constructor to bring in the variables from SpawnObjectsController.
	/// </summary>
	public SpawnFallingBlocks() {
		SOC = SpawnObjectsController.instance;
		radius = SOC.Radius;
		prefabSize = SOC.PrefabSize;
		FallingBlock = SOC.NormalBlock;
		InvestBlock = SOC.InvestBlock;
		maxCategorySum = SOC.MaxCategorySum;
		colors = new Color[] { SOC.Color1, SOC.Color2 };
	}

	public delegate void GraphCompletedEventHandler(object source, EventArgs args);

	public event GraphCompletedEventHandler GraphCompleted;

	/// <summary>
	/// Sets the position for each category in categoryList, equally spaced around a circle.
	/// </summary>
	public void BlockCategoryPositions() {
		List<Category> categoryList = SpawnObjectsController.CategoryList;
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
	public void CreateGraphs() {
		BlockCategoryPositions();
		SOC.StartCoroutine(CreateBlocksCR());
	}

	/// <summary>
	/// Creates the blocks according to their category's position, and spreads 'em out a bit.
	/// </summary>
	private IEnumerator CreateBlocksCR() {
		List<Category> categoryList= SpawnObjectsController.CategoryList;

		var wait = new WaitForSeconds(.05f);

		GameObject.FindObjectOfType<TransitionManager>().SetBarsHolder(SOC.BarsHolder);

		int index = categoryList.Count;

		//Allows a random fall of blocks
		while (index != 0) {
			//To always know the index of the category in the SOC CategoryList
			int categoryIndex = UnityEngine.Random.Range(0, categoryList.Count);
			Category c = categoryList[categoryIndex];
			if(c.Exists != true) {
				c.CategoryContainer = new GameObject() {
					name = c.Name,
				};

				c.CategoryContainer.transform.parent = SOC.BarsHolder.transform;
				c.CategoryContainer.transform.localPosition = c.Position;
				c.CategoryContainer.transform.localRotation = Quaternion.Euler(0, -c.Angle, 0);

				float sum = c.Sum * 20 / maxCategorySum;

				//Creates the multiple blocks in the category
				for (var i = 0; i < sum; i++) {
					GameObject block = SpawnBlock(c, 20 + i,true);
					block.GetComponent<Renderer>().material.color = AssignColor(index);
					yield return wait;
				}

				//We create the invest block
				GameObject investBlock = SpawnBlock(c, 20 + sum + SOC.PrefabSize,false);
				SOC.InvestBlocks.Add(investBlock);
				//investBlock.layer = 11;
				investBlock.GetComponentInChildren<Renderer>().material.color = SOC.InvestColor;
			
		
				//Adds the interaction to category and block
				AddGraphInteraction(categoryIndex);
				c.Exists = true;
				index--;
			}
		}
		//To wait for all blocks to fall, to improve in the future
		wait = new WaitForSeconds(4);
		yield return wait;
		OnGraphCompleted();
		yield break;
	}

	protected virtual void OnGraphCompleted() {
		if (GraphCompleted != null) { }
			GraphCompleted(this, EventArgs.Empty);
	}

	
	private void AddGraphInteraction(int categoryIndex) {
		Category c = SpawnObjectsController.CategoryList[categoryIndex];
		var focusEvt = c.CategoryContainer.AddComponent<OnFocusEvent>();
		c.CategoryContainer.AddComponent<OnFocusEvent>();
		GraphInteraction graph_interaction = c.CategoryContainer.AddComponent<GraphInteraction>();
		graph_interaction.TextPrefab = SOC.TextPrefab;
		graph_interaction.categoryIndex = categoryIndex;
	}

	Color  AssignColor(int index) {
		if (index % 2 == 0)
			return  colors[0];
		else
			return colors[1];
	}

/// <summary>
/// Instantiates a block of the defined size at the defined position, if block type is true spawns normal block
/// </summary>
/// <param name="position">Position of the block.</param>
/// <param name="size">Size of the block.</param>
GameObject SpawnBlock(Category c, float posY, bool blockType) {

		GameObject o;
		//If block is true then is normal 
		if (blockType) {
			o = UnityEngine.Object.Instantiate(FallingBlock, c.CategoryContainer.transform,false);
		} 
		else {
			o = UnityEngine.Object.Instantiate(InvestBlock, c.CategoryContainer.transform,false);
		}

		o.transform.localEulerAngles = Vector3.zero;
		o.transform.localPosition = Vector3.up * posY;
		o.transform.localScale *= prefabSize;
		return o;
	}

}
