using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class ChunkDestructor : MonoBehaviourExtended {

	void OnTriggerEnter2D(Collider2D collider) {
		collider.gameObject.Remove();
	}
}

