using UnityEngine;
using System.Collections;
using Magicolo;

public class RunnerCamera : MonoBehaviour {

	public float translateSpeed = 5;
	public float rotateSpeed = 5;
	public Vector3 offset = new Vector3(0, 0, -50);
	
	void FixedUpdate() {
		transform.RotateTowards(References.Runner.orientation, 5, "Z");
		transform.TranslateTowards(References.Runner.transform.position + offset, translateSpeed);
	}
}
