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
	
	[Disable] public ChunkFlow flow = null;
	[Disable] public bool chunkFlowPresent = false;
	[Disable] public Chunk nextChunk;
	[Disable] public Chunk lastChunk;
	[Disable] public bool playerPassedThrought;
	
	[Disable] public bool isStraight = true;
	
	[Disable] public bool startingChunk = false;
	[Disable] public Vector2 checkPointLocation;
	
	
	public System.Random randomToGenerate;

	public ProceduralGeneratorOfChunk proceduralGenerator;
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player" && chunkId > proceduralGenerator.currentChunkId) {
			proceduralGenerator.setCurrentChunk(this);
			playerPassedThrought = true;
			if(randomToGenerate != null){
				GameData.RandomGenerator = randomToGenerate;
				GameData.chunkId = chunkId;
			}
		}
	}
}
