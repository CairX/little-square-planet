using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
	public float Speed;

	private void Update() {
		// Reverse becaues we are moving the camera and not an object.
		if (Input.GetButton("Camera Up")) {
			Move(Vector3.down);
		}
		else if (Input.GetButton("Camera Down")) {
			Move(Vector3.up);
		}
		if (Input.GetButton("Camera Left")) {
			Move(Vector3.right);
		} else if (Input.GetButton("Camera Right")) {
			Move(Vector3.left);
		}
	}

	private void Move(Vector3 direction) {
		transform.position = transform.position + direction * Speed * Time.deltaTime;
	}
}
