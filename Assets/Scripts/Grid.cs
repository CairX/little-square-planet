using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
	public GameObject tile;
	public int width = 1;
	public int height = 1;

	GameObject[,] grid;

	int selectedX = 0;
	int selectedY = 0;

	void Start() {
		grid = new GameObject[width, height];

		selectedX = Mathf.FloorToInt(width * 0.5f);
		selectedY = Mathf.FloorToInt(height * 0.5f);

		for (int x = 0; x < grid.GetLength(0); x++) {
			for (int y = 0; y < grid.GetLength(1); y++) {
				var t = Instantiate(tile, transform);

				var cartX = x * 2.56f;
				var cartY = y * 2.56f;

				var isoX = (cartX - cartY) * 0.5f;
				var isoY = (cartX + cartY) * 0.25f;
				var isoZ = y;

				t.transform.localPosition = new Vector3(isoX, -isoY, 0);
				t.GetComponent<SpriteRenderer>().sortingOrder = isoZ;
				grid[x, y] = t;

				t.transform.Find("text").GetComponent<TextMesh>().text = x + "," + y;
				t.transform.Find("text").GetComponent<MeshRenderer>().sortingOrder = isoZ;
			}
		}

		grid[selectedX, selectedY].GetComponent<SpriteRenderer>().color = new Color(0, 1, 0);
	}

	void Update() {
		Vector2 map;
		if (Input.GetButton("Grid Alt")) {
			map = GetKeyMapDiagonal();
		}
		else {
			map = GetKeyMapSquareCounter();
		}
		if (map != Vector2.zero) {
			grid[selectedX, selectedY].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
			selectedX += (int)map.x;
			selectedY += (int)map.y;
			grid[selectedX, selectedY].GetComponent<SpriteRenderer>().color = new Color(0, 1, 0);
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
