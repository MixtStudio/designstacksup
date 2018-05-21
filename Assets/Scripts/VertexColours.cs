using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexColours : MonoBehaviour {

	public Color startColor;
	public Color targetColor;

	public float colourSpeed = 0.25f;
	public float spreadSpeed = 0.25f;
	public float bleedThreshold = 0.1f;

	private float[] deltas;
	private bool changingDone = false;
	private float uvThreshold = 0.0f;

	private Mesh mesh;
	private Vector2[] uv;
	private Color[] colors;


	void Start() {
		mesh = GetComponent<MeshFilter>().mesh;
		uv = mesh.uv;
		colors = new Color[uv.Length];
		deltas = new float[uv.Length];
		for (int i = 0; i < uv.Length; i++) {
			deltas[i] = 0.0f;
			colors[i] = startColor;
		}
		mesh.colors = colors;
	}

	void Update () {
		if(!changingDone)
			ColourBleed();
	}

	private void ColourBleed() {
		if (uvThreshold <= bleedThreshold)
			uvThreshold += Time.deltaTime * spreadSpeed;
		for (int i = 0; i < uv.Length; i++) {
			if (uv[i].x <= uvThreshold) {
				deltas[i] += Time.deltaTime * colourSpeed;
				colors[i] = Color.Lerp(startColor, targetColor, deltas[i]);
			}
		}

		mesh.colors = colors;
		/*
		if (deltas[deltas.Length-1] >= 1.0f) {
			changingDone = true;
		}
		*/
	}
}
