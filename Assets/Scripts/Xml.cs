using System.Globalization;
using System.Xml;
using UnityEngine;

public class Xml {
	private static XmlDocument _document;
	public static XmlDocument NewDocument() {
		_document = new XmlDocument();
		return _document;
	}
	
	// Empty Element
	public static XmlElement Element(string key) {
		return _document.CreateElement(key);
	}
	
	// String
	public static XmlElement Element(string key, string value) {
		var element = _document.CreateElement(key);
		element.InnerText = value;
		return element;
	}
	
	// Float
	public static XmlElement Element(string key, float value) {
		return Element(key, value.ToString(CultureInfo.InvariantCulture));
	}
	
	public static float Float(XmlNode node) {
		return float.Parse(node.InnerText);
	}
	
	// Integer
	public static XmlElement Element(string key, int value) {
		return Element(key, value.ToString(CultureInfo.InvariantCulture));
	}

	public static int Int(XmlNode node) {
		return int.Parse(node.InnerText);
	}
	
	// Boolean
	public static XmlElement Element(string key, bool value) {
		return Element(key, value ? "true" : "false");
	}
	
	// Vector3
	public static XmlElement Element(string key, Vector3 value) {
		var element = _document.CreateElement(key);
		element.SetAttribute("x", value.x.ToString(CultureInfo.InvariantCulture));
		element.SetAttribute("y", value.y.ToString(CultureInfo.InvariantCulture));
		element.SetAttribute("z", value.z.ToString(CultureInfo.InvariantCulture));
		return element;
	}
	
	public static Vector3 Vector3(XmlNode node) {
		if (node == null) return UnityEngine.Vector3.zero;
		if (node.Attributes == null) return UnityEngine.Vector3.zero;

		return new Vector3(
			float.Parse(node.Attributes["x"].InnerText),
			float.Parse(node.Attributes["y"].InnerText),
			float.Parse(node.Attributes["z"].InnerText)
		);
	}

	// GameObject
	public static XmlElement CreateFromGameObject(GameObject gameObject) {
		var element = _document.CreateElement(gameObject.name.Replace("(Clone)", ""));
		foreach (var savable in gameObject.GetComponents<ISave>()) {
			element.AppendChild(savable.Save());
		}
		return element;
	}

	public static void LoadComponents(GameObject gameObject, XmlNode node) {
		for (var j = 0; j < node.ChildNodes.Count; j++) {
			var componentElement = node.ChildNodes[j];
			var component = gameObject.GetComponent(componentElement.Name);
			((ISave)component).Load(componentElement);
		}
	}
	
	// Component
	public static XmlElement Element(Component component) {
		return _document.CreateElement(component.GetType().Name);
	}
}