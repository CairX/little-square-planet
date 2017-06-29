using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class Bank : MonoBehaviour, ISave {
	private static Bank Instance;
	
	public int Seeds;
	public Text Text;

	private GameObject[] _plants;
	private int _selected;

	private void Awake() {
		Instance = this;
		_plants = Resources.LoadAll<GameObject>("Plants");
	}

	private void Start() {
		UpdateInfo();
	}

	private void OnEnable() {
		Plant.OnPlant += RemoveSeeds;
		Plant.OnHarvest += AddSeeds;
	}

	private void OnDisable() {
		Plant.OnPlant += RemoveSeeds;
		Plant.OnHarvest += AddSeeds;
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			_selected = 0;
		} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			_selected = 1;
		}
	}

	private void AddSeeds(int seeds) {
		Seeds += seeds;
		UpdateInfo();
	}
	
	private void RemoveSeeds(int seeds) {
		Seeds -= seeds;
		UpdateInfo();
	}

	private void UpdateInfo() {
		Text.text = Seeds.ToString();
	}

	public static bool HasSeeds(int seeds) {
		return Instance.Seeds - seeds >= 0;
	}

	public static bool CanBuySelected() {
		return HasSeeds(Instance._plants[Instance._selected].GetComponent<Plant>().Cost);
	}

	public static GameObject Selected() {
		return Instance._plants[Instance._selected];
	}

	public XmlNode Save() {
		var element = Xml.Element(this);
		element.AppendChild(Xml.Element("Seeds", Seeds));
		return element;
	}

	public void Load(XmlNode data) {
		Seeds = Xml.Int(data.SelectSingleNode("Seeds"));
	}
}
