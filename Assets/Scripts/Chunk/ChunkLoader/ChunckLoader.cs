using UnityEngine;
using System.Collections;

public class ChunckLoader : TiledMapLoader {

	public Chunk chunk;
	public GameObject parent;
	private Transform tilesParent;
	public Linker linker;
	
	public ChunckLoader(GameObject parent){
		this.parent = parent;
		tilesParent = GameObjectExtend.createGameObject("Tiles", parent.transform, Vector3.zero).transform;
	
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
		if(id > linker.prefabs.Count){
			Debug.LogError("Il manque des prefab dans le linker."+ " : je ne connais pas id " + id + ". Quesque tu as fais OLI!");
		}else if (id != 0){
			GameObject prefab = linker.prefabs[id];
			if(prefab == null){
				Debug.LogWarning("Erreur au block " + x + "," + y + " : je ne connais pas id " + id + ". Quesque ta faite Õli!!");
			}else{
				Vector3 position = new Vector3(x,y,0);
				GameObjectExtend.createClone(prefab, prefab.name, tilesParent, position);
			}
			
		}
	}
	
	


	protected override void loadMapProperty(System.Collections.Generic.Dictionary<string, string> properties){
		Debug.Log("ASDAS" + properties.Count);
	}
}
