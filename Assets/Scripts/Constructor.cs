using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class Constructor : MonoBehaviourExtended {

	void OnTriggerEnter2D(Collider2D collision) {
		StartCoroutine(ActivateObject(collision.gameObject));
	}
	
	IEnumerator ActivateObject(GameObject obj) {
		foreach (GameObject child in obj.GetChildrenRecursive()) {
			child.SetActive(true);
		}
		
		ParticleSystem particles = obj.GetComponentInChildren<ParticleSystem>();
		SpriteRenderer sprite = obj.GetComponentInChildren<SpriteRenderer>();
		
		if (particles != null) {
			particles.Play();
		}
		
		if (sprite != null) {
			sprite.SetColor(0, "A");
		
			while (sprite.color.a < 1) {
				sprite.SetColor(sprite.color.a + 10 * Time.deltaTime, "A");
				yield return new WaitForSeconds(0);
			}
		}
	}
}

