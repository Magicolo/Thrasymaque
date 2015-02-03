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
		SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
		
		if (particles != null) {
			particles.Play();
		}
		
		while (sprite.color.a > 0) {
			sprite.SetColor(Color.Lerp(sprite.color, new Color(), 25 * Time.deltaTime), "A");
			yield return new WaitForSeconds(0);
		}
		
		if (particles != null) {
			while (particles.isPlaying) {
				yield return new WaitForSeconds(0);
			}
		}
		
		obj.Remove();
	}
}

