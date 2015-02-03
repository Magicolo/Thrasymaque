using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class Constructor : MonoBehaviourExtended {

	void OnTriggerEnter2D(Collider2D collision) {
		StartCoroutine(ActivateObject(collision.gameObject));
	}
	
	IEnumerator ActivateObject(GameObject obj) {
		bool activate = false;
		foreach (GameObject child in obj.GetChildrenRecursive()) {
			if (!child.activeSelf) {
				child.SetActive(true);
				activate = true;
			}
		}
		
		SpriteRenderer sprite = obj.GetComponentInChildren<SpriteRenderer>();
		
		if (activate && sprite != null) {
			GameObject particleFX = Instantiate(References.CreationParticleFX, obj.transform.position, Quaternion.identity) as GameObject;
			particleFX.transform.parent = obj.transform;
			particleFX.transform.position = obj.transform.position - new Vector3(0, 0, 1);
			ParticleSystem particles = particleFX.GetComponent<ParticleSystem>();
			particles.Play();
		
			sprite.SetColor(0, "A");
			while (sprite != null && sprite.color.a < 1) {
				sprite.SetColor(sprite.color.a + 10 * Time.deltaTime, "A");
				yield return new WaitForSeconds(0);
			}
		
			while (particles != null && particles.isPlaying) {
				yield return new WaitForSeconds(0);
			}
		
			particleFX.Remove();
		}
	}
}

