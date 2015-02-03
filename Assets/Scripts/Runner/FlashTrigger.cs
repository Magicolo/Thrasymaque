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
		while (distance < 6) {
			sprite.color = new Color(Random.value, Random.value, Random.value, 1);
			distance = Vector3.Distance(sprite.transform.position, References.Runner.transform.position);
			yield return new WaitForSeconds(0);
		}
		
		sprite.color = initColor;
	}
}

