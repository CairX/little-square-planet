using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour {
	public float Min;
	public float Max;
	public float Step;

	private void Update () {
		if (Input.GetButtonDown("Zoom Reset")) {
			transform.localScale = Vector3.one;
		} else if (Input.GetButtonDown("Zoom In")) {
			// Using only one of the x,y values because we want a square zoom.
			var step = Mathf.Min(Min, transform.localScale.x + Step);
			transform.localScale = new Vector3(step, step, transform.localScale.z);
		} else if (Input.GetButtonDown("Zoom Out")) {
			// Using only one of the x,y values because we want a square zoom.
			var step = Mathf.Max(Max, transform.localScale.x - Step);
			transform.localScale = new Vector3(step, step, transform.localScale.z);
		}
	}
}
