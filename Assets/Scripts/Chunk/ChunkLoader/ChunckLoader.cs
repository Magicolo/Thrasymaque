using UnityEngine;
using System.Collections;

public class ChunckLoader : TiledMapLoader {

	public Chunk chunk;
	public GameObject parent;
	public Linker linker;
	
	public ChunckLoader(GameObject parent){
		this.parent = parent;
		
		findOrCreateLinker();
	}

	void findOrCreateLinker(){
		GameObject linkerObject = GameObject.Find("Linker");
		if(linkerObject == null){
			GameObject linkerPrefab = Resources.Load<GameObject>("Prefab/ChunkLoader/Linker");
			linkerObject = GameObjectExtend.createClone(linkerPrefab,"Linker", null ,Vector3.zero);
		}
		
		this.linker = linkerObject.GetComponent<Linker>();
	}
	
	protected override void afterAll(){}
	
	protected override void afterMapAttributesLoaded(){}
	
	protected override void addObject(string objectGroupName, int x, int y, System.Collections.Generic.Dictionary<string, string> properties){}
	
	protected override void addLayer(string layerName, int width, int height, System.Collections.Generic.Dictionary<string, string> properties){}
	
	protected override void addTile(int x, int y, int id){
		if(linker.prefabs.Count > id){
			Debug.LogError("Il manque des prefab dans le linker. Quesque tu as fais OLI!");
		}
	}
	
	


	protected override void loadMapProperty(System.Collections.Generic.Dictionary<string, string> properties){
		Debug.Log("ASDAS" + properties.Count);
	}
}
