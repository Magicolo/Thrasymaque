using UnityEngine;
using System.Collections.Generic;
using Magicolo;

public class ProceduralGeneratorOfChunk : MonoBehaviour {

	public Transform playersTransform;
	
	public int currentRoom = 5;
	
	
	
	public int chunckInAdvanceOfPlayer = 1;
	
	public static int seed = 1337;
	
	public ChunkFlow chunkFlow;
	
	void Awake(){
		chunkFlow = new ChunkFlow(this,seed);
		chunkFlow.loadNextChunk();
		playersTransform.position = new Vector3(0,8,0);
	}

	public void setCurrentRoom(int chunkId){
		currentRoom = chunkId;
	}

	void Start () {
		
	}
	
	void Update () {
		if(currentRoom + chunckInAdvanceOfPlayer > chunkFlow.nextChunkId){
			chunkFlow.loadNextChunk();
		}
	}
}
