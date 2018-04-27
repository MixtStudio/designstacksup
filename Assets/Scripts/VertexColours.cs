using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexColours : MonoBehaviour {

	public Color startColor;
	public Color targetColor;

	public float colourSpeed = 0.25f;
	public float spreadSpeed = 0.25f;

	private float[] deltas;
	private bool scaling = false;
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

		for (int i = 0; i < uv.Length; i++) {
			if (uv[i].x <= uvThreshold) {
				deltas[i] += Time.deltaTime * colourSpeed;
				colors[i] = Color.Lerp(startColor, targetColor, deltas[i]);
			} else
				colors[i] = startColor;
		}

		mesh.colors = colors;

		if (deltas[deltas.Length-1] >= 1.0f) {
			changingDone = true;
		}

		uvThreshold += Time.deltaTime * spreadSpeed;
	}
}
