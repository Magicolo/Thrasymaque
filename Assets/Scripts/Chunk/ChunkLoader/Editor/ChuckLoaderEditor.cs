using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;
using Magicolo.EditorTools;
using Magicolo;


[System.Serializable]
public class ChuckLoaderEditor : EditorWindow {

	public string filePath = "";
	public string LevelName = "";
	
	void OnGUI(){	
		
		LevelName = EditorGUILayout.TextField("Level Name" , LevelName);
		addFileLine();
		if (GUILayout.Button ("Load Map")) {
			
			var info = new DirectoryInfo(filePath);
			var fileInfo = info.GetFiles("*.tmx");
			int index = 0;
			
			foreach (FileInfo file in fileInfo){
				string chuckname = file.Name .Split(new char[]{'.'})[0];
				
				GameObject chunkGameObject = new GameObject(chuckname);
				ChunckLoader loader = new ChunckLoader(chunkGameObject);
				
				loader.loadFromFile(file.FullName);
				makeGameObjectAsPrefab(chunkGameObject, chuckname);
				index++;
				Debug.Log("Yo oli on fait " + chuckname);
			}
			Debug.Log("Yo oli on a fini");
		}
	}

	void makeGameObjectAsPrefab(GameObject chunkGameObject, string chunkName){
		Chunk chunk = chunkGameObject.GetComponent<Chunk>();
		string rootFolder = "";
		if(chunk.isStraight){
			rootFolder = "Straight/";
		}else{
			rootFolder = "Corner/";
		}
		PrefabUtility.CreatePrefab("Assets/Resources/Chunks/" + LevelName + "/" + rootFolder + chunkName+".prefab", chunkGameObject);
		Object.DestroyImmediate(chunkGameObject);
	}
	
	void addFileLine(){
		GUILayout.BeginHorizontal ();
		filePath = GUILayout.TextField (filePath);
		if (GUILayout.Button ("Open Map File")) {
			filePath = EditorUtility.OpenFolderPanel("Open Map File","Other Assets/Chunks","tmx");
		}
		GUILayout.EndHorizontal ();
	}
	
	[MenuItem ("FruitsUtils/Map Loader")]
	public static void ShowWindow(){
		EditorWindow.GetWindow(typeof(ChuckLoaderEditor), true, "Map Loader");
	}
}
