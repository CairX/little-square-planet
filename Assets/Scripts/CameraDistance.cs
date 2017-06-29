using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDistance : MonoBehaviour {
	[Range(1, 100)]
	public int Min = 1;
	[Range(1, 100)]
	public int Max = 1;
	[Range(1, 100)]
	public int Steps = 1;
	[Range(0, 100)]
	public int DefaultStep;
	
	private float _min;
	private float _max;
	private float _interval;
	private float _default;

	private void Start() {
		_min = 1f / Min;
		_max = 1f / Max;
		_interval = (_min - _max) / Steps;
		_default = _min - DefaultStep * _interval;
		Scale(_default);
	}

	private void Update () {
		if (Input.GetButtonDown("Zoom Reset")) {
			Scale(_default);
		} else if (Input.GetButtonDown("Zoom In")) {
			// Using only one of the x,y values because we want a square zoom.
			Scale(Mathf.Clamp(transform.localScale.x + _interval, _max, _min));
		} else if (Input.GetButtonDown("Zoom Out")) {
			// Using only one of the x,y values because we want a square zoom.
			Scale(Mathf.Clamp(transform.localScale.x - _interval, _max, _min));
		}
	}

	private void Scale(float scale) {
		transform.localScale = new Vector3(scale, scale, transform.localScale.z);
	}
}
