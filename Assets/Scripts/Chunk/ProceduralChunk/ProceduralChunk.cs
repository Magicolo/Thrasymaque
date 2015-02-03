using UnityEngine;
using System.Collections.Generic;
using Magicolo;

public class ProceduralChunk : MonoBehaviour {

	public Transform playersTransform;
	
	[Disable] public int currentRoom;
	[Disable] public int nextChunkId = 1;
	[Disable] public System.Random random;
	
	public Vector3 lastRoomEndPosition;
	public int lastRoomRightExitY;
	
	public int chunckWidthInAdvanceOfPlayer = 50;
	
	public static int seed = 1337;
	
	
	public List<GameObject> linearChunkPrefab = new List<GameObject>();
	
	void Awake(){
		random = new System.Random(seed);
		loadLinearChunk();
		loadNextChunk();
		playersTransform.position = new Vector3(0,8,0);
	}

	void loadLinearChunk(){
		GameObject[] chunksObjects = (GameObject[]) Resources.LoadAll<GameObject>("Chunks/Straight");
		foreach (var obj in chunksObjects) {
			linearChunkPrefab.Add(obj);
		}
	}

	public void setCurrentRoom(int chunkId){
		currentRoom = chunkId;
	}

	void loadNextChunk(){
		GameObject nextChunkGO = getRandomChunk(linearChunkPrefab);
		Chunk nextChunk = nextChunkGO.GetComponent<Chunk>();
		int yDifference = this.lastRoomRightExitY - nextChunk.entreanceY;
		lastRoomEndPosition += new Vector3(0,yDifference,0);
		
		GameObjectExtend.createClone(nextChunkGO,"Chunk" + nextChunkId++,this.transform,lastRoomEndPosition);
		
		this.lastRoomEndPosition += new Vector3(nextChunk.width,0,0);
		lastRoomRightExitY = nextChunk.rightExitY;
		
	}

	GameObject getRandomChunk(List<GameObject> list){
		int index = (int)(random.NextDouble() * list.Count);
		return list[index];
		
	}
	void Start () {
		
	}
	
	void Update () {
		if(this.playersTransform.position.x + chunckWidthInAdvanceOfPlayer > lastRoomEndPosition.x){
			loadNextChunk();
		}
	}
}
