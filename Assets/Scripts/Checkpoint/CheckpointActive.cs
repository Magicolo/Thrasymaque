using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class CheckpointActive : State {
	
	public float oscillationFrequency = 20;
	public float oscillationAmplitude = 0.5F;
	
    Checkpoint Layer {
    	get { return ((Checkpoint)layer); }
    }
	
	public override void OnEnter() {
		Chunk parentChunk = GetComponentInParent<Chunk>();
		
		GameData.chunkId = parentChunk.chunkId;
		References.Runner.GetState<RunnerRunning>().speed = Mathf.Max(References.Runner.GetState<RunnerRunning>().speed + 1, 55);
		GameData.playerSpeed = References.Runner.GetState<RunnerRunning>().speed;
		AudioMaster.PlayNextAudioClip();
	}
	
	public override void OnExit() {
		
	}
	
	public override void OnUpdate() {
		Layer.Sprite.SetColor(oscillationAmplitude * Mathf.Sin(Time.time * oscillationFrequency) + 1 - oscillationAmplitude, "RB");
	}
}
