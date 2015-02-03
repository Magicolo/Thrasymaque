using UnityEngine;
using System.Collections.Generic;
using Magicolo;

public class ProceduralGeneratorOfChunk : MonoBehaviour {

	public Transform playersTransform;
	
	public string levelName;
	public int currentChunkId = 0;
	public Chunk currentChunk;

	public int chunckInAdvanceOfPlayer = 4;
	public int chunckBackOfPlayer = 4;
	
	public int seed = 1337;
	public System.Random random;
	
	public ChunkBag chunkBag;
	public List<ChunkFlow> chunkFlowsToAdd = new List<ChunkFlow>();
	public List<ChunkFlow> chunkFlowsToRemove = new List<ChunkFlow>();
	public List<ChunkFlow> chunkFlows = new List<ChunkFlow>();
	
	public List<Chunk> chunksToAdd = new List<Chunk>();
	public List<Chunk> chunksToRemove = new List<Chunk>();
	public List<Chunk> chunks = new List<Chunk>();
	
	void Awake(){
		random = new System.Random(seed);
		chunkBag = new ChunkBag(random, levelName);
		ChunkFlow chunkFlow = new ChunkFlow(this,chunkBag,random, 1, Vector3.zero, 0);
		chunkFlows.Add(chunkFlow);
		chunkFlow.loadNextChunk();
		playersTransform.position = new Vector3(0,8,0);
	}

	public void setCurrentChunk(Chunk chunk){
		if(chunk.chunkId > this.currentChunkId){
			currentChunk = chunk;
			currentChunkId = chunk.chunkId;
		}
		
	}

	void Start () {
		
	}
	
	void Update () {
		foreach (var chunk in chunkFlows) {
			chunk.update();
		}
		
		foreach (var chunkFlowToRemove in chunkFlowsToRemove) {
			chunkFlows.Remove(chunkFlowToRemove);
		}
		
		chunkFlowsToRemove.Clear();
		foreach (var chunkFlowToAdd in chunkFlowsToAdd) {
			chunkFlows.Add(chunkFlowToAdd);
		}
		chunkFlowsToAdd.Clear();
		
		
		
		
		foreach (var chunk in chunks) {
			if(chunk.chunkId + chunckBackOfPlayer < currentChunkId){
				chunksToRemove.Add(chunk);
			}
		}
		
		foreach (var chunkToRemove in chunksToRemove) {
			Object.Destroy(chunkToRemove.gameObject);
			chunks.Remove(chunkToRemove);
		}
		chunksToRemove.Clear();
		
		foreach (var chunkToAdd in chunksToAdd) {
			chunks.Add(chunkToAdd);
		}
		chunksToAdd.Clear();
	}
	
	public int getChunkIdToGenerate(){
		return this.chunckInAdvanceOfPlayer + currentChunkId;
	}
}
