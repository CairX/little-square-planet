using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

		var xml = Xml.NewDocument();
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
		var xml = Xml.NewDocument();
		xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", null));

		var root = Xml.Element("Save");
		xml.AppendChild(root);
		
		foreach (var item in Items) {
			var itemXml = Xml.CreateFromGameObject(item);
			root.AppendChild(itemXml);
		}

		xml.Save(_path);
		Debug.Log("Game Saved");
	}
}