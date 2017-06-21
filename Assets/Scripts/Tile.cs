using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	public Vector3 IsoPosition {
		get {
			return new Vector3(
				transform.position.x,
				transform.position.y,
				GetComponent<SpriteRenderer>().sortingOrder
			);
		}
		set {
			transform.localPosition = new Vector3(value.x, value.y, 0);
			GetComponent<SpriteRenderer>().sortingOrder = (int)value.z;
		}
	}
}
