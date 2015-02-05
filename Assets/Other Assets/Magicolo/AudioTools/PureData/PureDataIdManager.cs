using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Magicolo.GeneralTools;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataIdManager {

		public PureData pureData;
		
		readonly Dictionary<int, IIdentifiable> idIdentifiableDict = new Dictionary<int, IIdentifiable>();
		int idCounter;
		
		public PureDataIdManager(PureData pureData) {
			this.pureData = pureData;
		}
				
		public IIdentifiable GetIdentifiableWithId(int id) {
			return idIdentifiableDict.ContainsKey(id) ? idIdentifiableDict[id] : null;
		}
		
		public T GetIdentifiableWithId<T>(int id) where T : IIdentifiable {
			return (T)GetIdentifiableWithId(id);
		}
		
		public int GetUniqueId() {
			idCounter += 1;
			return idCounter;
		}
		
		public void SetUniqueId(IIdentifiable identifiable) {
			idCounter += 1;
			identifiable.Id = idCounter;
			idIdentifiableDict[idCounter] = identifiable;
		}
		
		public void AddId(int id, IIdentifiable identifiable) {
			idIdentifiableDict[id] = identifiable;
		}
		
		public void RemoveId(int id) {
			idIdentifiableDict.Remove(id);
		}
	}
}