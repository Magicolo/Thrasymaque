using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class Constructor : MonoBehaviourExtended {

	void OnTriggerEnter2D(Collider2D collision) {
		if (!collision.gameObject.activeSelf) {
			StartCoroutine(ActivateObject(collision.gameObject));
		}
	}
	
	IEnumerator ActivateObject(GameObject obj) {
		yield return null;
	}
}

