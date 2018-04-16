using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class that attaches to a spawner object, gets the public variables and does things that are non-specific to the
/// type of object.
/// </summary>
public class SpawnObjectsController: MonoBehaviour{
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
	private int numberOfWalls;

	[SerializeField]
	private int heightOfWalls;

	/// <summary>
	/// Select which column to classify by
	/// </summary>
	public enum ClassificationLevel { MarketVerticals, DesignDisciplines, DoubleDimond, DesignProcess }
	[SerializeField]
	private ClassificationLevel _level;
	public ClassificationLevel Level {
		get {
			return _level;
		}
	}

	/// <summary>
	/// Select the column of values
	/// </summary>
	public enum DataValues { GVA, GDP, TotalSales, AvailableSales }
	[SerializeField]
	private DataValues _dataRep;
	public DataValues DataRep {
		get {
			return _dataRep;
		}
	}

	[SerializeField]
	private GameObject _fallingBlock;
	public GameObject FallingBlock {
		get {
			return _fallingBlock;
		}
	}

	[SerializeField]
	private TextMesh _textPrefab;
	public TextMesh TextPrefab {
		get {
			return _textPrefab;
		}
	}

	[SerializeField]
	private float _radius = 5;
	public float Radius {
		get {
			return _radius;
		}
	}

	[SerializeField]
	private float _prefabSize = 1f;
	public float PrefabSize {
		get {
			return _prefabSize;
		}
	}

	public static SpawnObjectsController instance;

	/// <summary>
	/// All the data values in the csv.
	/// </summary>
	private static List<Dictionary<string, object>> valueList;

	/// <summary>
	/// List of categories of data according to categoryColumnName.
	/// </summary>
	private static List<Category> categoryList;

	/// <summary>
	/// Largest sum from all categories.
	/// </summary>
	public float MaxCategorySum = 0;

	private void Awake() {
		instance = this;
		categoryList = new List<Category>();
		//categoryList = CategoriesSameHeight(numberOfWalls, heightOfWalls);
		//MaxCategorySum = 5;
		//SpawnFallingBlocks sFB = new SpawnFallingBlocks();
		//sFB.CreateBlocks(categoryList);
		CreateStatsBar();
	}

	/*
	void Update() {
		if (Input.GetKeyDown("space")) {
			Debug.Log("HEYYYY");
			CreateStatsBar();
		}
	}
	*/

	private void CreateStatsBar() {

		//GameObject walls = GameObject.FindWithTag("BarHolder");
		GameObject walls = GameObject.Find("BarsHolder");
		Destroy(walls);
		categoryList.Clear();
		var watch = System.Diagnostics.Stopwatch.StartNew();
		// the code that you want to measure comes here
		valueList = CSVReader.Read(InputFile); // 1. Fills valueList
		watch.Stop();
		var elapsedMs = watch.ElapsedMilliseconds;
		Debug.Log("Time is: " + elapsedMs);
		categoryList = new List<Category>();
		categoryList = GetCategories(); // 2. Fills categoryList
		MaxCategorySum = GetMaxCategorySum();
		SpawnFallingBlocks sFBx = new SpawnFallingBlocks();

		//3.Uses data to create objects
		sFBx.CreateBlocks(categoryList);
	}

	private List<Category> CategoriesSameHeight(int numberWalls, float height) {
		List<Category> categories = new List<Category>();
		for (int i = 0; i < numberWalls; i++) {
			string categoryName = "Wall_" + i;
			Category c = new Category(categoryName) {
				Sum = height
			};
			categories.Add(c);
		}

		return categories;
	}

	/// <summary>
	/// Returns the name of the columns according to user input
	/// </summary>
	private string[] GetColumnsName(ClassificationLevel level, DataValues dataRep) {
		string[] columnsName = new string[2];

		switch (level) {
			case ClassificationLevel.MarketVerticals:
				columnsName[0] = "Level 2";
				break;
			case ClassificationLevel.DesignDisciplines:
				columnsName[0] = "Level 3";
				break;
			case ClassificationLevel.DoubleDimond:
				columnsName[0] = "Level 4";
				break;
			case ClassificationLevel.DesignProcess:
				columnsName[0] = "Level 5";
				break;
			default:
				columnsName[0] = "Level 2";
				break;
		}

		switch (dataRep) {
			case DataValues.GVA:
				columnsName[1] = "GVA";
				break;
			case DataValues.GDP:
				columnsName[1] = "GDP";
				break;
			case DataValues.TotalSales:
				columnsName[1] = "Total Sales $m";
				break;
			case DataValues.AvailableSales:
				columnsName[1] = "Available Sales $m";
				break;
			default:
				columnsName[1] = "GDP";
				break;
		}

		return columnsName;
	}

	/// <summary>
	/// Loops through pointList to find the categories.
	/// </summary>
	/// <returns>A list of Category objects from the "categoryColumnName" column in pointList.</returns>
	private List<Category> GetCategories() {
		List<Category> categories = new List<Category>();
		string[] columnsName = GetColumnsName(Level, DataRep);

		List<string> nameList = new List<string>();
		for (var i = 0; i < valueList.Count; i++) {
			string aString = System.Convert.ToString(valueList[i][columnsName[0]]);
			if (aString != "No Data") {
				if (!nameList.Contains(aString)) {

					categories.Add(new Category(aString));
					nameList.Add(aString);
				}
				categories[FindCategoryWithName(aString, categories)].Sum +=
														System.Convert.ToSingle(valueList[i][columnsName[1]]);
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
		foreach (var c in categoryList) {
			if (c.Sum > max) {
				max = c.Sum;
			}
		}
		return max;
	}
}
