using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : Mixt.Singleton<TextManager> {
	
	[SerializeField]
	private DynamicTextController _dynamicTextPrefab;
	public DynamicTextController DynamicTextPrefab => _dynamicTextPrefab;
	
	protected override void Init() {}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
