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
		float distance = Vector2.Distance(sprite.transform.position, References.Runner.transform.position);
		yield return new WaitForSeconds(0);
	}
}

