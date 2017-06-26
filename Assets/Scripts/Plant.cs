using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour {

	public float Time;
	private float _seedTime;
	private float _growthTime;

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
		_seedTime = Time * 0.3f;
		_growthTime = Time - _seedTime;

		transform.Find("text").GetComponent<MeshRenderer>().sortingLayerName = "Plant";
		transform.Find("text").GetComponent<MeshRenderer>().sortingOrder = 1;

		StartCoroutine(Grow());
	}

	private void Update() {
		if (_growing) {
			_timer += UnityEngine.Time.deltaTime;
			transform.Find("text").GetComponent<TextMesh>().text = Mathf.Max(0, Time - _timer).ToString("F1");
		}
	}

	private IEnumerator Grow() {
		_growing = true;
		_renderer.sprite = SeedSprite;
		yield return new WaitForSeconds(_seedTime);
		_renderer.sprite = GrowthSprite;
		yield return new WaitForSeconds(_growthTime);
		_renderer.sprite = PlantSprite;
		_growing = false;
	}

	public bool IsDone() {
		return !_growing;
	}
}
