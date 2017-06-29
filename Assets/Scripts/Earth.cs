using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Earth : MonoBehaviour, ISave {
	public Color SelectedColor;
	private Color _standardColor;

	private SpriteRenderer _renderer;
	private bool _selected;

	public GameObject GrassTemplate;
	private GameObject _grass;

	private GameObject _plant;

	private void Awake() {
		_renderer = GetComponent<SpriteRenderer>();
		_standardColor = _renderer.color;
	}

	private void Start() {
		if (GetComponent<Tile>().Position.z == 0) {
			_grass = Instantiate(GrassTemplate, transform.parent);
			_grass.GetComponent<Tile>().Position = GetComponent<Tile>().Position + new Vector3(0, 0, -1);
			if (_plant) _grass.SetActive(false);
			UpdateColor();
		}
	}

	private void Plant() {
		_plant = Instantiate(Bank.Selected(), transform.parent);
		_plant.GetComponent<Tile>().Position = GetComponent<Tile>().Position + new Vector3(0, 0, -1);
		if (_grass) _grass.SetActive(false);
		UpdateColor();
	}

	private void Harvest() {
		Destroy(_plant);
		_grass.SetActive(true);
	}

	private void UpdateColor() {
		_renderer.color = _selected ? SelectedColor : _standardColor;
		if (_grass) {
			_grass.GetComponent<SpriteRenderer>().color = _renderer.color;
		}
		if (_plant) {
			_plant.GetComponent<SpriteRenderer>().color = _renderer.color;
		}
	}

	public void PerformAction() {
		if (!_plant) {
			if (Bank.CanBuySelected()) {
				Plant();
			}
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

	public XmlNode Save() {
		var element = Xml.Element(this);
		if (_plant) {
			var plantXml = Xml.CreateFromGameObject(_plant);
			element.AppendChild(plantXml);
		}
		return element;
	}

	public void Load(XmlNode data) {
		var plantXml = data.SelectSingleNode("Plant");
		if (plantXml != null) {
			Plant();
			Xml.LoadComponents(_plant, plantXml);
		}
	}
}
