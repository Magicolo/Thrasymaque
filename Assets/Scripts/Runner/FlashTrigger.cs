using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class FlashTrigger : MonoBehaviourExtended {

	void OnTriggerEnter2D(Collider2D collision) {
		SpriteRenderer sprite = collision.gameObject.GetComponentInChildren<SpriteRenderer>();
		
		if (sprite != null) {
			StartCoroutine(Flash(sprite));
		}
	}
	
	IEnumerator Flash(SpriteRenderer sprite) {
		Color initColor = sprite.color;
		
		float distance = Vector3.Distance(sprite.transform.position, References.Runner.transform.position);
		while (sprite != null && distance < 15) {
			float brightness = 0.2F;
			float randomness = 1 - (distance / 40);
			float r = initColor.r + Random.Range(-initColor.r, initColor.r) * randomness + brightness;
			float g = initColor.g + Random.Range(-initColor.g, initColor.g) * randomness + brightness;
			float b = initColor.b + Random.Range(-initColor.b, initColor.b) * randomness + brightness;
			
			sprite.color = new Color(r, g, b, 1);
			distance = Vector3.Distance(sprite.transform.position, References.Runner.transform.position);
			yield return new WaitForSeconds(0);
		}
		
//		sprite.color = initColor;
	}
}

