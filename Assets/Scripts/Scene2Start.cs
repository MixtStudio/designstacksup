using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2Start : MonoBehaviour {

	// Use this for initialization
	void Start () {

		foreach (Grow grow in GameObject.FindObjectsOfType<Grow>()) {
			grow.BeginGrowing(0);
		}

		foreach (Leaves leaf in GameObject.FindObjectsOfType<Leaves>()) {
			leaf.BeginColourBleed();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
