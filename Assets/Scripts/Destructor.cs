using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class Destructor : MonoBehaviourExtended {

	void OnTriggerEnter2D(Collider2D collision) {
		StartCoroutine(DestroyObject(collision.gameObject));
	}
	
	IEnumerator DestroyObject(GameObject obj) {
		GameObject particleFX = Instantiate(References.CreationParticleFX, obj.transform.position, Quaternion.identity) as GameObject;
		particleFX.transform.parent = obj.transform;
		particleFX.transform.position = obj.transform.position - new Vector3(0, 0, 1);
		ParticleSystem particles = particleFX.GetComponent<ParticleSystem>();
		particles.Play();
		
		SpriteRenderer sprite = obj.GetComponentInChildren<SpriteRenderer>();
		while (sprite != null && sprite.color.a > 0) {
			sprite.SetColor(sprite.color.a - 10 * Time.deltaTime, "A");
			yield return new WaitForSeconds(0);
		}
			
		while (particles != null && particles.isPlaying) {
			yield return new WaitForSeconds(0);
		}
		
		obj.Remove();
	}
}

