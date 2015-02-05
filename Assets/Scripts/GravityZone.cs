using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class GravityZone : MonoBehaviourExtended {

	public int targetRotation = 90;
	
	void OnTriggerEnter2D(Collider2D collision) {
		Runner runner = collision.gameObject.GetComponent<Runner>();
			
		if (runner != null) {
			Chunk parentChunk = GetComponentInParent<Chunk>();
			runner.ChangeOrientation(parentChunk.orientation + targetRotation);
		}
	}
}

