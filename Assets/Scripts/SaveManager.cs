using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;

public class SaveManager : MonoBehaviour {
	public GameObject[] Items;
	public bool DebugLoad;

	private string _path = "Saves/test.xml";
	
	private void Awake() {
		_path = Path.Combine(Application.dataPath, _path);
		if (DebugLoad && File.Exists(_path)) Load();
	}

	private void Update () {
		if (Input.GetButtonDown("Save")) Save();
	}

	private void Load() {
		var notLoaded = Items.ToDictionary(item => item.name);

		var xml = new XmlDocument();
		xml.LoadXml(File.ReadAllText(_path));
		var root = xml.SelectSingleNode("Save");

		for (var i = 0; i < root.ChildNodes.Count; i++) {
			var itemElement = root.ChildNodes[i];
			var itemGameObject = notLoaded[itemElement.Name];

			for (var j = 0; j < itemElement.ChildNodes.Count; j++) {
				var componentElement = itemElement.ChildNodes[j];
				var component = itemGameObject.GetComponent(componentElement.Name);
				((ISave)component).Load(componentElement);
			}

			notLoaded.Remove(itemElement.Name);
		}
		Debug.Log("Game Loaded");
	}

	private void Save() {
		var xml = new XmlDocument();
		xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", null));

		var root = xml.CreateElement("Save");
		xml.AppendChild(root);
		
		foreach (var item in Items) {
			var itemXml = XmlUtil.CreateFromGameObject(xml, item);
			root.AppendChild(itemXml);
		}

		xml.Save(_path);
		Debug.Log("Game Saved");
	}
}

public class XmlUtil {
	public static XmlElement CreateFromName(XmlDocument xml, string key, string value) {
		var element = xml.CreateElement(key);
		element.InnerText = value;
		return element;
	}
	
	public static XmlElement CreateFromName(XmlDocument xml, string key, float value) {
		return CreateFromName(xml, key, value.ToString());
	}
	
	public static XmlElement CreateFromName(XmlDocument xml, string key, int value) {
		return CreateFromName(xml, key, value.ToString());
	}
	
	public static XmlElement CreateFromName(XmlDocument xml, string key, bool value) {
		return CreateFromName(xml, key, value ? "true" : "false");
	}
	
	public static XmlElement CreateFromName(XmlDocument xml, string key, Vector3 value) {
		var element = xml.CreateElement(key);
		element.SetAttribute("x", value.x.ToString());
		element.SetAttribute("y", value.y.ToString());
		element.SetAttribute("z", value.z.ToString());
		return element;
	}

	public static XmlElement CreateFromGameObject(XmlDocument xml, GameObject gameObject) {
		var element = xml.CreateElement(gameObject.name.Replace("(Clone)", ""));
		foreach (var savable in gameObject.GetComponents<ISave>()) {
			element.AppendChild(savable.Save(xml));
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
}
