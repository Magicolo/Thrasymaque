using UnityEngine;
using System.Collections.Generic;
using Magicolo;

[System.Serializable]
public class ChunkBag {

	
	[Disable] public System.Random random;
	
	
	public List<GameObject> linearChunkPrefab = new List<GameObject>();
	public List<GameObject> cornerChunkPrefab = new List<GameObject>();
	public List<GameObject> startChunkPrefab = new List<GameObject>();
	
	public ChunkBag(System.Random random, string levelName){
		this.random = random;
		loadChunksFrom(levelName, "Straight",linearChunkPrefab);
		loadChunksFrom(levelName, "Corner"	,cornerChunkPrefab);
		loadChunksFrom(levelName, "Start"	,startChunkPrefab);
	}
	

	void loadChunksFrom(string levelName, string FolderName, List<GameObject> to){
		string assetFolderName = "Chunks/"+ levelName + "/" + FolderName;
		GameObject[] chunksObjects = (GameObject[]) Resources.LoadAll<GameObject>(assetFolderName);
		foreach (var obj in chunksObjects) {
			to.Add(obj);
		}
	}
	
	public GameObject getRandomChunk(){
		return getRandomChunkFrom(linearChunkPrefab);
	}
	
	public GameObject getRandomStartChunk(){
		return getRandomChunkFrom(startChunkPrefab);
	}
	
	public GameObject getRandomChunkFrom(List<GameObject> list){
		if(list.Count == 0) return null;
		int index = (int)(random.NextDouble() * list.Count);
		return list[index];
		
	}
}
