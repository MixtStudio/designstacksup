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
    public float Sum { get; set; }
	public GameObject CategoryContainer { get; set; }
	

    public Category(string categoryName) {
        Name = categoryName;
        Sum = 0;
    }
}
