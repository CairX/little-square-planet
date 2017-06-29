using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Grid : MonoBehaviour, ISave {
	public GameObject TileTemplate;
	[Range(1, 50)]
	public int Width = 1;
	[Range(1, 50)]
	public int Height = 1;
	[Range(1, 50)]
	public int Depth = 1;

	private GameObject[,,] _tiles;
	private Vector3 _center;
	private Vector3 _selected;
	private bool _loaded;

	private void Start() {
		_center = new Vector3(Mathf.Floor(Width * 0.5f), Mathf.Floor(Height * 0.5f), 0);
		var isoCenter = Tile.ToIsometricPosition(_center);
		Debug.Log(isoCenter);
		transform.position = new Vector3(isoCenter.x, -(isoCenter.y * 0.5f), transform.position.z);
		
		if (!_loaded) {
			_tiles = new GameObject[Width, Height, Depth];
			_selected = _center;

			for (var x = 0; x < _tiles.GetLength(0); x++) {
				for (var y = 0; y < _tiles.GetLength(1); y++) {
					for (var z = 0; z < _tiles.GetLength(2); z++) {
						var tile = Instantiate(TileTemplate, transform);
						tile.GetComponent<Tile>().Position = new Vector3(x, y, z);
						_tiles[x, y, z] = tile;
					}
				}
			}

			_tiles[(int) _selected.x, (int) _selected.y, (int) _selected.z].GetComponent<Earth>().Select();
		}
	}

	private void Update() {
		Vector3 map;
		if (Input.GetButton("Grid Alt")) {
			map = GetKeyMapDiagonal();
		}
		else {
			map = GetKeyMapSquareCounter();
		}
		var next = _selected + map;
		if (map != Vector3.zero &&
			next.x >= 0 && next.x < _tiles.GetLength(0) &&
			next.y >= 0 && next.y < _tiles.GetLength(1)) {
			_tiles[(int)_selected.x, (int)_selected.y, (int)_selected.z].GetComponent<Earth>().Deselect();
			_tiles[(int)next.x, (int)next.y, (int)next.z].GetComponent<Earth>().Select();
			_selected = next;
		}

		if (Input.GetButtonDown("Tile Action")) {
			_tiles[(int)_selected.x, (int)_selected.y, (int)_selected.z].GetComponent<Earth>().PerformAction();
		}
	}

	private static Vector3 GetKeyMapDiagonal() {
		if (Input.GetButtonDown("Grid Up")) {
			Debug.Log("Up");
			return new Vector3(-1, -1, 0);
		}
		if (Input.GetButtonDown("Grid Right")) {
			Debug.Log("Right");
			return new Vector3(1, -1, 0);
		}
		if (Input.GetButtonDown("Grid Down")) {
			Debug.Log("Down");
			return new Vector3(1, 1, 0);
		}
		if (Input.GetButtonDown("Grid Left")) {
			Debug.Log("Left");
			return new Vector3(-1, 1, 0);
		}
		return Vector3.zero;
	}

	private static Vector3 GetKeyMapSquare() {
		if (Input.GetButtonDown("Grid Up")) {
			Debug.Log("Up");
			return new Vector3(0, -1, 0);
		}
		if (Input.GetButtonDown("Grid Right")) {
			Debug.Log("Right");
			return new Vector3(1, 0, 0);
		}
		if (Input.GetButtonDown("Grid Down")) {
			Debug.Log("Down");
			return new Vector3(0, 1, 0);
		}
		if (Input.GetButtonDown("Grid Left")) {
			Debug.Log("Left");
			return new Vector3(-1, 0, 0);
		}
		return Vector3.zero;
	}

	private static Vector3 GetKeyMapSquareCounter() {
		if (Input.GetButtonDown("Grid Up")) {
			Debug.Log("Up");
			return new Vector3(-1, 0, 0);
		}
		if (Input.GetButtonDown("Grid Right")) {
			Debug.Log("Right");
			return new Vector3(0, -1, 0);
		}
		if (Input.GetButtonDown("Grid Down")) {
			Debug.Log("Down");
			return new Vector3(1, 0, 0);
		}
		if (Input.GetButtonDown("Grid Left")) {
			Debug.Log("Left");
			return new Vector3(0, 1, 0);
		}
		return Vector3.zero;
	}

	public XmlNode Save(XmlDocument xml) {
		var element = xml.CreateElement(GetType().Name);
		element.AppendChild(XmlUtil.CreateFromName(xml, "Width", Width));
		element.AppendChild(XmlUtil.CreateFromName(xml, "Height", Height));
		element.AppendChild(XmlUtil.CreateFromName(xml, "Depth", Depth));
		element.AppendChild(XmlUtil.CreateFromName(xml, "Selected", _selected));

		var tilesXml = xml.CreateElement("Tiles");
		element.AppendChild(tilesXml);

		for (var x = 0; x < _tiles.GetLength(0); x++) {
			for (var y = 0; y < _tiles.GetLength(1); y++) {
				for (var z = 0; z < _tiles.GetLength(2); z++) {
					var tile = _tiles[x, y, z];
					var tileXml = XmlUtil.CreateFromGameObject(xml, tile);
					tilesXml.AppendChild(tileXml);
				}
			}
		}

		return element;
	}

	public void Load(XmlNode data) {
		Width = int.Parse(data.SelectSingleNode("Width").InnerText);
		Height = int.Parse(data.SelectSingleNode("Height").InnerText);
		Depth = int.Parse(data.SelectSingleNode("Depth").InnerText);
		
		_tiles = new GameObject[Width, Height, Depth];
		
		var selectedXml = data.SelectSingleNode("Selected");
		_selected = new Vector3(
			float.Parse(selectedXml.Attributes["x"].InnerText),
			float.Parse(selectedXml.Attributes["y"].InnerText),
			float.Parse(selectedXml.Attributes["z"].InnerText)
		);

		var tilesXml = data.SelectSingleNode("Tiles").ChildNodes;
		Debug.Log(tilesXml.Count);
		for (var i = 0; i < tilesXml.Count; i++) {
			// TODO Might not be object of TileTemplate. Need to look for the correct template.
			var tile = Instantiate(TileTemplate, transform);
			XmlUtil.LoadComponents(tile, tilesXml[i]);
			var pos = tile.GetComponent<Tile>().Position;
			_tiles[(int) pos.x, (int) pos.y, (int) pos.z] = tile;
		}

		_tiles[(int) _selected.x, (int) _selected.y, (int) _selected.z].GetComponent<Earth>().Select();
		_loaded = true;
	}
}
