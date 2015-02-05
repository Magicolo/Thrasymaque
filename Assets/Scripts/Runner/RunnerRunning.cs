using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class RunnerRunning : State {
	
	public float speed = 5;
	[Disable] public Vector2 velocity;
	
    Runner Layer {
    	get { return ((Runner)layer); }
    }
	
	public override void OnAwake(){
		speed = GameData.playerSpeed;
	}
	
	public override void OnFixedUpdate() {
		velocity = rigidbody2D.velocity.Rotate(Layer.orientation);
		velocity.x = speed;
		velocity = velocity.Rotate(-Layer.orientation);
		rigidbody2D.velocity = velocity;
	}
}
