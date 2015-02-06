using UnityEngine;
using System.Collections;

public class CameraEffet  {

	private Color32 startColor;
	private Color32 endColor;
	private Camera camera;
	
	private float t;
	private float duration;
	private EffetManager manager;
	
	public CameraEffet(Color32 startColor, Color32 endColor, Camera camera, float duration, EffetManager manager){
		this.startColor = startColor;
		this.endColor = endColor;
		this.camera = camera;
		
		this.duration = duration;
		this.manager = manager;
	}
	
	public void Update(){
		camera.backgroundColor = Color32.Lerp(startColor,endColor,t/duration);
		t+= Time.deltaTime;
		
		if(t >= duration){
			manager.removeEffect(this);
		}
	}
	
}
