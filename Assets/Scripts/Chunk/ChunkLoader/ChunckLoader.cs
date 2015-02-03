using System;
using UnityEngine;
using System.Collections;

public class ChunckLoader : TiledMapLoader {

	public Chunk chunk;
	public GameObject parent;
	private Transform tilesParent;
	public Linker linker;
	
	public ChunckLoader(GameObject parent){
		this.parent = parent;
		this.chunk = parent.AddComponent<Chunk>();
		this.chunk.gameObject.layer = LayerMask.NameToLayer("Chunk");
		tilesParent = GameObjectExtend.createGameObject("Tiles", parent.transform, Vector3.zero).transform;
	
		findOrCreateLinker();
	}

	void findOrCreateLinker(){
		GameObject linkerObject = GameObject.Find("Linker");
		if(linkerObject == null){
			GameObject linkerPrefab = Resources.Load<GameObject>("Prefab/Game/Linker");
			linkerObject = GameObjectExtend.createClone(linkerPrefab,"Linker", null ,Vector3.zero);
		}
		
		this.linker = linkerObject.GetComponent<Linker>();
	}
	
	protected override void afterAll(){}
	
	protected override void afterMapAttributesLoaded(){
		this.chunk.width = this.mapWidth;
		this.chunk.height = this.mapHeight;
		
		BoxCollider2D b2d = this.chunk.gameObject.AddComponent<BoxCollider2D>();
		b2d.isTrigger = true;
		b2d.center = new Vector2(this.chunk.width/2 - 0.5f,this.chunk.height/2 - 0.5f);
		b2d.size = new Vector2(this.chunk.width,this.chunk.height);
	}
	
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
		this.chunk.entreanceY = this.chunk.height - Int32.Parse(properties["MaxY"]) - 1;
		if(properties.ContainsKey("RightExitMaxY")){
			this.chunk.rightExitY = this.chunk.height - Int32.Parse(properties["RightExitMaxY"]) - 1;
		}
		if(properties.ContainsKey("UpExitMaxX")){
			this.chunk.upExitX = Int32.Parse(properties["UpExitMaxX"]);
			this.chunk.isStraight = false;
		}
		
	}
}
