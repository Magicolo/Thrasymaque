using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class TileConstructor : MonoBehaviourExtended {

	void OnTriggerEnter2D(Collider2D collision) {
		ActivateObject(collision.gameObject);
	}
	
	void ActivateObject(GameObject obj) {
		bool activate = false;
		foreach (GameObject child in obj.GetChildrenRecursive()) {
			if (!child.activeSelf) {
				child.SetActive(true);
				activate = true;
			}
		}
		
		SpriteRenderer[] sprites = obj.GetComponentsInChildren<SpriteRenderer>();
		
		if (activate) {
			StartCoroutine(PlayCreationParticleFX(obj));
		
			foreach (SpriteRenderer sprite in sprites) {
				StartCoroutine(FadeInAlpha(sprite, 10));
			}
		}
	}
	
	IEnumerator PlayCreationParticleFX(GameObject parent) {
		GameObject particleFX = Instantiate(References.CreationParticleFX, parent.transform.position, Quaternion.identity) as GameObject;
		particleFX.transform.parent = parent.transform;
		particleFX.transform.position = parent.transform.position - new Vector3(0, 0, 1);
		ParticleSystem particles = particleFX.GetComponent<ParticleSystem>();
		particles.Play();
		
		while (particles != null && particles.isPlaying) {
			yield return new WaitForSeconds(0);
		}
		
		particleFX.Remove();
	}
	
	IEnumerator FadeInAlpha(SpriteRenderer sprite, float speed) {
		sprite.SetColor(0, "A");
		
		while (sprite != null && sprite.color.a < 1) {
			sprite.SetColor(sprite.color.a + speed * Time.deltaTime, "A");
			yield return new WaitForSeconds(0);
		}
	}
}

