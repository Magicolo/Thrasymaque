using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public static class GameObjectExtend{

	
	public static GameObject createClone(GameObject original){
		GameObject go = (GameObject)Object.Instantiate(original);
		go.name = original.name;
		
		return go;
	}
	
	public static GameObject createClone(GameObject original, string name, Transform parent, Vector3 position, bool translateToParent = false){
		GameObject go = (GameObject)Object.Instantiate(original);
		go.name = name;
		go.transform.parent = parent;
		if(translateToParent){
			go.transform.position = parent.transform.position;
		}
		go.transform.Translate(position);
		go.transform.Translate(original.transform.position);
		
		return go;
	}
	
	public static GameObject createGameObject(string name, Transform parent, Vector3 position, bool translateToParent = false){
		GameObject go = new GameObject();
		go.name = name;
		go.transform.parent = parent;
		if(translateToParent){
			go.transform.position = parent.transform.position;
		}
		
		go.transform.Translate(position);
		
		return go;
	}
	
	public static GameObject createGameObject(string name, Transform parent){
		GameObject go = new GameObject();
		go.name = name;
		go.transform.parent = parent;
		
		return go;
	}
	
	public static void deleteAllChilds(GameObject go){
		var children = new List<GameObject>();
		foreach (Transform child in go.transform) children.Add(child.gameObject);
		children.ForEach(child => deleteGameObject(child));
	}
	
	public static void deleteGameObject(GameObject go){
		if(Application.isPlaying){
			Object.Destroy(go);
		}else{
			Object.DestroyImmediate(go);
		}
	}
}