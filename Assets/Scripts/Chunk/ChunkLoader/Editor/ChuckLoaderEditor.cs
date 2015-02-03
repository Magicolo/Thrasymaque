﻿using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;


[System.Serializable]
public class ChuckLoaderEditor : EditorWindow {

	public string filePath = "";
	
	void OnGUI(){	
		addFileLine();
		if (GUILayout.Button ("Load Map")) {
			string chunkName = Path.GetFileName(filePath).Split(new char[]{'.'})[0];
			GameObject chunkGameObject = new GameObject(chunkName);
			
			ChunckLoader loader = new ChunckLoader(chunkGameObject);
			loader.loadFromFile(filePath);
			
			makeGameObjectAsPrefab(chunkGameObject, chunkName);
		}
	}

	void makeGameObjectAsPrefab(GameObject chunkGameObject, string chunkName){
		PrefabUtility.CreatePrefab("Assets/Resources/Chunks/"+chunkName+".prefab", chunkGameObject);
		Object.DestroyImmediate(chunkGameObject);
	}
	
	void addFileLine(){
		GUILayout.BeginHorizontal ();
		filePath = GUILayout.TextField (filePath);
		if (GUILayout.Button ("Open Map File")) {
			filePath = EditorUtility.OpenFilePanel("Open Map File","maps","tmx");
		}
		GUILayout.EndHorizontal ();
	}
	
	[MenuItem ("FruitsUtils/Map Loader")]
	public static void ShowWindow(){
		EditorWindow.GetWindow(typeof(ChuckLoaderEditor), true, "Map Loader");
	}
}
