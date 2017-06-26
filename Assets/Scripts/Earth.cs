using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour {
	public Color SelectedColor;
	private Color _standardColor;

	private SpriteRenderer _renderer;
	private bool _selected;
	private GameObject _plant;
	public GameObject Grass;

	private void Awake() {
		_renderer = GetComponent<SpriteRenderer>();
		_standardColor = _renderer.color;
	}

	private void Plant(Vector3 isoPosition) {
		var template = (GameObject)Resources.Load("plant");
		if (!Bank.HasSeeds(template.GetComponent<Plant>().Cost)) {
			return;
		}
		_plant = Instantiate(template, transform.parent);
		_plant.GetComponent<Tile>().IsoPosition = isoPosition;
		Grass.SetActive(false);
		UpdateColor();
	}

	private void Harvest() {
		Destroy(_plant);
		Grass.SetActive(true);
	}

	private void UpdateColor() {
		_renderer.color = _selected ? SelectedColor : _standardColor;
		Grass.GetComponent<SpriteRenderer>().color = _renderer.color;
		if (_plant != null) {
			_plant.GetComponent<SpriteRenderer>().color = _renderer.color;
		}
	}
	
	public void InitGrass(Vector3 isoPosition) {
		var template = (GameObject)Resources.Load("grass");
		Grass = Instantiate(template, transform.parent);
		Grass.GetComponent<Tile>().IsoPosition = isoPosition;
		UpdateColor();
	}

	public void PerformAction(Vector3 isoPosition) {
		if (_plant == null) {
			Plant(isoPosition);
		}
		else if (_plant.GetComponent<Plant>().IsDone()) {
			Harvest();
		}
	}

	public void Select() {
		_selected = true;
		UpdateColor();
	}

	public void Deselect() {
		_selected = false;
		UpdateColor();
	}
}
