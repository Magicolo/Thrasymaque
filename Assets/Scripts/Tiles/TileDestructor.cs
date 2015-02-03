using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class TileDestructor : MonoBehaviourExtended {

	void OnTriggerEnter2D(Collider2D collision) {
		DestroyObject(collision.gameObject);
	}
	
	void DestroyObject(GameObject obj) {
		StartCoroutine(PlayDestructionParticleFX(obj));
		
		foreach (SpriteRenderer sprite in obj.GetComponentsInChildren<SpriteRenderer>()) {
			StartCoroutine(FadeOutAlpha(sprite, 10));
		}
	}
	
	IEnumerator PlayDestructionParticleFX(GameObject parent) {
		GameObject particleFX = Instantiate(References.DestructionParticleFX, parent.transform.position, Quaternion.identity) as GameObject;
		particleFX.transform.parent = parent.transform;
		particleFX.transform.position = parent.transform.position - new Vector3(0, 0, 1);
		ParticleSystem particles = particleFX.GetComponent<ParticleSystem>();
		particles.Play();
		
		while (particles != null && particles.isPlaying) {
			yield return new WaitForSeconds(0);
		}
		
		parent.Remove();
	}
	
	IEnumerator FadeOutAlpha(SpriteRenderer sprite, float speed) {
		while (sprite != null && sprite.color.a > 0) {
			sprite.SetColor(sprite.color.a - 10 * Time.deltaTime, "A");
			yield return new WaitForSeconds(0);
		}
	}
}

