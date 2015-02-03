using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections.Generic;
using Magicolo;

[System.Serializable]
public class ChunkFlow {

	
	public Vector3 direction;
	
	public Vector3 lastRoomEndPosition;
	public int lastRoomRightExitY;
	[Disable] public int nextChunkId = 1;
	
	[Disable]public ChunkBag chunkBag;
	private ProceduralGeneratorOfChunk proceduralGeneratorOfChunk;
	private Transform generationParentTransform;
	
	public System.Random random;
	public float nextCornerChance = 0;
	public float baseCornerChance = 0.1f;
	public float baseCornerChanceIncremental = 0.1f;

	public ChunkFlow(ProceduralGeneratorOfChunk proceduralGeneratorOfChunk, ChunkBag chunkBag, System.Random random, int startingChunkId, Vector3 startingPosition, Vector3 direction){
		lastRoomEndPosition = startingPosition;
		this.proceduralGeneratorOfChunk = proceduralGeneratorOfChunk;
		generationParentTransform = proceduralGeneratorOfChunk.transform;
		this.direction = direction;
		this.chunkBag = chunkBag;
		this.random = random;
		this.nextChunkId = startingChunkId;
	}
	
	public void update(){
		if(proceduralGeneratorOfChunk.getChunkIdToGenerate() > nextChunkId){
			loadNextChunk();
		}
	}
	
	public void loadNextChunk(){
		
		float nextrandom = (float)random.NextDouble();
		if(nextrandom <= nextCornerChance){
			nextCornerChance = baseCornerChance;
			makeCornerChunk();
		}else{
			nextCornerChance += baseCornerChanceIncremental;
			makeStraightChunk();
		}
		
		
	}

	void makeCornerChunk(){
		GameObject nextChunkPrefab = chunkBag.getRandomChunkFrom(chunkBag.cornerChunkPrefab);
		Chunk newChunk = createAndPlaceNewChunk(nextChunkPrefab,nextChunkId);
		nextChunkId++;
		
		if(newChunk.upExitX != 0){
			Vector3 startinPosition = lastRoomEndPosition += new Vector3(newChunk.width - newChunk.upExitX, newChunk.height,0);
			Vector3 newDirection = direction.Rotate(90);
			ChunkFlow newFlow = new ChunkFlow(proceduralGeneratorOfChunk,chunkBag,random,nextChunkId,startinPosition,newDirection);
			proceduralGeneratorOfChunk.chunkFlows.Add(newFlow);
		}
	}

	void makeStraightChunk(){
		GameObject nextChunkPrefab = chunkBag.getRandomChunk();
		createAndPlaceNewChunk(nextChunkPrefab,nextChunkId);
		nextChunkId++;
	}

	Chunk createAndPlaceNewChunk(GameObject prefab, int chunkId){
		Chunk prefabChunk = prefab.GetComponent<Chunk>();
		int yDifference = this.lastRoomRightExitY - prefabChunk.entreanceY;
		lastRoomEndPosition += new Vector3(0,yDifference,0);
		
		GameObject newChunkGO = GameObjectExtend.createClone(prefab,"Chunk" + chunkId, generationParentTransform,lastRoomEndPosition);
		
		Chunk newChunk = newChunkGO.GetComponent<Chunk>();
		newChunk.chunkId = chunkId;
		newChunk.proceduralGenerator = proceduralGeneratorOfChunk;
		
		this.lastRoomEndPosition += new Vector3(prefabChunk.width,0,0);
		lastRoomRightExitY = prefabChunk.rightExitY;
		return newChunk;
	}
}
