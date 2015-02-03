using UnityEngine;
using System.Collections;

public class Chunk : MonoBehaviour {

	public int width;
	public int height;
	public int chunkId;
	public int yEntreance;
	
	public ProceduralChunk proceduralChunk;
	
	void OnTriggerEnter2D(Collider2D other) {
		if ( other.tag == "Player"){
			proceduralChunk.setCurrentRoom(chunkId);
		}
	
	}
			
}
