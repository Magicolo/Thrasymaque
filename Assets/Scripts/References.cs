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
    
    
}

