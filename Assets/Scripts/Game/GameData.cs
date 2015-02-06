using UnityEngine;
using System.Collections;
using Magicolo;

public static class GameData {

	static System.Random randomGenerator;
	public static System.Random RandomGenerator {
		get {
			if (randomGenerator == null){
				randomGenerator = new System.Random(References.ProceduralGeneratorOfChunk.seed);
			}
			
			return randomGenerator;
		}
		set {
			randomGenerator = value;
		}
	}

	public static int chunkId;
	public static float playerSpeed = 25;
	public static int audioClipIndex;
}
