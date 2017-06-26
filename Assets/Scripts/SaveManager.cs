using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class SaveManager : MonoBehaviour {
	public GameObject Bank;
	public bool DebugLoad;

	private string _path = "Saves/test.xml";
	
	private void Awake() {
		_path = Path.Combine(Application.dataPath, _path);
		if (DebugLoad && File.Exists(_path)) Load();
	}

	private void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha0)) Save();
	}

	private void Load() {
		var xml = new XmlDocument();
		xml.LoadXml(File.ReadAllText(_path));
		Bank.GetComponent<Bank>().Load(xml.SelectSingleNode("root/Bank/Bank"));
	}

	private void Save() {
		var xml = new XmlDocument();
		xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", null));

		var root = xml.CreateElement("root");
		xml.AppendChild(root);

		var bankXml = XmlUtil.FromGameObject(xml, Bank);
		root.AppendChild(bankXml);

		xml.Save(_path);
	}
}

public class XmlUtil {
	public static XmlElement CreateByName(XmlDocument xml, string key, string value) {
		var element = xml.CreateElement(key);
		element.InnerText = value;
		return element;
	}

	public static XmlElement FromGameObject(XmlDocument xml, GameObject gameObject) {
		var element = xml.CreateElement(gameObject.name);
		foreach (var savable in gameObject.GetComponents<ISave>()) {
			element.AppendChild(savable.Save(xml));
		}
		return element;
	}
}
