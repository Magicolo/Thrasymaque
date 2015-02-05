using System.Security.Cryptography;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Threading;
using System.Diagnostics;

public abstract class TiledMapLoader {
	
	protected int mapWidth;
	protected int mapHeight;
	protected Color32 backgroundColor = new Color32(0,0,0,0);
	
	protected Dictionary<Int32,Dictionary<String,String>> tilesetTiles;
	
	private XDocument document;
	private XElement map;
	
	
	public void loadFromFile(string fileName){
		string text = System.IO.File.ReadAllText(fileName);
		tilesetTiles = new Dictionary<int, Dictionary<string, string>>();
        loadLevel(text);
	}
	
	private void loadLevel(string levelFileContent){
		document = XDocument.Parse(levelFileContent);
		map = document.Element("map");
		loadMapAttributes();
        loadMapProperties();
        loadTilesets();
        loadLevelsLayers();
        loadLevelsObjectGroup();
        afterAll();
	}
	
	
	protected abstract void afterAll();

	
	void loadMapAttributes(){
		XElement mapElement = document.Elements().First();
		this.mapWidth  = this.parseInt(mapElement.Attribute("width").Value);
		this.mapHeight = this.parseInt(mapElement.Attribute("height").Value);
		if(mapElement.Attribute("backgroundcolor") != null){
			debugLog(mapElement.Attribute("backgroundcolor").Value);
		}
		afterMapAttributesLoaded();
	}
	protected abstract void afterMapAttributesLoaded();

	void loadTilesets(){
		var tileset = map.Element("tileset");
		foreach (var tile in tileset.Elements("tile")) {
			int id = this.parseInt(tile.Attribute("id").Value);
			
			Dictionary<string, string> dictionnary = makePropertiesDictionary(tile.Element("properties"));
			tilesetTiles.Add(id, dictionnary);
		}
	}
	
	private Dictionary<string, string> makePropertiesDictionary(XElement propertiesElement){
		Dictionary<string, string> properties = new Dictionary<string, string>();
		if(propertiesElement == null) return properties;
		
		foreach (var property in propertiesElement.Elements("property")) {
			string name = property.Attribute("name").Value;
			string value = property.Attribute("value").Value;
			properties.Add(name,value);
		}
		
		return properties;
	}
	
	
	void loadLevelsObjectGroup(){
		 var objGroups = document.Elements().Descendants().Where(e => e.Name == "objectgroup");
		 foreach (var objGroup in objGroups) {
		 	string name = objGroup.Attribute("name").Value;
		 	foreach (var obj in objGroup.Descendants().Where(e => e.Name == "object")) {
		 		loadObject(obj,name);
		 	}	
		 }
	}
	
	void loadObject(XElement obj, string objectGroupName){
		int x = Int32.Parse(obj.Attribute("x").Value);
		int y = Int32.Parse(obj.Attribute("y").Value);
		var propertiesElemens = obj.Descendants().First(e => e.Name == "properties").Descendants();
		Dictionary<string, string> properties = new Dictionary<string, string>();
		
		foreach (var property in propertiesElemens) {
			string name = property.Attribute("name").Value;
			string value = property.Attribute("value").Value;
			properties.Add(name,value);
		}
		
		addObject(objectGroupName,x,y,properties);
	}
	
	protected abstract void addObject(string objectGroupName, int x, int y, Dictionary<string, string> properties);
	
	#region loadMapLayerRegion
	
	void loadLevelsLayers(){
		 var layers = document.Elements().Descendants().Where(e => e.Name == "layer");
		 foreach (var layer in layers) {
		 	loadLayer(layer);
		 }
	}

	void loadLayer(XElement layer){
		int width = Int32.Parse(layer.Attribute("width").Value);
		int height = Int32.Parse(layer.Attribute("height").Value);
		
		createNewLayer(layer, width, height);
		loadLayerTiles(layer, height);
	}

	void createNewLayer(XElement layer, int width, int height){
		Dictionary<string, string> properties = new Dictionary<string, string>();
		
		if(layer.Elements("properties").Any() ){
			var propertiesElements = layer.Element("properties").Descendants();
			foreach (var property in propertiesElements) {
				string pname = property.Attribute("name").Value;
				string value = property.Attribute("value").Value;
				properties.Add(pname, value);
			}
		}
		
		string name = layer.Attribute("name").Value;
		addLayer(name, width, height, properties);
	}
	
	protected abstract void addLayer(string layerName, int width, int height, Dictionary<string, string> properties);

	void loadLayerTiles(XElement layer, int height){
		string tilesCSV = layer.Element("data").Value;
		string[] tilesLines = tilesCSV.Split(new string[] { "\n\r", "\r\n", "\n", "\r" }, StringSplitOptions.None);
		int y = height;
		for (int i = 1; i <= height; i++) {
			y--;
			loadLayerLine( y, tilesLines[i]);
		}
	}
	
	void loadLayerLine(int y, string tileLine){
		string[] tiles = tileLine.Split(new char[] { ',' }, StringSplitOptions.None);
		int x = 0;
		foreach (string tileId in tiles) {
			if(!tileId.Equals("0") && !tileId.Equals("") && tileId != null){
				int id = parseInt(tileId) - 1;
				addTile(x,y,id);
			}
			x++;
		}
	}

	protected abstract void addTile(int x, int y, int id);
	
	#endregion
	
	
	
	
	void loadMapProperties(){
		Dictionary<string, string> dictionnary = makePropertiesDictionary(map.Element("properties"));
		if(dictionnary.Count > 0){
			loadMapProperty(dictionnary);
		}
	}
	
	protected abstract void loadMapProperty(Dictionary<string, string> properties);
	
	
	
	
	
	protected int parseInt(string intStr){
		try{
			int id = Int32.Parse(intStr);
			return id;
		}catch (OverflowException){
			UnityEngine.Debug.LogError(intStr + " overflow the memory :(");
		}
		return -1;
	}
	
	private void debugLog(string log){
		UnityEngine.Debug.Log(log);
	}
}