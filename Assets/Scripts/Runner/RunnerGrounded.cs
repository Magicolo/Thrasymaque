using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class RunnerGrounded : State {
	
	Runner Layer {
		get { return ((Runner)layer); }
	}

	public override void OnUpdate() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			SwitchState<RunnerJump>(1);
			return;
		}
		
		RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 2, new LayerMask().AddToMask("Runner").Inverse());
		
		if (Layer.debug) {
			Debug.DrawRay(transform.position, -transform.up * 2, Color.green);
		}
		
		if (hit.collider == null) {
			SwitchState<RunnerInAir>(1);
			return;
		}
	}
	
}
