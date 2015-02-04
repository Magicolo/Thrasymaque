using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class FlashTrigger : MonoBehaviourExtended {

	void Update() {
//		const float distance = 50;
//		Vector2 direction = transform.right;
//		Vector2 size = new Vector2(70, 70);
//		Vector2 center = transform.position - (Vector3)(direction * distance);
//		LayerMask layerMask = new LayerMask().AddToMask("Destroyed").Inverse();
//		RaycastHit2D[] hits = Physics2D.BoxCastAll(center, size, 0, direction, distance, layerMask);
//		
//		foreach (RaycastHit2D hit in hits) {
//			SpriteRenderer sprite = hit.collider.GetComponentInChildren<SpriteRenderer>();
//			
//			if (sprite != null) {
//				StartCoroutine(Flash(sprite));
//			}
//		}
	}
	
	void OnTriggerEnter2D(Collider2D collision) {
		SpriteRenderer sprite = collision.gameObject.GetComponentInChildren<SpriteRenderer>();
		
		if (sprite != null && sprite.gameObject.layer != 19) {
			StartCoroutine(Flash(sprite));
		}
	}
	
	IEnumerator Flash(SpriteRenderer sprite) {
		Color initColor = sprite.color;
		float distance = Vector3.Distance(sprite.transform.position, References.Runner.transform.position);
		
		while (sprite != null && sprite.gameObject.layer != 19 && distance < 15) {
			float brightness = 0.2F;
			float randomness = 1 - (distance / 40);
			float r = initColor.r + Random.Range(-initColor.r, initColor.r) * randomness + brightness;
			float g = initColor.g + Random.Range(-initColor.g, initColor.g) * randomness + brightness;
			float b = initColor.b + Random.Range(-initColor.b, initColor.b) * randomness + brightness;
			
			sprite.color = new Color(r, g, b, 1);
			distance = Vector3.Distance(sprite.transform.position, References.Runner.transform.position);
			yield return new WaitForSeconds(0);
		}
	}
}

