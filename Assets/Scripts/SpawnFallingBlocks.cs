using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Creates the positions for the blocks and text to spawn at, and spawns them in.
/// </summary>
public class SpawnFallingBlocks {
	private static SpawnObjectsController SOC;
	private float radius, prefabSize;
	private GameObject FallingBlock;
	private TextMesh TextPrefab;
	private float maxCategorySum;


	/// <summary>
	/// Constructor to bring in the variables from SpawnObjectsController.
	/// </summary>
	public SpawnFallingBlocks() {
		SOC = SpawnObjectsController.instance;
		radius = SOC.Radius;
		prefabSize = SOC.PrefabSize;
		FallingBlock = SOC.FallingBlock;
		TextPrefab = SOC.TextPrefab;
		maxCategorySum = SOC.MaxCategorySum;
	}

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
		var wait = new WaitForSeconds(.1f);

		GameObject barsHolder = new GameObject() {
			name = "BarsHolder",
			//tag="BarHolder",
			layer = LayerMask.NameToLayer("Gaze")
		};

		barsHolder.AddComponent<ScaleChanger>();

		// Loop through category list
		foreach (Category c in categoryList) {
			c.CategoryContainer = new GameObject() {
				name = c.Name,
				layer = LayerMask.NameToLayer("Gaze")
			};

			c.CategoryContainer.transform.parent = barsHolder.transform;
			c.CategoryContainer.transform.localPosition = c.Position;
			c.CategoryContainer.transform.localRotation = Quaternion.Euler(0, -c.Angle, 0);

			float sum = c.Sum * 20 / maxCategorySum;

			for (var i = 0; i < sum; i++) {
				SpawnBlock(c, i / sum, 20+i);
				yield return wait;
			}

			yield return wait;	
		}

		foreach( Category c in categoryList) {
			//AddCollider(c);
			//CreateText(c);
			//GazeInteraction(c);
		}

		yield break;
	}

	private BoxCollider AddCollider(Category c) {
		BoxCollider col = c.CategoryContainer.AddComponent<BoxCollider>();
		col.isTrigger = true;	
		FitColliderToChildren(c.CategoryContainer);
		return col;
	}

	//private void GazeInteraction(Category c) {
	//	CategoryGaze gaze = c.CategoryContainer.AddComponent<CategoryGaze>();
	//	gaze.frontObject = TextPrefab;
	//	gaze.MsgToPrint = c.Name;
	//}

	private void FitColliderToChildren(GameObject parentObject) {
		BoxCollider bc = parentObject.GetComponent<BoxCollider>();
		if (bc == null) { bc = parentObject.AddComponent<BoxCollider>(); }
		Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
		bool hasBounds = false;
		Renderer[] renderers = parentObject.GetComponentsInChildren<Renderer>();
		foreach (Renderer render in renderers) {
			if (hasBounds) {
				bounds.Encapsulate(render.bounds);
			} else {
				bounds = render.bounds;
				hasBounds = true;
			}
		}
		if (hasBounds) {
			Vector3 targetCenter = bounds.center;
			targetCenter.y = bounds.size.y / 2;
			bc.center = targetCenter - parentObject.transform.position;
			bc.size = bounds.size;
		} else {
			bc.size = bc.center = Vector3.zero;
			bc.size = Vector3.zero;
		}
	}

	

/// <summary>
/// Instantiates a block of the defined size at the defined position, and sets its color.
/// </summary>
/// <param name="position">Position of the block.</param>
/// <param name="size">Size of the block.</param>
void SpawnBlock(Category c, float index, float posY) {
		//GameObject o = Object.Instantiate(FallingBlock, c.Position + Vector3.up * 20, Quaternion.Euler(0, -c.Angle, 0), c.CategoryContainer.transform);
		GameObject o = Object.Instantiate(FallingBlock, c.CategoryContainer.transform, false);
		o.transform.localEulerAngles = Vector3.zero;
		o.transform.localPosition = Vector3.up * posY;
		o.transform.localScale *= prefabSize;
		o.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.yellow, index);
	}
}
