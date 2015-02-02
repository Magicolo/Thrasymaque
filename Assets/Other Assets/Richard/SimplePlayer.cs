using UnityEngine;
using System.Collections;

public class SimplePlayer : MonoBehaviour {

	public float speed = 5;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {	
		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");
		
		Vector3 motion = new Vector3(x,0,z);
		motion.Normalize();
		
		this.transform.position += motion * Time.deltaTime * speed;
		
	}
}