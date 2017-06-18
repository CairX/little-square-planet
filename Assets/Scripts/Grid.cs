using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
	public GameObject tile;

	GameObject[,] grid;
	int width = 5;
	int height = 5;

	void Start() {
		grid = new GameObject[width, height];

		for(int x = 0; x < grid.GetLength(0); x++) {
			for(int y = 0; y < grid.GetLength(1); y++) {
				var t = Instantiate(tile, transform);

				var cartX = x * 2.56f;
				var cartY = y * 2.56f;

				var isoX = (cartX - cartY) * 0.5f;
				var isoY = (cartX + cartY) * 0.25f;
				var isoZ = y;

				t.transform.localPosition = new Vector3(isoX, -isoY, 0);
				t.GetComponent<SpriteRenderer>().color = new Color(0.2f * x, 0.2f * y, 0.2f * x);
				t.GetComponent<SpriteRenderer>().sortingOrder = isoZ;
				grid[x, y] = t;
			}
		}
	}

	void Update() {

	}
}
