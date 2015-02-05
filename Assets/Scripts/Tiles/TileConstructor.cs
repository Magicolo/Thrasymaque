using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class TileConstructor : MonoBehaviourExtended {

	void Update() {
		const float distance = 50;
		Vector2 direction = transform.right;
		Vector2 size = new Vector2(70, 70);
		Vector2 center = transform.position - (Vector3)(direction * distance);
		LayerMask layerMask = new LayerMask().AddToMask("ToConstruct");
		RaycastHit2D[] hits = Physics2D.BoxCastAll(center, size, 0, direction, distance, layerMask);
		
		foreach (RaycastHit2D hit in hits) {
			ActivateObject(hit.collider.gameObject);
		}
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

