using UnityEngine;
using System.Collections.Generic;
using Magicolo;

public class ProceduralChunk : MonoBehaviour {

	public Transform playersTransform;
	
	[Disable] public int currentRoom;
	[Disable] public int nextChunkId = 1;
	[Disable] public System.Random random;
	
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
		GameObject nextChunk = getRandomChunk(linearChunkPrefab);
		GameObjectExtend.createClone(nextChunk,"Chunk" + nextChunkId++,this.transform,new Vector3(0,0,0));
	}

	GameObject getRandomChunk(List<GameObject> list){
		int index = (int)(random.NextDouble() * list.Count);
		return list[index];
		
	}
	void Start () {
		
	}
	
	void Update () {
	
	}
}
