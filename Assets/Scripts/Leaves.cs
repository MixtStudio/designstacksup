using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaves : MonoBehaviour {

	private Color startColor;
	public Color targetColor;

	public float colourSpeed = 0.1f;
	public float spreadSpeed = 0.2f;
	public float bleedThreshold = 0.1f;

	private float[] deltas;
	private bool beginBleeding = false;
	private bool changingDone = false;
	private float uvThreshold = 1.0f;

	private Mesh mesh;
	private Vector2[] uv;
	private Color[] colors;

	void OnEnable() {
		MeshRenderer mr = GetComponent<MeshRenderer>();
		Material newMat = new Material(mr.material);
		Color colour = mr.material.color;
		colour = new Color(colour.r + Random.Range(0.0f, 0.5f),
						   colour.g + Random.Range(-0.25f, 0.25f),
						   colour.b + Random.Range(0.0f, 0.5f));
		newMat.color = colour;
		mr.material = newMat;
		startColor = colour;

		mesh = GetComponent<MeshFilter>().mesh;
		uv = mesh.uv;
		colors = new Color[uv.Length];
		deltas = new float[uv.Length];
		for (int i = 0; i < uv.Length; i++) {
			deltas[i] = 0.0f;
			colors[i] = startColor;
		}
		mesh.colors = colors;

		BeginColourBleed();
	}

	void Update() {
		if (!changingDone && beginBleeding)
			ColourBleed();

		for (int i = 0; i < uv.Length; i++) {
			if (!(uv[i].y >= uvThreshold)) {
				//colors[i] = startColor;
			}
		}
		//mesh.colors = colors;
	}

	public void BeginColourBleed() {
		beginBleeding = true;
	}

	private void ColourBleed() {
		//Debug.Log(uvThreshold);
		if (uvThreshold >= bleedThreshold)
			uvThreshold -= Time.deltaTime * spreadSpeed;
		for (int i = 0; i < uv.Length; i++) {
			if ((uv[i].y >= uvThreshold)) {
				//Debug.Log("Pass");
				deltas[i] += Time.deltaTime * colourSpeed;
				colors[i] = Color.Lerp(startColor, targetColor, deltas[i]);
			} else {
				colors[i] = startColor;
				//Debug.Log(uv[i].y);
			}
		}

		mesh.colors = colors;
		/*
		if (deltas[deltas.Length - 4] >= 1.0f) {
			changingDone = true;
			Debug.Log("DONE");
		}
		*/
		
		
		
		
	}

}
