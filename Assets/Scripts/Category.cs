using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class for each category that contains its name, angle, position and sum.
/// </summary>
public class Category
{
    public string Name { get; set; }
    public float Angle { get; set; }
    public Vector3 Position { get; set; }
    public int Sum { get; set; }
	public int CurrentSum { get; set; }
	public GameObject CategoryContainer { get; set; }
	public bool Exists { get; set; }
	
	public Category (string categoryName, int sum) {
		Name = categoryName;
		Sum = sum;
		CurrentSum = sum;
	}
}
