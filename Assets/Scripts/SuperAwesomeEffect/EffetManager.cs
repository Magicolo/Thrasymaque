using UnityEngine;
using System.Collections.Generic;

public class EffetManager : MonoBehaviour {

	public List<CameraEffet> effets = new List<CameraEffet>();
	private List<CameraEffet> effetsToAdd = new List<CameraEffet>();
	private List<CameraEffet> effetsToRemove = new List<CameraEffet>();
	
	void Update () {
		foreach (var effect in effets) {
			effect.Update();
		}
		
		foreach (var toRemove in effetsToRemove) {
			effets.Remove(toRemove);
		}
		effetsToRemove.Clear();
		
		foreach (var toAdd in effetsToAdd) {
			effets.Add(toAdd);
		}
		effetsToAdd.Clear();
	}

	public void removeEffect(CameraEffet cameraEffet){
		effetsToRemove.Add(cameraEffet);
	}
	
	public void addEffect(CameraEffet effect){
		this.effetsToAdd.Add(effect);
	}
}
