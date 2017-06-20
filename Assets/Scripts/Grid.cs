using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
	public GameObject tile;
	[Range(1, 50)]
	public int width = 1;
	[Range(1, 50)]
	public int height = 1;

	Tile[,] tiles;
	Vector2 selected;

	void Start() {
		tiles = new Tile[width, height];
		selected = new Vector2(Mathf.Floor(width * 0.5f), Mathf.Floor(height * 0.5f));

		for (int x = 0; x < tiles.GetLength(0); x++) {
			for (int y = 0; y < tiles.GetLength(1); y++) {
				var tileObject = Instantiate(tile, transform);
				var tileScript = tileObject.GetComponent<Tile>();

				var cartX = (x - (selected.x - 0.5f)) * tileScript.width;
				var cartY = (y - (selected.y - 0.5f)) * tileScript.height;

				var isoX = (cartX - cartY) * tileScript.widthRatio;
				var isoY = (cartX + cartY) * tileScript.heightRatio;
				var isoZ = y;

				tileObject.transform.localPosition = new Vector3(isoX, -isoY, 0);
				tileObject.GetComponent<SpriteRenderer>().sortingOrder = isoZ;

				tileObject.transform.Find("text").GetComponent<TextMesh>().text = x + "," + y;
				tileObject.transform.Find("text").GetComponent<MeshRenderer>().sortingOrder = isoZ;

				tiles[x, y] = tileScript;
			}
		}

		tiles[(int)selected.x, (int)selected.y].Select();
	}

	void Update() {
		Vector2 map;
		if (Input.GetButton("Grid Alt")) {
			map = GetKeyMapDiagonal();
		}
		else {
			map = GetKeyMapSquareCounter();
		}
		var next = selected + map;

		if (map != Vector2.zero &&
			next.x >= 0 && next.x < tiles.GetLength(0) &&
			next.y >= 0 && next.y < tiles.GetLength(1)) {
			tiles[(int)selected.x, (int)selected.y].GetComponent<Tile>().Deselect();
			tiles[(int)next.x, (int)next.y].GetComponent<Tile>().Select();
			selected = next;

		}
	}

	static Vector2 GetKeyMapDiagonal() {
		if (Input.GetButtonDown("Grid Up")) {
			Debug.Log("Up");
			return new Vector2(-1, -1);
		}
		if (Input.GetButtonDown("Grid Right")) {
			Debug.Log("Right");
			return new Vector2(1, -1);
		}
		if (Input.GetButtonDown("Grid Down")) {
			Debug.Log("Down");
			return new Vector2(1, 1);
		}
		if (Input.GetButtonDown("Grid Left")) {
			Debug.Log("Left");
			return new Vector2(-1, 1);
		}
		return Vector2.zero;
	}

	static Vector2 GetKeyMapSquare() {
		if (Input.GetButtonDown("Grid Up")) {
			Debug.Log("Up");
			return new Vector2(0, -1);
		}
		if (Input.GetButtonDown("Grid Right")) {
			Debug.Log("Right");
			return new Vector2(1, 0);
		}
		if (Input.GetButtonDown("Grid Down")) {
			Debug.Log("Down");
			return new Vector2(0, 1);
		}
		if (Input.GetButtonDown("Grid Left")) {
			Debug.Log("Left");
			return new Vector2(-1, 0);
		}
		return Vector2.zero;
	}

	static Vector2 GetKeyMapSquareCounter() {
		if (Input.GetButtonDown("Grid Up")) {
			Debug.Log("Up");
			return new Vector2(-1, 0);
		}
		if (Input.GetButtonDown("Grid Right")) {
			Debug.Log("Right");
			return new Vector2(0, -1);
		}
		if (Input.GetButtonDown("Grid Down")) {
			Debug.Log("Down");
			return new Vector2(1, 0);
		}
		if (Input.GetButtonDown("Grid Left")) {
			Debug.Log("Left");
			return new Vector2(0, 1);
		}
		return Vector2.zero;
	}
}
