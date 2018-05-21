using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class that attaches to a spawner object, gets the public variables and does things that are non-specific to the
/// type of object.
/// </summary>
public class SpawnObjectsController : Mixt.Singleton<SpawnObjectsController> {

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

	//[SerializeField]
	//private TMP_Text _textPrefab;
	//public TMP_Text TextPrefab => _textPrefab;



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

	[HideInInspector]
	public float PrefabSize = 1f;

	//[HideInInspector]
	//public static SpawnObjectsController instance;


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

	//public Prompts PromptsInstance;

	protected override void Init() {
		//instance = this;
		CategoryList = new List<Category>();
		InvestBlocks = new List<GameObject>();
		CreateHarcodeBar();
	}


	private void CreateHarcodeBar() {

		BarsHolder = new GameObject() {
			name = "BarsHolder",
		};

		TextContainer = new GameObject() {
			name = "TextContainer",
		};

		CategoryList = new List<Category>();
		// 2. Fills categoryList
		CategoryList = GetHardCodeList();
		MaxCategorySum = GetMaxCategorySum();
		FallingBlocksInstance = new SpawnFallingBlocks();
		
		//3.Uses data to create objects
		FallingBlocksInstance.CreateGraphs();
	}




	private List<Category>  GetHardCodeList() {
		List<Category> categories = new List<Category>();

		categories.Add(new Category("Manufacturing Industry", 2691));
		categories.Add(new Category("Human Health", 895));
		categories.Add(new Category("Financial", 757));
		categories.Add(new Category("Environmental Industry", 651));
		categories.Add(new Category("Construction", 607));
		categories.Add(new Category("Agriculture", 467));
		categories.Add(new Category("Logistics Industry", 445));
		categories.Add(new Category("Retail Industry", 432));
		categories.Add(new Category("Other Public Sector", 428));
		categories.Add(new Category("Food and Beverage\nIndustry", 378));
		categories.Add(new Category("Defence Industry", 298));
		categories.Add(new Category("Engineering Industry\n(excluding Civils)", 269));
		categories.Add(new Category("Digital and Creative\nIndustry", 261));
		categories.Add(new Category("Process Industry", 257));
		categories.Add(new Category("Utilities", 231));
		categories.Add(new Category("Marine Industry", 173));
		categories.Add(new Category("Designer Fashion", 172));
		categories.Add(new Category("Communications Industry", 157));
		categories.Add(new Category("Education", 133));
		categories.Add(new Category("Leisure Industry less tourism", 124));
		categories.Add(new Category("Public & Private Transport\nIndustry(not Freight)", 58));
		categories.Add(new Category("Aerospace Industry", 46));
		categories.Add(new Category("Automotive Industry", 42));
		categories.Add(new Category("Tourism", 40));
		categories.Add(new Category("Security & Cyber", 30));
		categories.Add(new Category("Commercial Travel Industry", 27));
		categories.Add(new Category("Animal Health", 13));
		categories.Add(new Category("Civil Engineering Industry", 12));
		categories.Add(new Category("Data Services", 3));
		return categories;
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
