using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIActions : MonoBehaviour {
	public Text Description;
	public Text Secondary;

	private Earth _tile;
	
	private void OnEnable() {
		Grid.OnTileSelect += Information;
	}
	
	private void OnDisable() {
		Grid.OnTileSelect -= Information;
	}

	private void Update() {
		Description.text = _tile.StateDescription();
		Secondary.text = _tile.StateSecondary();
	}

	private void Information(GameObject tile) {
		_tile = tile.GetComponent<Earth>();
	}
}
