using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Plant : MonoBehaviour, ISave {

	public float Time;

	public int Cost;
	public int Worth;

	public Sprite SeedSprite;
	public Sprite GrowthSprite;
	public Sprite PlantSprite;

	private SpriteRenderer _renderer;
	private bool _selected;
	private bool _growing;
	private float _timer;

	public delegate void Planted(int cost);
	public static event Planted OnPlant;

	public delegate void Harvested(int worth);
	public static event Harvested OnHarvest;

	private void Awake() {
		// TODO Can't be called from here because it takes money from the bank when loading a save.
		if (OnPlant != null) OnPlant(Cost);
	}

	private void OnDestroy() {
		// TODO Can't be called from here because it adds money to the bank when loading a save.
		if (OnHarvest != null) OnHarvest(Worth);
	}

	private void Start() {
		_renderer = GetComponent<SpriteRenderer>();

		transform.Find("text").GetComponent<MeshRenderer>().sortingLayerName = "Plant";
		transform.Find("text").GetComponent<MeshRenderer>().sortingOrder = 100;

		StartCoroutine(Grow());
	}

	private void Update() {
		if (!_growing) return;
		_timer += UnityEngine.Time.deltaTime;
		transform.Find("text").GetComponent<TextMesh>().text = Mathf.Max(0, Time - _timer).ToString("F1");
	}

	private IEnumerator Grow() {
		// Seed
		_growing = true;
		_renderer.sprite = SeedSprite;
		while (_timer < Time * 0.3f) {
			yield return null;
		}

		// Growing
		_renderer.sprite = GrowthSprite;
		while (_timer < Time) {
			yield return null;
		}

		// Done
		_renderer.sprite = PlantSprite;
		_growing = false;
	}

	public bool IsDone() {
		return !_growing;
	}

	public XmlNode Save() {
		var element = Xml.Element(this);
		element.AppendChild(Xml.Element("Timer", _timer));
		return element;
	}

	public void Load(XmlNode data) {
		_timer = Xml.Float(data.SelectSingleNode("Timer"));
	}
}
