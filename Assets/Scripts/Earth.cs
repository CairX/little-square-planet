using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour {
	public Color selectedColor;
	Color standardColor;

	new SpriteRenderer renderer;
	bool selected = false;
	GameObject plant;
	public GameObject grass;

	void Awake() {
		renderer = GetComponent<SpriteRenderer>();
		standardColor = renderer.color;
	}

	public void Grass(Vector3 isoPosition) {
		var template = (GameObject)Resources.Load("grass");
		grass = Instantiate(template, transform.parent);
		grass.GetComponent<Tile>().IsoPosition = isoPosition;
		UpdateColor();
	}

	public void PerformAction(Vector3 isoPosition) {
		if (plant == null) {
			Plant(isoPosition);
		}
		else if (plant.GetComponent<Plant>().IsDone()) {
			Harvest();
		}
	}

	void Plant(Vector3 isoPosition) {
		var template = (GameObject)Resources.Load("plant");
		if (!Bank.HasSeeds(template.GetComponent<Plant>().cost)) {
			return;
		}
		plant = Instantiate(template, transform.parent);
		plant.GetComponent<Tile>().IsoPosition = isoPosition;
		grass.SetActive(false);
		UpdateColor();
	}

	void Harvest() {
		Destroy(plant);
		grass.SetActive(true);
	}

	void UpdateColor() {
		renderer.color = selected ? selectedColor : standardColor;
		grass.GetComponent<SpriteRenderer>().color = renderer.color;
		if (plant != null) {
			plant.GetComponent<SpriteRenderer>().color = renderer.color;
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
