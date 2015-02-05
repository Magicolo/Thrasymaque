using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataSpatializerManager : ScriptableObject {

		public PureDataSpatializer[] spatializers = new PureDataSpatializer[0];
		public PureData pureData;
		
		Dictionary<string, PureDataSpatializer> nameSpatializerDict;
		
		public void Initialize(PureData pureData) {
			this.pureData = pureData;
			
			foreach (PureDataSpatializer spatializer in spatializers) {
				spatializer.Initialize(pureData);
			}
			
			if (Application.isPlaying) {
				BuildSpatializerDict();
			}
		}
		
		public void Update() {
			foreach (PureDataSpatializer spatializer in spatializers) {
				spatializer.Update();
			}
		}
		
		public void OnDrawGizmos() {
			foreach (PureDataSpatializer spatializer in spatializers) {
				spatializer.OnDrawGizmos();
			}
		}

		public PureDataSpatializer GetSpatializer(string spatializerName) {
			return nameSpatializerDict[spatializerName];
		}
		
		public void BuildSpatializerDict() {
			nameSpatializerDict = new Dictionary<string, PureDataSpatializer>();
			
			foreach (PureDataSpatializer spatializer in spatializers) {
				nameSpatializerDict[spatializer.Name] = spatializer;
			}
		}
				
		public static void Switch(PureDataSpatializerManager source, PureDataSpatializerManager target) {
			source.spatializers = target.spatializers;
			
			source.Initialize(source.pureData);
		}
		
		public static PureDataSpatializerManager Create(string path) {
			return HelperFunctions.GetOrAddAssetOfType<PureDataSpatializerManager>("Spatializers", path);
		}
	}
}
