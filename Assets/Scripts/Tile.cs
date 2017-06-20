using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	public float width;
	public float height;

	[Space(16)]
	public float widthRatio;
	public float heightRatio;

	[Space(16)]
	public Color selectedColor;
	Color standardColor;

	new SpriteRenderer renderer;

	bool selected = false;

	void Start() {
		renderer = GetComponent<SpriteRenderer>();
		standardColor = renderer.color;
		UpdateColor();
	}

	void UpdateColor() {
		if (renderer) {
			renderer.color = selected ? selectedColor : standardColor;
		}
	}

	public void Select() {
		selected = true;
		UpdateColor();
	}

	public void Deselect() {
		selected = false;
		UpdateColor();
	}
}
