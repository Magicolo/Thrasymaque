using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections.Generic;
using Magicolo;

[System.Serializable]
public class ChunkFlow {

	
	public Vector3 direction;
	public float rotation;
	
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

	public ChunkFlow(ProceduralGeneratorOfChunk proceduralGeneratorOfChunk, ChunkBag chunkBag, System.Random random, int startingChunkId, Vector3 startingPosition, float angle){
		lastRoomEndPosition = startingPosition;
		this.proceduralGeneratorOfChunk = proceduralGeneratorOfChunk;
		generationParentTransform = proceduralGeneratorOfChunk.transform;
		this.chunkBag = chunkBag;
		this.random = random;
		this.nextChunkId = startingChunkId;
		rotation = angle;
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
		if(nextChunkPrefab == null){
			return;
		}
		
		Chunk newChunk = createAndPlaceNewChunk(nextChunkPrefab,nextChunkId);
		nextChunkId++;
		
		if(newChunk.upExitX != 0){
			Vector3 movement = new Vector3(-newChunk.upExitX, newChunk.height,0);
			movement = movement.Rotate(rotation, Vector3.back);
			Vector3 startinPosition = lastRoomEndPosition + movement;
			float newAngle = rotation + 90;
			newAngle %= 360;
			ChunkFlow newFlow = new ChunkFlow(proceduralGeneratorOfChunk,chunkBag,random,nextChunkId,startinPosition,newAngle);
			proceduralGeneratorOfChunk.chunkFlowsToAdd.Add(newFlow);
			proceduralGeneratorOfChunk.chunkFlowsToRemove.Add(this);
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
		Vector3 movement = new Vector3(0,yDifference,0);
		movement = movement.Rotate(rotation, Vector3.back);
		lastRoomEndPosition += movement;
		
		GameObject newChunkGO = GameObjectExtend.createClone(prefab,"Chunk" + chunkId, generationParentTransform,lastRoomEndPosition);
		newChunkGO.transform.Rotate(0,0, rotation);
		Chunk newChunk = newChunkGO.GetComponent<Chunk>();
		newChunk.chunkId = chunkId;
		newChunk.proceduralGenerator = proceduralGeneratorOfChunk;
		
		Vector3 movementX = new Vector3(prefabChunk.width,0,0);
		movementX = movementX.Rotate(rotation, Vector3.back);
		Debug.Log(lastRoomEndPosition + " - " + movementX);
		lastRoomEndPosition += movementX;
		lastRoomRightExitY = prefabChunk.rightExitY;
		return newChunk;
	}
}
