using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class Destructor : MonoBehaviourExtended {

	void OnTriggerEnter2D(Collider2D collision) {
		StartCoroutine(DestroyObject(collision.gameObject));
	}
	
	IEnumerator DestroyObject(GameObject obj) {
		ParticleSystem particles = obj.GetComponentInChildren<ParticleSystem>();
		SpriteRenderer sprite = obj.GetComponentInChildren<SpriteRenderer>();
		
		if (particles != null) {
			particles.Play();
		}
		
		if (sprite != null) {
			while (sprite.color.a > 0) {
				sprite.SetColor(sprite.color.a - 10 * Time.deltaTime, "A");
				yield return new WaitForSeconds(0);
			}
		}
		
		if (particles != null) {
			while (particles.isPlaying) {
				yield return new WaitForSeconds(0);
			}
		}
		
		obj.Remove();
	}
}

