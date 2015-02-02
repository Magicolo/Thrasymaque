using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class BaseObject : StateLayer {
	
	public float orientation;
	public float gravityStrength;
	[Disable] public Vector2 gravity;
	
	public override void OnFixedUpdate() {
		gravity = (Vector2.up * -gravityStrength).Rotate(-orientation);
		rigidbody2D.velocity += gravity;
		
		base.OnFixedUpdate();
	}
}
