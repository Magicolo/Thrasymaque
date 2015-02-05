using UnityEngine;
using System.Collections;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataSingleItem : PureDataItem {
		
		public override string Name {
			get {
				return audioSource.Name;
			}
		}
		
		public override PureDataStates State {
			get {
				return audioSource.State;
			}
		}

		public override bool IsContainer {
			get {
				return false;
			}
		}

		public PureDataSource audioSource;
				
		public PureDataSingleItem(PureDataSource audioSource, PureData pureData)
			: base(pureData) {
			
			this.audioSource = audioSource;
		}
		
		public override void Play(float delay) {
			audioSource.Play(delay);
		}

		public override void Pause(float delay) {
			audioSource.Pause(delay);
		}
		
		public override void Stop(float delay) {
			audioSource.Stop(delay);
		}

		public override void StopImmediate() {
			audioSource.StopImmediate();
		}
		
		public override void SetVolume(float targetVolume, float time, float delay) {
			audioSource.SetVolume(targetVolume, Mathf.Max(time, 0.01F), delay);
		}

		public override void SetPitch(float targetPitch, float time, float delay) {
			audioSource.SetPitch(targetPitch, time, delay);
		}

		public override void Load() {
			audioSource.Load();
		}
		
		public override void Unload() {
			audioSource.Unload();
		}

		public override PureDataItem GetItem(int index, bool ignoreContainers) {
			return this;
		}
		
		public override PureDataItem GetItem(string name, bool ignoreContainers) {
			return this;
		}
		
		public override PureDataItem[] GetItems(bool ignoreContainers) {
			return new []{ this };
		}

		public override PureDataItemInfo GetInfo() {
			return new PureDataItemInfo(audioSource.Info);
		}

		public override void ApplyOptions(params PureDataOption[] options) {
			audioSource.ApplyOptions(options);
		}
		
		public override void ApplyOption(PureDataOption option, float delay) {
			audioSource.ApplyOption(option, delay);
		}
		
		public override string ToString() {
			return string.Format("{0}({1}, {2})", GetType().Name, Name, State);
		}
	}
}
