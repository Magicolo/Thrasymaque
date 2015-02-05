using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using LibPDBinding;
using Magicolo.GeneralTools;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataBridge {
	
		public int sampleRate;
		public int bufferSize;
		public int bufferAmount;
		public bool initialized;
		public int ticks;
		public PureData pureData;
	
		public PureDataBridge(PureData pureData) {
			this.pureData = pureData;
		}
	
		void StartLibPD() {
			ResolvePath();
			SetAudioSettings();
			OpenAudio();
		}
	
		void StopLibPD() {
			initialized = false;
			LibPD.Release();
		}

		void OpenAudio() {
			if (LibPD.OpenAudio(2, 2, sampleRate) == 0) {
				initialized = true;
			}
			else {
				Logger.LogError("Failed to start LibPD.");
			}
		}

		void SetAudioSettings() {
			AudioSettings.GetDSPBufferSize(out bufferSize, out bufferAmount);
			sampleRate = AudioSettings.outputSampleRate;
			ticks = bufferSize / LibPD.BlockSize;
		}
	
		void ResolvePath() {
			#if !UNITY_WEBPLAYER
			string currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
			string dllPath = Application.dataPath + "/" + "Plugins";
			dllPath.Replace('/', Path.DirectorySeparatorChar);
		
			if (!currentPath.Contains(dllPath)) {
				Environment.SetEnvironmentVariable("PATH", currentPath + Path.PathSeparator + dllPath, EnvironmentVariableTarget.Process);
			}
			#endif
		}
	
		public void Start() {
			StartLibPD();
		}

		public void Stop() {
			StopLibPD();
		}
	}
}
