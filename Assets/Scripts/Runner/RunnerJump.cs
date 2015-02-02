using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class RunnerJump : State {
	
	public float jumpDuration = 0.5F;
	public float jumpHeight = 5;
	public float jumpMinHeight = 8;
	[Disable] public float jumpYPosition;
	[Disable] public float jumpOrientation;
	[Disable] public float jumpCounter;
	[Disable] public Vector2 velocity;
	
	Runner Layer {
		get { return ((Runner)layer); }
	}
	
	public override void OnEnter() {
		jumpCounter = jumpDuration;
		jumpOrientation = Layer.orientation;
		jumpYPosition = transform.position.Rotate(jumpOrientation).y;
		
		velocity = rigidbody2D.velocity + new Vector2(0, jumpHeight * jumpMinHeight).Rotate(-jumpOrientation);
		rigidbody2D.velocity = velocity;
	}
	
	public override void OnExit() {
		
	}
	
	public override void OnFixedUpdate() {
		jumpCounter -= Time.fixedDeltaTime;
		
		if (jumpCounter <= 0 && Input.GetKey(KeyCode.Space)) {
			SwitchState<RunnerGliding>(1);
			return;
		}
		
		if (jumpCounter <= 0 || !Input.GetKey(KeyCode.Space)) {
			SwitchState<RunnerInAir>(1);
			return;
		}
		
		velocity = rigidbody2D.velocity + new Vector2(0, jumpHeight).Rotate(-jumpOrientation);
		rigidbody2D.velocity = velocity;
	}
	
}
