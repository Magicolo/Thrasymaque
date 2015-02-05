using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.AudioTools;


/// <summary>
/// Gives access to all the current settings of a PureDataItem.
/// </summary>
/// <remarks>These settings will be updated if they are changed from the PureDataItem.</remarks>
[System.Serializable]
public class PureDataItemInfo {

	public string Name {
		get {
			return info.Name;
		}
	}
	
	public string Path {
		get {
			return info.path;
		}
	}
	
	public int Samples {
		get {
			return info.samples;
		}
	}
	
	public int Frequency {
		get {
			return info.frequency;
		}
	}
	
	public int Channels {
		get {
			return info.channels;
		}
	}
	
	public float Length {
		get {
			return info.length;
		}
	}
	
	public bool LoadOnAwake {
		get {
			return info.loadOnAwake;
		}
	}
	
	public string Output {
		get {
			return info.output;
		}
	}
	
	public bool Fixed {
		get {
			return info.isFixed;
		}
	}
	
	public Vector2 PlayRange {
		get {
			return new Vector2(info.playRangeStart, info.playRangeEnd);
		}
	}
	
	public float Volume {
		get {
			return info.volume;
		}
	}
	
	public float Pitch {
		get {
			return info.pitch;
		}
	}
	
	public float RandomVolume {
		get {
			return info.randomVolume;
		}
	}
	
	public float RandomPitch {
		get {
			return info.randomPitch;
		}
	}
	
	public float FadeIn {
		get {
			return info.fadeIn;
		}
	}
	
	public float FadeOut {
		get {
			return info.fadeOut;
		}
	}
	
	public bool Loop {
		get {
			return info.loop;
		}
	}
	
	public float DopplerLevel {
		get {
			return info.dopplerLevel;
		}
	}
	
	public PureDataVolumeRolloffModes VolumeRolloffMode {
		get {
			return info.volumeRolloffMode;
		}
	}
	
	public float MinDistance {
		get {
			return info.minDistance;
		}
	}
	
	public float MaxDistance {
		get {
			return info.maxDistance;
		}
	}
	
	public float PanLevel {
		get {
			return info.panLevel;
		}
	}
	
	PureDataInfo info;
	
	public PureDataItemInfo(PureDataInfo info) {
		this.info = info;
	}
}

