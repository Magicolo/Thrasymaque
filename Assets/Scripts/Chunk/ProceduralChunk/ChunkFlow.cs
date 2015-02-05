using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections.Generic;
using Magicolo;

[System.Serializable]
public class ChunkFlow {

	
	public Vector3 direction;
	public float rotation;
	
	[Disable] public Vector3 lastRoomEndPosition;
	[Disable] public int lastRoomRightExitY;
	[Disable] public int nextChunkId = 1;
	[Disable] public Chunk lastChunk;
	
	[Disable]public ChunkBag chunkBag;
	private ProceduralGeneratorOfChunk proceduralGeneratorOfChunk;
	private Transform generationParentTransform;
	
	public System.Random random;
	public float nextCornerChance = 0;
	public float baseCornerChance = 0.05f;
	public float baseCornerChanceIncremental = 0.05f;

	public ChunkFlow(ProceduralGeneratorOfChunk proceduralGeneratorOfChunk, Chunk lastChunk, ChunkBag chunkBag, System.Random random, int startingChunkId, Vector3 startingPosition, float angle){
		lastRoomEndPosition = startingPosition;
		this.proceduralGeneratorOfChunk = proceduralGeneratorOfChunk;
		generationParentTransform = proceduralGeneratorOfChunk.transform;
		this.chunkBag = chunkBag;
		this.lastChunk = lastChunk;
		this.random = random;
		this.nextChunkId = startingChunkId;
		rotation = angle;
		
		makeStartChunk();
	}
	
	public void update(){
		if(playerIsInThisFlow()){
			if(proceduralGeneratorOfChunk.getChunkIdToGenerate() > nextChunkId){
				loadNextChunk();
			}
		}else{
			proceduralGeneratorOfChunk.chunkFlowsToRemove.Add(this);
		}
		
	}

	bool playerIsInThisFlow(){
		Chunk previousChunk = lastChunk;
		while(previousChunk != null){
			if(previousChunk.playerPassedThrought){
				return true;
			}else{
				previousChunk = previousChunk.lastChunk;
			}
		}
		
		return false;
	}
	
	public Chunk loadFirstChunk(){
		System.Random randomToGen = this.random.Clone<System.Random>();
		
		GameObject nextChunkPrefab = chunkBag.getRandomChunk(random);
		Chunk newChunk = createAndPlaceNewChunk(nextChunkPrefab,nextChunkId);
		newChunk.orientation = this.rotation;
		nextChunkId++;
		return newChunk;
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

	Chunk makeCornerChunk(){
		GameObject nextChunkPrefab = chunkBag.getRandomChunkFrom(random, chunkBag.cornerChunkPrefab);
		if(nextChunkPrefab == null){
			return null;
		}
		
		Chunk newChunk = createAndPlaceNewChunk(nextChunkPrefab,nextChunkId);
		newChunk.orientation = this.rotation;
		nextChunkId++;
		
		if(newChunk.upExitX != -1){
			Vector3 movement = new Vector3(-newChunk.width + newChunk.upExitX, newChunk.height,0);
			float newAngle = (rotation + 90) % 360;
			
			makeFlow(newChunk, movement, newAngle,nextChunkId);
		}
		
		if(newChunk.downExitX != -1){
			Vector3 movement = new Vector3(-newChunk.width + newChunk.downExitX, 0,0);
			float newAngle = (rotation - 90) % 360;
			
			makeFlow(newChunk, movement, newAngle,nextChunkId);
		}
		
		if(newChunk.rightExitY == -1){
			proceduralGeneratorOfChunk.chunkFlowsToRemove.Add(this);
		}
		return newChunk;
	}

	void makeFlow(Chunk chunk, Vector3 movement, float newAngle, int chunkId){
		Vector3 startingPosition = moveRelative(lastRoomEndPosition, movement, rotation);
		
		ChunkFlow newFlow = new ChunkFlow(proceduralGeneratorOfChunk,lastChunk,chunkBag,random,chunkId,startingPosition,newAngle);
		chunk.flow = newFlow;
		chunk.chunkFlowPresent = true;
		proceduralGeneratorOfChunk.chunkFlowsToAdd.Add(newFlow);
	}
	
	
	Vector3 moveRelative(Vector3 target, Vector3 translation, float rotationToDo){
		Vector3 movement = translation.Rotate(rotationToDo, Vector3.back);
		return target + movement;
	}

	Chunk makeStartChunk(){
		GameObject nextChunkPrefab = chunkBag.getRandomStartChunk(random);
		Chunk newChunk = createAndPlaceNewChunk(nextChunkPrefab,nextChunkId);
		nextChunkId++;
		newChunk.orientation = this.rotation;
		return newChunk;
	}
	
	Chunk makeStraightChunk(){
		GameObject nextChunkPrefab = chunkBag.getRandomChunk(random);
		Chunk newChunk = createAndPlaceNewChunk(nextChunkPrefab,nextChunkId);
		newChunk.orientation = this.rotation;
		nextChunkId++;
		return newChunk;
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
		if(lastChunk != null){
			lastChunk.nextChunk = newChunk;
			newChunk.lastChunk = lastChunk;
		}
		lastChunk = newChunk;
		
		proceduralGeneratorOfChunk.chunksToAdd.Add(newChunk);
		newChunk.chunkId = chunkId;
		newChunk.proceduralGenerator = proceduralGeneratorOfChunk;
		
		Vector3 movementX = new Vector3(prefabChunk.width,0,0);
		movementX = movementX.Rotate(rotation, Vector3.back);
		lastRoomEndPosition += movementX;
		lastRoomRightExitY = prefabChunk.rightExitY;
		return newChunk;
	}
}
