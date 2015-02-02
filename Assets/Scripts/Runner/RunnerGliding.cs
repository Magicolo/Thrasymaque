using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class RunnerGliding : State {
	
	public float glideDuration = 0.2F;
	[Disable] public float glideCounter;
	[Disable] public Vector2 velocity;
	
	Runner Layer {
		get { return ((Runner)layer); }
	}
		
	public override void OnEnter() {
		glideCounter = glideDuration;
	}
	
	public override void OnFixedUpdate() {
		glideCounter -= Time.fixedDeltaTime;
		if (glideCounter <= 0 || !Input.GetKey(KeyCode.Space)) {
			SwitchState<RunnerInAir>(1);
			return;
		}
		
		velocity = rigidbody2D.velocity.Rotate(Layer.orientation);
		velocity.y = Mathf.Max(velocity.y, 0);
		velocity = velocity.Rotate(-Layer.orientation);
		rigidbody2D.velocity = velocity;
	}
	
}
