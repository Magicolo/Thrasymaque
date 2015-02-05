using UnityEngine;
using System.Collections;

public class AudioMaster : MonoBehaviour
{

	[SerializeField, PropertyField]
	float tempo = 120;
	public float Tempo {
		get {
			return tempo;
		}
		set {
			tempo = value;
			PureData.Send("Tempo", tempo);
		}
	}
	
	[Button("Test", "Test", NoPrefixLabel = true)] public bool test;
	void Test()
	{
		PlayNextAudioClip();
	}
	
	public static PureDataItem currentAudioClip;
	public static int currentAudioClipIndex = 1;
	public static bool patchOpened;
	
	const string audioClipPrefix = "raw_runner_voice-";
	
	public static void PlayNextAudioClip()
	{
		if (currentAudioClipIndex > 29) {
			return;
		}
		
		if (currentAudioClip != null){
			currentAudioClip.Stop();
		}
		
		currentAudioClipIndex += 1;
		currentAudioClip = PureData.Play(audioClipPrefix + (currentAudioClipIndex < 10 ? "0" + currentAudioClipIndex : currentAudioClipIndex.ToString()), PureDataOption.Output("Voice"));
	}
	
	void Awake()
	{
		if (!patchOpened) {
			PureData.OpenPatch("_Main");
			PureData.Send("Tempo", tempo);
			patchOpened = true;
		}
	}
	
	void OnDestroy(){
		if (currentAudioClip != null){
			currentAudioClip.Stop();
			currentAudioClipIndex -= 1;
		}
	}
}
