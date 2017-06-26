﻿using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class Bank : MonoBehaviour, ISave {
	private static Bank Instance;
	
	public int Seeds;
	public Text Text;

	private void Awake() {
		Instance = this;
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

	public XmlNode Save(XmlDocument xml) {
		var element = xml.CreateElement(GetType().Name);
		element.AppendChild(XmlUtil.CreateFromName(xml, "Seeds", Seeds));
		return element;
	}

	public void Load(XmlNode data) {
		Seeds = int.Parse(data.SelectSingleNode("Seeds").InnerText);
	}
}
