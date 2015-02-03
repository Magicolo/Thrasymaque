using UnityEngine;
using Magicolo;

public class Chunk : MonoBehaviour {

	public int chunkId;
	
	public int width;
	public int height;
	public int entreanceY;
	
	public int rightExitY = -1;
	public int upExitX = -1;
	public int downExitX = -1;
	
	public float orientation;
	
	[Disable] public ChunkFlow flow;
	
	[Disable] public bool isStraight = true;
	
	public ProceduralGeneratorOfChunk proceduralGenerator;
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			proceduralGenerator.setCurrentChunk(this);
		}
	}
}
