using UnityEngine;
using System.Collections;
using Magicolo;

public class RunnerCamera : MonoBehaviour {

	public float translateSpeed = 5;
	public float rotateSpeed = 5;
	public Vector3 offset = new Vector3(0, 0, -50);
	
	void FixedUpdate() {
		transform.RotateTowards(References.Runner.orientation, 5, "Z");
		
//		if (References.Runner.rigidbody2D.velocity.magnitude >= 50) {
//			rigidbody2D.TranslateTowards(References.Runner.transform.position + offset, translateSpeed);
//		}
//		else {
//			Logger.Log(transform.right * translateSpeed * Time.fixedDeltaTime);
//			rigidbody2D.Translate(transform.right * translateSpeed * Time.fixedDeltaTime);
//		}
	}
}
