using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class ChunkDestructor : MonoBehaviourExtended {

	void OnTriggerEnter2D(Collider2D collision) {
		Chunk chunk = collision.gameObject.GetComponent<Chunk>();
		
		if (chunk != null && chunk.chunkId < References.ProceduralGeneratorOfChunk.currentChunkId) {
			collision.gameObject.Remove();
		}
	}
}

