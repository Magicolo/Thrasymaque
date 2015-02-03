using UnityEngine;
using Magicolo;

public class Chunk : MonoBehaviour {

	public int chunkId;
	
	public int width;
	public int height;
	public int entreanceY;
	
	public int rightExitY;
	public int upExitX;
	
	[Disable] public bool isStraight = true;
	
	public ProceduralGeneratorOfChunk proceduralGenerator;
	
	void OnTriggerEnter2D(Collider2D other) {
		if ( other.tag == "Player"){
			proceduralGenerator.setCurrentChunk(this);
		}
	
	}
			
}
