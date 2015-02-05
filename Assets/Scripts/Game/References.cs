using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class References : MonoBehaviourExtended {

	static References instance;
	static References Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<References>();
			}
			
			return instance;
		}
	}
	
	public Runner runner;
	public static Runner Runner {
		get {
			if (Instance.runner == null) {
				Instance.runner = FindObjectOfType<Runner>();
			}
    		
			return Instance.runner;
		}
	}
    
	public RunnerCamera runnerCamera;
	public static RunnerCamera RunnerCamera {
		get {
			return Instance.runnerCamera;
		}
	}
    
	public GameObject creationParticleFX;
	public static GameObject CreationParticleFX {
		get {
			return Instance.creationParticleFX;
		}
	}
    
	public GameObject destructionParticleFX;
	public static GameObject DestructionParticleFX {
		get {
			return Instance.destructionParticleFX;
		}
	}
	
	public ProceduralGeneratorOfChunk proceduralGeneratorOfChunk;
	public static ProceduralGeneratorOfChunk ProceduralGeneratorOfChunk {
		get {
			return Instance.proceduralGeneratorOfChunk;
		}
	}
    
}

