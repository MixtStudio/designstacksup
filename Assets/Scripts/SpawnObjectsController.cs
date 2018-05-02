using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class that attaches to a spawner object, gets the public variables and does things that are non-specific to the
/// type of object.
/// </summary>
public class SpawnObjectsController : MonoBehaviour {
	[Header("Settings")]
	/// <summary>
	/// Exact file name of the csv file, must be in resources folder.
	/// </summary>
	[SerializeField]
	private string _inputFile;
	public string InputFile {
		get {
			return _inputFile;
		}
	}

	[SerializeField]
	private GameObject _normalBlock;
	public GameObject NormalBlock {
		get {
			return _normalBlock;
		}
	}

	[SerializeField]
	private GameObject _investBlock;
	public GameObject InvestBlock {
		get {
			return _investBlock;
		}
	}

	[SerializeField]
	private TMP_Text _textPrefab;
	public TMP_Text TextPrefab => _textPrefab;

	[SerializeField]
	private float _radius = 5;
	public float Radius {
		get {
			return _radius;
		}
	}

	[SerializeField]
	private Color _color1;
	public Color Color1 {
		get {
			return _color1;
		}
	}

	[SerializeField]
	private Color _color2;
	public Color Color2 {
		get {
			return _color2;
		}
	}

	[SerializeField]
	private Color _investColor;
	public Color InvestColor {
		get {
			return _investColor;
		}
	}

	[SerializeField]
	private Color _highlightColor;
	public Color HighlightColor {
		get {
			return _highlightColor;
		}
	}

	//[HideInInspector]
	public float PrefabSize = 1f;

	[HideInInspector]
	public static SpawnObjectsController instance;

	/// <summary>
	/// All the data values in the csv.
	/// </summary>
	private static List<Dictionary<string, object>> valueList;

	/// <summary>
	/// List of categories of data according to categoryColumnName.
	/// </summary>
	public static List<Category> CategoryList{ private set; get; }

	[HideInInspector]
	public List<GameObject> InvestBlocks { get; set; }

	[HideInInspector]
	/// <summary>
	/// Largest sum from all categories.
	/// </summary>
	public float MaxCategorySum = 0;

	public SpawnFallingBlocks FallingBlocksInstance { get; private set; }

	public GameObject BarsHolder { get; private set; }

	public GameObject TextContainer { get; private set; }

	private void Awake() {
		instance = this;
		CategoryList = new List<Category>();
		InvestBlocks = new List<GameObject>();
		CreateStatsBar();
	}


	private void CreateStatsBar() {

		BarsHolder = new GameObject() {
			name = "BarsHolder",
		};

	
		TextContainer = new GameObject() {
			name = "TextContainer",
		};

		valueList = CSVReader.Read(InputFile); // 1. Fills valueList
		CategoryList = new List<Category>();
		CategoryList = GetCategories(); // 2. Fills categoryList
		MaxCategorySum = GetMaxCategorySum();
		FallingBlocksInstance = new SpawnFallingBlocks();

		//3.Uses data to create objects
		FallingBlocksInstance.CreateGraphs();
	}


	/// <summary>
	/// Loops through pointList to find the categories.
	/// </summary>
	/// <returns>A list of Category objects from the "categoryColumnName" column in pointList.</returns>
	private List<Category> GetCategories() {
		List<Category> categories = new List<Category>();
		//string[] columnsName = GetColumnsName();

		List<string> nameList = new List<string>();
		for (var i = 0; i < valueList.Count; i++) {
			string aString = System.Convert.ToString(valueList[i]["Level 2"]);
			if (aString != "No Data") {
				if (!nameList.Contains(aString)) {

					categories.Add(new Category(aString));
					nameList.Add(aString);
				}
				categories[FindCategoryWithName(aString, categories)].Sum +=
														System.Convert.ToSingle(valueList[i]["GDP"]);
			}
		}

		return categories;
	}

	/// <summary>
	/// Finds index of a category name in a list.
	/// </summary>
	/// <returns>The index of the inputed category.</returns>
	/// <param name="searchedName">The category name that you want to find the index of.</param>
	/// /// <param name="categoryList">The list of categories.</param>
	public int FindCategoryWithName(string searchedName, List<Category> categoryList) {
		foreach (var c in categoryList) {
			if (c.Name.Equals(searchedName)) {
				return categoryList.IndexOf(c);
			}
		}
		return -1;
	}

	/// <summary>
	/// Finds largest sum from all categories.
	/// </summary>
	/// <returns>The max sum.</returns>
	private float GetMaxCategorySum() {
		float max = 0;
		foreach (var c in CategoryList) {
			if (c.Sum > max) {
				max = c.Sum;
			}
		}
		return max;
	}
}
