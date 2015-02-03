using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class RunnerInAir : State {
	
	Runner Layer {
		get { return ((Runner)layer); }
	}
	
	public override void OnUpdate() {
		LayerMask layerMask = new LayerMask().AddToMask("Runner", "Chunk").Inverse();
		Vector2 direction = -transform.up;
		float distance = 2;
		RaycastHit2D hitLeft = Physics2D.Raycast(transform.position - transform.right * 0.9F, direction, distance, layerMask);
		RaycastHit2D hitCenter = Physics2D.Raycast(transform.position, direction, distance, layerMask);
		RaycastHit2D hitRight = Physics2D.Raycast(transform.position + transform.right * 0.9F, direction, distance, layerMask);
		
		if (Layer.debug) {
			Debug.DrawRay(transform.position - transform.right * 0.9F, direction * distance, Color.green);
			Debug.DrawRay(transform.position, direction * distance, Color.cyan);
			Debug.DrawRay(transform.position + transform.right * 0.9F, direction * distance, Color.green);
		}
		
		if (hitLeft.collider != null || hitCenter.collider != null || hitRight.collider != null) {
			SwitchState<RunnerGrounded>(1);
			return;
		}
	}
}
