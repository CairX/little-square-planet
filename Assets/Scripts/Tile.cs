using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Tile : MonoBehaviour, ISave {
	private const float Width = 2.56f;
	private const float Height = 2.56f;

	public const float WidthRatio = 0.5f;
	public const float HeightRatio = 0.25f;

	private static Vector3 ToWorldPosition(Vector3 grid) {
		return new Vector3(
			grid.x * Width,
			grid.y * Height,
			grid.z
		);
	}

	// TODO Make private
	public static Vector3 ToIsometricPosition(Vector3 position) {
		var world = ToWorldPosition(position);
		return new Vector3(
			(world.x - world.y) * WidthRatio,
			-((world.x + world.y) * HeightRatio + (position.z * Height * 0.5f)),
			position.y - position.z + position.x
		);
	}
	
	private Vector3 _position = Vector3.zero;
	public Vector3 Position {
		get { return _position; }
		set {
			_position = value;
			var isometric = ToIsometricPosition(value);
			transform.localPosition = new Vector3(isometric.x, isometric.y, 0);
			GetComponent<SpriteRenderer>().sortingOrder = (int)isometric.z;
		}
	}


	private void Start() {
//		transform.Find("text").GetComponent<TextMesh>().text = _position.x + "," + _position.y;
//		transform.Find("text").GetComponent<MeshRenderer>().sortingOrder = (int)_position.z;
	}

	public XmlNode Save() {
		var element = Xml.Element(this);
		element.AppendChild(Xml.Element("Position", Position));
		return element;
	}

	public void Load(XmlNode data) {
		Position = Xml.Vector3(data.SelectSingleNode("Position"));
	}
}
