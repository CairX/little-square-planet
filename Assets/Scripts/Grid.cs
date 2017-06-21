using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Grid : MonoBehaviour {
	public GameObject tile;
	[Range(1, 50)]
	public int width = 1;
	[Range(1, 50)]
	public int height = 1;
	[Range(1, 50)]
	public int depth = 1;

	[Space(16)]
	public float tileWidth;
	public float tileHeight;

	[Space(16)]
	public float tileWidthRatio;
	public float tileHeightRatio;

	GameObject[,,] tiles;
	Vector3 center;
	Vector3 selected;


	Vector3 GetCoordinates(Vector3 grid) {
		return new Vector3(
			(grid.x - (center.x - 0.5f)) * tileWidth,
			(grid.y - (center.y - 0.5f)) * tileHeight,
			grid.z
		);
	}

	Vector3 CartToIso(Vector3 grid, Vector3 cart) {
		return new Vector3(
			(cart.x - cart.y) * tileWidthRatio,
			-((cart.x + cart.y) * tileHeightRatio + (grid.z * tileHeight * 0.5f)),
			grid.y - grid.z + grid.x
		);
	}

	void Start() {
		tiles = new GameObject[width, height, depth];
		center = new Vector3(Mathf.Floor(width * 0.5f), Mathf.Floor(height * 0.5f), 0);
		selected = center;

		for (int x = 0; x < tiles.GetLength(0); x++) {
			for (int y = 0; y < tiles.GetLength(1); y++) {
				for (int z = 0; z < tiles.GetLength(2); z++) {
					var tileObject = Instantiate(tile, transform);
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
						tileScript.Grass(isoG);
					}

					tiles[x, y, z] = tileObject;
				}
			}
		}

		tiles[(int)selected.x, (int)selected.y, (int)selected.z].GetComponent<Earth>().Select();
	}

	void Update() {
		Vector3 map;
		if (Input.GetButton("Grid Alt")) {
			map = GetKeyMapDiagonal();
		}
		else {
			map = GetKeyMapSquareCounter();
		}
		var next = selected + map;
		if (map != Vector3.zero &&
			next.x >= 0 && next.x < tiles.GetLength(0) &&
			next.y >= 0 && next.y < tiles.GetLength(1)) {
			tiles[(int)selected.x, (int)selected.y, (int)selected.z].GetComponent<Earth>().Deselect();
			tiles[(int)next.x, (int)next.y, (int)next.z].GetComponent<Earth>().Select();
			selected = next;
		}

		if (Input.GetButtonDown("Tile Action")) {
			var grid = selected + new Vector3(0, 0, -1);
			var cart = GetCoordinates(grid);
			var iso = CartToIso(grid, cart);
			tiles[(int)selected.x, (int)selected.y, (int)selected.z].GetComponent<Earth>().PerformAction(iso);
		}
	}

	static Vector3 GetKeyMapDiagonal() {
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

	static Vector3 GetKeyMapSquare() {
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

	static Vector3 GetKeyMapSquareCounter() {
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
}
