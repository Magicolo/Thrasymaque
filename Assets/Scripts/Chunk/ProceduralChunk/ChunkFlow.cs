using UnityEngine;
using System.Collections.Generic;
using Magicolo;

[System.Serializable]
public class ChunkFlow {

	public Vector3 lastRoomEndPosition;
	public int lastRoomRightExitY;
	[Disable] public int nextChunkId = 1;
	
	[Disable]public ChunkBag chunkBag;
	private ProceduralGeneratorOfChunk proceduralGenerator;
	private Transform generationParentTransform;
	
	public System.Random random;
	public float nextCornerChance = 0;
	public float baseCornerChance = 0.1f;
	public float baseCornerChanceIncremental = 0.1f;
	
	public ChunkFlow(ProceduralGeneratorOfChunk proceduralGenerator, int seed){
		this.proceduralGenerator = proceduralGenerator;
		generationParentTransform = proceduralGenerator.transform;
		
		random = new System.Random(seed);
		chunkBag = new ChunkBag(random);
	}
	
	public void loadNextChunk(){
		
		
		
		GameObject nextChunkPrefab = chunkBag.getRandomChunk();
		createAndPlaceNewChunk(nextChunkPrefab,nextChunkId);
		nextChunkId++;
	}
	
	void createAndPlaceNewChunk(GameObject prefab, int chunkId){
		Chunk prefabChunk = prefab.GetComponent<Chunk>();
		int yDifference = this.lastRoomRightExitY - prefabChunk.entreanceY;
		lastRoomEndPosition += new Vector3(0,yDifference,0);
		
		GameObject newChunkGO = GameObjectExtend.createClone(prefab,"Chunk" + nextChunkId, generationParentTransform,lastRoomEndPosition);
		
		Chunk newChunk = newChunkGO.GetComponent<Chunk>();
		newChunk.chunkId = nextChunkId;
		newChunk.proceduralGenerator = proceduralGenerator;
		
		this.lastRoomEndPosition += new Vector3(prefabChunk.width,0,0);
		lastRoomRightExitY = prefabChunk.rightExitY;
	}
}
