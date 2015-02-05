using UnityEngine;
using System.Collections;
using Magicolo;
using Magicolo.AudioTools;

/// <summary>
/// Lets you override the settings of a sound set in the inspector.
/// </summary>
[System.Serializable]
public class PureDataOption {

	public enum OptionTypes {
		Volume,
		Pitch,
		RandomVolume,
		RandomPitch,
		FadeIn,
		FadeOut,
		Loop,
		Clip,
		Output,
		PlayRange,
		Time,
		DopplerLevel,
		VolumeRolloffMode,
		MinDistance,
		MaxDistance,
		PanLevel
	}
	
	public OptionTypes type;
	
	public bool IsFloat {
		get {
			return FloatTypes.Contains(type);
		}
	}
	
	public bool IsString {
		get {
			return StringTypes.Contains(type);
		}
	}
		
	public bool IsBool {
		get {
			return BoolTypes.Contains(type);
		}
	}
		
	public bool IsVector2 {
		get {
			return Vector2Types.Contains(type);
		}
	}
		
	public bool IsVolumeRolloffMode {
		get {
			return VolumeRolloffModeTypes.Contains(type);
		}
	}
		
	public bool IsClip {
		get {
			return ClipTypes.Contains(type);
		}
	}
	
	[SerializeField]
	float floatValue;
	
	[SerializeField]
	string stringValue;
	
	[SerializeField]
	bool boolValue;
	
	[SerializeField]
	Vector2 vector2Value;
	
	[SerializeField]
	PureDataVolumeRolloffModes volumeRolloffModeValue;
	
	[SerializeField]
	AudioClip clipValue;
	
	static readonly OptionTypes[] FloatTypes = { OptionTypes.FadeIn, OptionTypes.FadeOut, OptionTypes.Volume, OptionTypes.Pitch, OptionTypes.RandomVolume, OptionTypes.RandomPitch, OptionTypes.DopplerLevel, OptionTypes.MinDistance, OptionTypes.MaxDistance, OptionTypes.PanLevel, OptionTypes.Time };
	static readonly OptionTypes[] StringTypes = { OptionTypes.Output };
	static readonly OptionTypes[] BoolTypes = { OptionTypes.Loop };
	static readonly OptionTypes[] Vector2Types = { OptionTypes.PlayRange };
	static readonly OptionTypes[] VolumeRolloffModeTypes = { OptionTypes.VolumeRolloffMode };
	static readonly OptionTypes[] ClipTypes = { OptionTypes.Clip };
	
	PureDataOption(OptionTypes type, object value) {
		this.type = type;
		SetDefaultValue();
		SetValue(value);
	}
	
	public static PureDataOption Clip(AudioClip clip) {
		return new PureDataOption(OptionTypes.Clip, clip);
	}
	
	public static PureDataOption Output(string busName) {
		return new PureDataOption(OptionTypes.Output, busName);
	}
	
	public static PureDataOption FadeIn(float fadeIn) {
		return new PureDataOption(OptionTypes.FadeIn, fadeIn);
	}
	
	public static PureDataOption FadeOut(float fadeOut) {
		return new PureDataOption(OptionTypes.FadeOut, fadeOut);
	}
	
	public static PureDataOption Volume(float volume) {
		return new PureDataOption(OptionTypes.Volume, volume);
	}
	
	public static PureDataOption Pitch(float pitch) {
		return new PureDataOption(OptionTypes.Pitch, pitch);
	}
	
	public static PureDataOption RandomVolume(float randomVolume) {
		return new PureDataOption(OptionTypes.RandomVolume, randomVolume);
	}
	
	public static PureDataOption RandomPitch(float randomPitch) {
		return new PureDataOption(OptionTypes.RandomPitch, randomPitch);
	}
	
	public static PureDataOption Loop(bool loop) {
		return new PureDataOption(OptionTypes.Loop, loop);
	}
	
	public static PureDataOption DopplerLevel(float dopplerLevel) {
		return new PureDataOption(OptionTypes.DopplerLevel, dopplerLevel);
	}
	
	public static PureDataOption VolumeRolloffMode(PureDataVolumeRolloffModes volumeRolloffMode) {
		return new PureDataOption(OptionTypes.VolumeRolloffMode, volumeRolloffMode);
	}
	
	public static PureDataOption MinDistance(float minDistance) {
		return new PureDataOption(OptionTypes.MinDistance, minDistance);
	}
	
	public static PureDataOption MaxDistance(float maxDistance) {
		return new PureDataOption(OptionTypes.MaxDistance, maxDistance);
	}
	
	public static PureDataOption PanLevel(float panLevel) {
		return new PureDataOption(OptionTypes.PanLevel, panLevel);
	}
		
	public static PureDataOption PlayRange(float start, float end) {
		start = Mathf.Clamp(start, 0, Mathf.Min(end, 1));
		end = Mathf.Clamp(end, start, 1);
		return new PureDataOption(OptionTypes.PlayRange, new Vector2(start, end));
	}

	public static PureDataOption PlayRange(Vector2 range) {
		range.x = Mathf.Clamp(range.x, 0, Mathf.Min(range.y, 1));
		range.y = Mathf.Clamp(range.y, range.x, 1);
		return new PureDataOption(OptionTypes.PlayRange, range);
	}

	public static PureDataOption Time(float time) {
		return new PureDataOption(OptionTypes.Time, Mathf.Clamp01(time));
	}
	
	public string GetValueDisplayName() {
		if (type == OptionTypes.PlayRange) {
			Vector2 playRange = GetValue<Vector2>();
			return string.Format("Start: {0} | End: {1}", playRange.x.Round(0.001F), playRange.y.Round(0.001F));
		}
		
		return GetValue().ToString();
	}
	
	public T GetValue<T>() {
		return (T)GetValue();
	}
	
	public object GetValue() {
		if (IsFloat) {
			return floatValue;
		}
		if (IsString) {
			return stringValue;
		}
		if (IsBool) {
			return boolValue;
		}
		if (IsVector2) {
			return vector2Value;
		}
		if (IsVolumeRolloffMode) {
			return volumeRolloffModeValue;
		}
		if (IsClip) {
			return clipValue;
		}
		return null;
	}

	public void SetValue(object value) {
		if (value is float) {
			floatValue = (float)value;
		}
		else if (value is string) {
			stringValue = (string)value;
		}
		else if (value is bool) {
			boolValue = (bool)value;
		}
		else if (value is Vector2) {
			vector2Value = (Vector2)value;
		}
		else if (value is PureDataVolumeRolloffModes) {
			volumeRolloffModeValue = (PureDataVolumeRolloffModes)value;
		}
		else if (value is AudioClip) {
			clipValue = (AudioClip)value;
		}
	}
	
	public void SetDefaultValue() {
		floatValue = 0;
		stringValue = "";
		boolValue = false;
		vector2Value = new Vector2(0, 1);
		volumeRolloffModeValue = PureDataVolumeRolloffModes.Logarithmic;
		clipValue = null;
	}
	
	public void Apply(PureDataSource source, float delay) {
		switch (type) {
			case PureDataOption.OptionTypes.Volume:
				source.SetVolume(GetValue<float>(), 0, delay);
				break;
			case PureDataOption.OptionTypes.Pitch:
				source.SetPitch(GetValue<float>(), 0, delay);
				break;
			case PureDataOption.OptionTypes.RandomVolume:
				float randomVolume = GetValue<float>();
				source.SetVolume(source.Info.volume + source.Info.volume * HelperFunctions.RandomRange(-randomVolume, randomVolume), 0, delay);
				break;
			case PureDataOption.OptionTypes.RandomPitch:
				float randomPitch = GetValue<float>();
				source.SetPitch(source.Info.pitch + source.Info.pitch * HelperFunctions.RandomRange(-randomPitch, randomPitch), 0, delay);
				break;
			case PureDataOption.OptionTypes.FadeIn:
				source.SetFadeIn(GetValue<float>());
				break;
			case PureDataOption.OptionTypes.FadeOut:
				source.SetFadeOut(GetValue<float>());
				break;
			case PureDataOption.OptionTypes.Loop:
				source.SetLoop(GetValue<bool>(), delay);
				break;
			case PureDataOption.OptionTypes.Clip:
				source.SetClip(source.pureData.clipManager.GetClip(GetValue<AudioClip>().name));
				break;
			case PureDataOption.OptionTypes.Output:
				source.SetOutput(GetValue<string>(), delay);
				break;
			case PureDataOption.OptionTypes.PlayRange:
				Vector2 playRange = GetValue<Vector2>();
				source.SetPlayRange(playRange.x, playRange.y, delay, true);
				break;
			case PureDataOption.OptionTypes.Time:
				source.SetPhase(GetValue<float>(), delay);
				break;
			case PureDataOption.OptionTypes.DopplerLevel:
				source.DopplerLevel = GetValue<float>();
				break;
			case PureDataOption.OptionTypes.VolumeRolloffMode:
				source.VolumeRolloffMode = GetValue<PureDataVolumeRolloffModes>();
				break;
			case PureDataOption.OptionTypes.MinDistance:
				source.MinDistance = GetValue<float>();
				break;
			case PureDataOption.OptionTypes.MaxDistance:
				source.MaxDistance = GetValue<float>();
				break;
			case PureDataOption.OptionTypes.PanLevel:
				source.PanLevel = GetValue<float>();
				break;
		}
	}
	
	public override string ToString() {
		return string.Format("PureDataOption({0}, {1})", type, GetValue());
	}
}
