using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class Runner : BaseObject {
	
	public bool debug;
	
	public override void OnUpdate() {
		base.OnUpdate();
		
		if (debug) {
			if (Input.GetKeyDown(KeyCode.RightArrow)) {
				ChangeOrientation(orientation + 90);
			}
			
			if (Input.GetKeyDown(KeyCode.LeftArrow)) {
				ChangeOrientation(orientation - 90);
			}
		}
	}
	
	public void ChangeOrientation(float newOrientation) {
		orientation = newOrientation % 360;
		transform.SetEulerAngles(orientation, "Z");
		PureData.Send("Rotate");
	}

	public void die(){
		Application.LoadLevel(0);
	}
}
