using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataMultipleItem : PureDataItem {

		string name;
		public override string Name {
			get {
				return name;
			}
		}

		PureDataStates state;
		public override PureDataStates State {
			get {
				state = (items.Count == 0 || items.TrueForAll(i => i.State == PureDataStates.Stopped)) ? PureDataStates.Stopped : state;
				return state;
			}
		}

		public override bool IsContainer {
			get {
				return true;
			}
		}

		public List<PureDataItem> items = new List<PureDataItem>();
		
		public PureDataMultipleItem(string name, PureData pureData)
			: base(pureData) {
			
			this.name = name;
		}
		
		public virtual void AddItem(PureDataItem item) {
			items.Add(item);
		}
		
		public virtual void RemoveItem(PureDataItem item) {
			items.Remove(item);
		}
		
		public virtual void ExecuteOnItems(System.Action<PureDataItem> action) {
			for (int i = items.Count - 1; i >= 0; i--) {
				PureDataItem item = items[i];
				
				if (item.State == PureDataStates.Stopped) {
					RemoveItem(item);
				}
				else {
					action(item);
				}
			}
		}

		public override void Play(float delay) {
			ExecuteOnItems(item => item.Play(delay));
		}

		public override void Pause(float delay) {
			ExecuteOnItems(item => item.Pause(delay));
		}

		public override void Stop(float delay) {
			ExecuteOnItems(item => item.Stop(delay));
		}

		public override void StopImmediate() {
			ExecuteOnItems(item => item.StopImmediate());
		}
		
		public override void SetVolume(float targetVolume, float time, float delay) {
			ExecuteOnItems(item => item.SetVolume(targetVolume, time, delay));
		}

		public override void SetPitch(float targetPitch, float time, float delay) {
			ExecuteOnItems(item => item.SetPitch(targetPitch, time, delay));
		}

		public override void Load() {
			ExecuteOnItems(item => item.Load());
		}
			
		public override void Unload() {
			ExecuteOnItems(item => item.Unload());
		}

		public override PureDataItem GetItem(int index, bool ignoreContainers) {
			return GetItems(ignoreContainers)[index];
		}
		
		public override PureDataItem GetItem(string name, bool ignoreContainers) {
			return System.Array.Find(GetItems(ignoreContainers), item => item.Name == name);
		}
		
		public override PureDataItem[] GetItems(bool ignoreContainers) {
			List<PureDataItem> itemList = new List<PureDataItem>();
			
			if (!ignoreContainers) {
				itemList.Add(this);
			}
			
			for (int i = 0; i < items.Count; i++) {
				itemList.AddRange(items[i].GetItems(ignoreContainers));
			}
			
			return itemList.ToArray();
		}
		
		public override PureDataItemInfo GetInfo() {
			return null;
		}
		
		public override void ApplyOptions(params PureDataOption[] options) {
			ExecuteOnItems(item => item.ApplyOptions(options));
		}
		
		public override void ApplyOption(PureDataOption option, float delay) {
			ExecuteOnItems(item => item.ApplyOption(option, delay));
		}
		
		public override string ToString() {
			return string.Format("{0}({1}, {2}, {3})", GetType().Name, Name, State, Logger.ObjectToString(items));
		}
	}
}
