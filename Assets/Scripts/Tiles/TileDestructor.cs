using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class TileDestructor : MonoBehaviourExtended {

	void Update() {
		const float distance = 50;
		Vector2 direction = transform.right;
		Vector2 size = new Vector2(70, 70);
		Vector2 center = transform.position - (Vector3)(direction * distance) * 1.5F;
		LayerMask layerMask = new LayerMask().AddToMask("ToDestroy");
		RaycastHit2D[] hits = Physics2D.BoxCastAll(center, size, 0, direction, distance, layerMask);
		
		foreach (RaycastHit2D hit in hits) {
			DestroyObject(hit.collider.gameObject);
		}
	}
	
	void DestroyObject(GameObject obj) {
		obj.layer = 19;
		
		StartCoroutine(PlayDestructionParticleFX(obj));
		
		foreach (SpriteRenderer sprite in obj.GetComponentsInChildren<SpriteRenderer>()) {
			StartCoroutine(FadeOutAlpha(sprite, 10));
		}
		
		PureData.Send("Destroy");
	}
	
	IEnumerator PlayDestructionParticleFX(GameObject parent) {
		GameObject particleFX = Instantiate(References.DestructionParticleFX, parent.transform.position, Quaternion.identity) as GameObject;
		particleFX.transform.parent = parent.transform;
		particleFX.transform.position = parent.transform.position - new Vector3(0, 0, 1);
		ParticleSystem particles = particleFX.GetComponentInChildren<ParticleSystem>();
		
		if (particles != null) {
			particles.Play();
		
			while (particles != null && particles.isPlaying) {
				yield return new WaitForSeconds(0);
			}
		
			parent.Remove();
		}
	}
	
	IEnumerator FadeOutAlpha(SpriteRenderer sprite, float speed) {
		while (sprite != null && sprite.color.a > 0) {
			sprite.SetColor(sprite.color.a - 10 * Time.deltaTime, "A");
			yield return new WaitForSeconds(0);
		}
	}
}

