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
	Plant plant;

	void Start() {
		renderer = GetComponent<SpriteRenderer>();
		standardColor = renderer.color;
		UpdateColor();
		transform.Find("grass").GetComponent<SpriteRenderer>().sortingOrder = renderer.sortingOrder;
	}

	void Update() {
		if (selected && Input.GetButtonDown("Tile Action")) {
			if (plant == null) {
				var template = (GameObject)Resources.Load("plant");
				plant = Instantiate<Plant>(template.GetComponent<Plant>(), transform);
				plant.GetComponent<SpriteRenderer>().sortingOrder = renderer.sortingOrder;
				transform.Find("grass").gameObject.SetActive(false);
				if (selected) {
					plant.Select();
				}
			}
			else if (plant.IsDone()) {
				Destroy(plant.gameObject);
				transform.Find("grass").gameObject.SetActive(true);
			}
		}
	}

	void UpdateColor() {
		if (renderer) {
			renderer.color = selected ? selectedColor : standardColor;
			transform.Find("grass").GetComponent<SpriteRenderer>().color = renderer.color;
		}
	}

	public void Select() {
		selected = true;
		UpdateColor();
		if (plant != null) { plant.Select(); }
	}

	public void Deselect() {
		selected = false;
		UpdateColor();
		if (plant != null) { plant.Deselect(); }
	}
}
