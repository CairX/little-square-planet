using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour {
	public float time;
	float seedTime;
	float growthTime;

	public Sprite seedSprite;
	public Sprite growthSprite;
	public Sprite plantSprite;

	new SpriteRenderer renderer;
	bool selected = false;
	bool growing = false;
	float timer = 0;

	void Start() {
		renderer = GetComponent<SpriteRenderer>();
		seedTime = time * 0.3f;
		growthTime = time - seedTime;

		transform.Find("text").GetComponent<MeshRenderer>().sortingLayerName = "Plant";
		transform.Find("text").GetComponent<MeshRenderer>().sortingOrder = 1;

		StartCoroutine(Grow());
	}

	void Update() {
		if (growing) {
			timer += Time.deltaTime;
			transform.Find("text").GetComponent<TextMesh>().text = (Mathf.Max(0, time - timer)).ToString("F1");
		}
	}

	IEnumerator Grow() {
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
