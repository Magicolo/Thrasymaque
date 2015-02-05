using System.Threading;
using UnityEngine;
using System.Collections;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataGeneralSettings : ScriptableObject {
		
		[SerializeField, PropertyField(typeof(RangeAttribute), 0, 1)]
		float masterVolume = 1;
		public float MasterVolume {
			get {
				return masterVolume;
			}
			set {
				masterVolume = value;
				
				if (applicationPlaying) {
					SetMasterVolume(masterVolume);
				}
			}
		}
		
		[SerializeField, PropertyField(typeof(MinAttribute))]
		int maxVoices = 100;
		public int MaxVoices {
			get {
				return maxVoices;
			}
			set {
				if (!applicationPlaying) {
					maxVoices = value;
					pureData.sourceManager.UpdateSourceContainer();
				}
			}
		}

		[SerializeField, PropertyField]
		int sampleRate;
		public int SampleRate {
			get {
				return sampleRate;
			}
			set {
				sampleRate = value;
				AudioSettings.outputSampleRate = sampleRate;
			}
		}
		
		public string patchesPath = "Patches";
		public float speedOfSound = 343;
		public bool applicationPlaying;
		public Thread mainThread;
		public PureData pureData;

		public void Initialize(PureData pureData) {
			this.pureData = pureData;
			
			applicationPlaying = Application.isPlaying;
			mainThread = Thread.CurrentThread;
			SampleRate = sampleRate;
		}
		
		public void SetDefaultValues() {
			patchesPath = "Patches";
			maxVoices = 100;
			SampleRate = 44100;
			speedOfSound = 343;
			masterVolume = 1;
		}
		
		public void SetMasterVolume(float targetVolume, float time = 0, float delay = 0) {
			masterVolume = Mathf.Clamp01(targetVolume);
			time = Mathf.Max(time, 0.01F);
			delay = Mathf.Max(delay, 0);
			
			if (delay > 0) {
				pureData.communicator.Send("uresources_messagedelayer_ff", masterVolume, time * 1000, "umastervolume", delay * 1000);
			}
			else {
				pureData.communicator.Send("umastervolume", masterVolume, time * 1000);
			}
		}

		public bool IsMainThread() {
			return Thread.CurrentThread == mainThread;
		}
		
		public static void Switch(PureDataGeneralSettings source, PureDataGeneralSettings target) {
			source.MasterVolume = target.MasterVolume;
			source.MaxVoices = target.MaxVoices;
			source.patchesPath = target.patchesPath;
			source.speedOfSound = target.speedOfSound;
			
			source.Initialize(source.pureData);
		}
		
		public static PureDataGeneralSettings Create(string path) {
			PureDataGeneralSettings generalSettings = HelperFunctions.GetOrAddAssetOfType<PureDataGeneralSettings>("General", path);
			generalSettings.SetDefaultValues();
			return generalSettings;
		}
	}
}
