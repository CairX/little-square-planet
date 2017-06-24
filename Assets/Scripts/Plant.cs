using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour {

	public float time;
	private float seedTime;
	private float growthTime;

	public int cost;
	public int worth;

	public Sprite seedSprite;
	public Sprite growthSprite;
	public Sprite plantSprite;

	private new SpriteRenderer renderer;
	private bool selected = false;
	private bool growing = false;
	private float timer;

	public delegate void Planted(int cost);
	public static event Planted OnPlant;

	public delegate void Harvested(int worth);

	public static event Harvested OnHarvest;

	private void Awake() {
		OnPlant(cost);
	}

	private void OnDestroy() {
		OnHarvest(worth);
	}

	private void Start() {
		renderer = GetComponent<SpriteRenderer>();
		seedTime = time * 0.3f;
		growthTime = time - seedTime;

		transform.Find("text").GetComponent<MeshRenderer>().sortingLayerName = "Plant";
		transform.Find("text").GetComponent<MeshRenderer>().sortingOrder = 1;

		StartCoroutine(Grow());
	}

	private void Update() {
		if (growing) {
			timer += Time.deltaTime;
			transform.Find("text").GetComponent<TextMesh>().text = (Mathf.Max(0, time - timer)).ToString("F1");
		}
	}

	private IEnumerator Grow() {
		growing = true;
		renderer.sprite = seedSprite;
		yield return new WaitForSeconds(seedTime);
		renderer.sprite = growthSprite;
		yield return new WaitForSeconds(growthTime);
		renderer.sprite = plantSprite;
		growing = false;
	}

	public bool IsDone() {
		return !growing;
	}
}
