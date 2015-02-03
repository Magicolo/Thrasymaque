using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class TileConstructor : MonoBehaviourExtended {

	void OnTriggerEnter2D(Collider2D collision) {
		ActivateObject(collision.gameObject);
	}
	
	void ActivateObject(GameObject obj) {
		obj.layer = 12;
		
		foreach (GameObject child in obj.GetChildrenRecursive()) {
			child.SetActive(true);
			child.layer = 12;
		}
		
		StartCoroutine(PlayCreationParticleFX(obj));
		
		foreach (SpriteRenderer sprite in obj.GetComponentsInChildren<SpriteRenderer>()) {
			StartCoroutine(FadeInAlpha(sprite, 10));
		}
	}
	
	IEnumerator PlayCreationParticleFX(GameObject parent) {
		GameObject particleFX = Instantiate(References.CreationParticleFX, parent.transform.position, Quaternion.identity) as GameObject;
		particleFX.transform.parent = parent.transform;
		particleFX.transform.position = parent.transform.position - new Vector3(0, 0, 1);
		ParticleSystem particles = particleFX.GetComponentInChildren<ParticleSystem>();
		
		if (particles != null) {
			particles.Play();
		
			while (particles != null && particles.isPlaying) {
				yield return new WaitForSeconds(0);
			}
		
			particleFX.Remove();
		}
	}
	
	IEnumerator FadeInAlpha(SpriteRenderer sprite, float speed) {
		sprite.SetColor(0, "A");
		
		while (sprite != null && sprite.color.a < 1) {
			sprite.SetColor(sprite.color.a + speed * Time.deltaTime, "A");
			yield return new WaitForSeconds(0);
		}
	}
}

