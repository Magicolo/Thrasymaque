using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;


[System.Serializable]
public class ChuckLoaderEditor : EditorWindow {

	public string filePath = "";
	
	void OnGUI(){	
		addFileLine();
		if (GUILayout.Button ("Load Map")) {
			string filename = Path.GetFileName(filePath).Split(new char[]{'.'})[0];
			GameObject tempParent = new GameObject(filename);
			
			ChunckLoader loader = new ChunckLoader(tempParent);
			loader.loadFromFile(filePath);
		}
	}

	/*GameObject addOrGetPrefab(){
		
		
		GameObject go = Resources.Load<GameObject>("Chunks/" + filename);
		
		Object prefab = EditorUtility.CreateEmptyPrefab("Assets/Temporary/"+t.gameObject.name+".prefab");
        EditorUtility.ReplacePrefab(t.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
		
		return null;
	}*/
	
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
