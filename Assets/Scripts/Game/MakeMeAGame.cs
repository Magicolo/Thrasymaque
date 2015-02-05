using UnityEngine;

[ExecuteInEditMode]
public class MakeMeAGame : MonoBehaviour {

	public bool hasBeenDone = false;
	
	void Update () {
		if(hasBeenDone) return;
		hasBeenDone = true;
		ProceduralGeneratorOfChunk pc 	= getMeOrCreate("ProceduralGeneratorOfChunk","Prefab/Game/ProceduralGeneratorOfChunk").GetComponent<ProceduralGeneratorOfChunk>();
		RunnerCamera rc = getMeOrCreate("Main Camera","Prefab/Main Camera").GetComponent<RunnerCamera>();
		Runner ru = getMeOrCreate("Runner","Prefab/Runner").GetComponent<Runner>();
		References gm = getMeOrCreate("GameManager","Prefab/GameManager").GetComponent<References>();
		getMeOrCreate("PureData","Prefab/PureData").GetComponent<PureData>();
		
		
		gm.runner = ru;
		gm.runnerCamera = rc;
		gm.proceduralGeneratorOfChunk = pc;
		pc.playersTransform = rc.transform;
		
		Object.DestroyImmediate(gameObject);
	}

	GameObject getMeOrCreate(string prefabName, string prefabPath){
		GameObject foundGO = GameObject.Find(prefabName);
		
		if(foundGO == null){
			GameObject prefab = Resources.Load<GameObject>(prefabPath);
			if(prefab == null){
				Debug.LogWarning("Developpeur noob, il manque le prefab de " + prefabName);
			}else{
				return GameObjectExtend.createClone(prefab);
			}
		}else{
			return foundGO;
		}
		return null;
	}
	
}
