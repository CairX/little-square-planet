using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;

public class Grid : MonoBehaviour, ISave {
	public GameObject Tile;
	[Range(1, 50)]
	public int Width = 1;
	[Range(1, 50)]
	public int Height = 1;
	[Range(1, 50)]
	public int Depth = 1;

	[Space(16)]
	public float TileWidth;
	public float TileHeight;

	[Space(16)]
	public float TileWidthRatio;
	public float TileHeightRatio;

	private GameObject[,,] _tiles;
	private Vector3 _center;
	private Vector3 _selected;
	private bool _loaded;


	private Vector3 GetCoordinates(Vector3 grid) {
		return new Vector3(
			(grid.x - (_center.x - 0.5f)) * TileWidth,
			(grid.y - (_center.y - 0.5f)) * TileHeight,
			grid.z
		);
	}

	private Vector3 CartToIso(Vector3 grid, Vector3 cart) {
		return new Vector3(
			(cart.x - cart.y) * TileWidthRatio,
			-((cart.x + cart.y) * TileHeightRatio + (grid.z * TileHeight * 0.5f)),
			grid.y - grid.z + grid.x
		);
	}

	private void Start() {
		_tiles = new GameObject[Width, Height, Depth];
		_center = new Vector3(Mathf.Floor(Width * 0.5f), Mathf.Floor(Height * 0.5f), 0);
		if (!_loaded) {
			_selected = _center;
		}

		for (var x = 0; x < _tiles.GetLength(0); x++) {
			for (var y = 0; y < _tiles.GetLength(1); y++) {
				for (var z = 0; z < _tiles.GetLength(2); z++) {
					var tileObject = Instantiate(Tile, transform);
					var tileScript = tileObject.GetComponent<Earth>();

					var grid = new Vector3(x, y, z);
					var cart = GetCoordinates(grid);
					var iso = CartToIso(grid, cart);

					tileObject.GetComponent<Tile>().IsoPosition = iso;

					tileObject.transform.Find("text").GetComponent<TextMesh>().text = x + "," + y;
					tileObject.transform.Find("text").GetComponent<MeshRenderer>().sortingOrder = (int)iso.z;

					if (z == 0) {
						var gridG = grid + new Vector3(0, 0, -1);
						var cartG = GetCoordinates(gridG);
						var isoG = CartToIso(gridG, cartG);
						tileScript.InitGrass(isoG);
					}

					_tiles[x, y, z] = tileObject;
				}
			}
		}

		_tiles[(int)_selected.x, (int)_selected.y, (int)_selected.z].GetComponent<Earth>().Select();
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
			var grid = _selected + new Vector3(0, 0, -1);
			var cart = GetCoordinates(grid);
			var iso = CartToIso(grid, cart);
			_tiles[(int)_selected.x, (int)_selected.y, (int)_selected.z].GetComponent<Earth>().PerformAction(iso);
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
		Debug.Log(GetType().Name);
		var element = xml.CreateElement(GetType().Name);
		element.AppendChild(XmlUtil.CreateByName(xml, "Width", Width.ToString()));
		element.AppendChild(XmlUtil.CreateByName(xml, "Height", Width.ToString()));
		element.AppendChild(XmlUtil.CreateByName(xml, "Depth", Width.ToString()));
		
		var SelectedXml = xml.CreateElement("Selected");
		SelectedXml.AppendChild(XmlUtil.CreateByName(xml, "x", _selected.x.ToString()));
		SelectedXml.AppendChild(XmlUtil.CreateByName(xml, "y", _selected.y.ToString()));
		SelectedXml.AppendChild(XmlUtil.CreateByName(xml, "z", _selected.z.ToString()));
		element.AppendChild(SelectedXml);
		return element;
	}

	public void Load(XmlNode data) {
		var SelectedXml = data.SelectSingleNode("Selected");
		_selected = new Vector3(
			float.Parse(SelectedXml.SelectSingleNode("x").InnerText),
			float.Parse(SelectedXml.SelectSingleNode("y").InnerText),
			float.Parse(SelectedXml.SelectSingleNode("z").InnerText)
		);
		_loaded = true;
	}
}
