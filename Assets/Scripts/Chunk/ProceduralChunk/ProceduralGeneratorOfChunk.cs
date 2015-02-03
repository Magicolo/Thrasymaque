using UnityEngine;
using System.Collections.Generic;
using Magicolo;

public class ProceduralGeneratorOfChunk : MonoBehaviour {

	public Transform playersTransform;
	
	public int currentRoom = 5;

	public int chunckInAdvanceOfPlayer = 1;
	
	public static int seed = 1337;
	public System.Random random = new System.Random(seed);
	
	public ChunkBag chunkBag;
	public List<ChunkFlow> chunkFlows = new List<ChunkFlow>();
	
	void Awake(){
		chunkBag = new ChunkBag(random);
		ChunkFlow chunkFlow = new ChunkFlow(this,chunkBag,random, 1, Vector3.zero, Vector3.zero);
		chunkFlows.Add(chunkFlow);
		chunkFlow.loadNextChunk();
		playersTransform.position = new Vector3(0,8,0);
	}

	public void setCurrentRoom(int chunkId){
		currentRoom = chunkId;
	}

	void Start () {
		
	}
	
	void Update () {
		foreach (var chunk in chunkFlows) {
			chunk.update();
		}
	}
	
	public int getChunkIdToGenerate(){
		return this.chunckInAdvanceOfPlayer + currentRoom;
	}
}
