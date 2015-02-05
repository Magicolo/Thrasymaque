using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class CheckpointIdle : State {
	
	public float oscillationFrequency = 10;
	public float oscillationAmplitude = 0.5F;
	
    Checkpoint Layer {
    	get { return ((Checkpoint)layer); }
    }
	
	public override void OnEnter() {
		
	}
	
	public override void OnExit() {
		
	}
	
	public override void OnUpdate() {
		Layer.Sprite.SetColor(oscillationAmplitude * Mathf.Sin(Time.time * oscillationFrequency) + 1 - oscillationAmplitude, "B");
	}
	
	public override void TriggerEnter2D(Collider2D collision){
		RunnerRunning runnerRunning = collision.GetComponent<RunnerRunning>();
		
		if (runnerRunning != null){
			SwitchState<CheckpointActive>();
		}
	}
	
}
