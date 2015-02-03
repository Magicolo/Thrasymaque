﻿using UnityEngine;
using System.Collections.Generic;
using Magicolo;

[System.Serializable]
public class ChunkBag {

	
	[Disable] public System.Random random;
	
	
	public List<GameObject> linearChunkPrefab = new List<GameObject>();
	public List<GameObject> cornerChunkPrefab = new List<GameObject>();
	
	public ChunkBag(System.Random random, string levelName){
		this.random = random;
		loadChunksFrom("Chunks/"+ levelName + "/Straight",linearChunkPrefab);
		loadChunksFrom("Chunks/"+ levelName + "/Chunks/Corner",cornerChunkPrefab);
	}
	

	void loadChunksFrom(string assetFolderName, List<GameObject> to){
		GameObject[] chunksObjects = (GameObject[]) Resources.LoadAll<GameObject>(assetFolderName);
		foreach (var obj in chunksObjects) {
			to.Add(obj);
		}
	}
	
	public GameObject getRandomChunk(){
		return getRandomChunkFrom(linearChunkPrefab);
	}
	
	public GameObject getRandomChunkFrom(List<GameObject> list){
		if(list.Count == 0) return null;
		int index = (int)(random.NextDouble() * list.Count);
		return list[index];
		
	}
}
