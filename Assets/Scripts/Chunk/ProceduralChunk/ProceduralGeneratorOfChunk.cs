using UnityEngine;
using System.Collections.Generic;
using Magicolo;

public class ProceduralGeneratorOfChunk : MonoBehaviour {

	public Transform playersTransform;
	
	public string levelName;
	public int currentChunkId = 0;
	public Chunk currentChunk;

	public int chunckInAdvanceOfPlayer = 4;
	
	public static int seed = 1337;
	public System.Random random = new System.Random(seed);
	
	public ChunkBag chunkBag;
	public List<ChunkFlow> chunkFlowsToAdd = new List<ChunkFlow>();
	public List<ChunkFlow> chunkFlowsToRemove = new List<ChunkFlow>();
	public List<ChunkFlow> chunkFlows = new List<ChunkFlow>();
	
	void Awake(){
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
	}
	
	public int getChunkIdToGenerate(){
		return this.chunckInAdvanceOfPlayer + currentChunkId;
	}
}
