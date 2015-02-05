using UnityEngine;
using System.Collections;

public class PureDataExample : MonoBehaviour {

	PureDataItem item;
	
	[Button("Play Sound", "PlaySound", NoPrefixLabel = true, DisableOnStop = true)] public bool playSound;
	void PlaySound() {
		item = PureData.Play("Synth_Up");
	}
	
	[Button("PlayMono", "PlayMono", NoPrefixLabel = true, DisableOnStop = true)] public bool playMono;
	void PlayMono() {
		item = PureData.Play("PickupCoin");
	}
	
	[Button("Play48KHz", "Play48KHz", NoPrefixLabel = true, DisableOnStop = true)] public bool play48KHz;
	void Play48KHz() {
		item = PureData.Play("PianoD");
	}
	
	[Button("PlayLong", "PlayLong", NoPrefixLabel = true, DisableOnStop = true)] public bool playLong;
	void PlayLong() {
		item = PureData.Play("Synth_Chaotic");
	}
	
	[Button("Play Container", "PlayContainer", NoPrefixLabel = true, DisableOnStop = true)] public bool playContainer;
	void PlayContainer() {
		item = PureData.PlayContainer("Magic");
	}
	
	[Button("Stop Last", "StopLast", NoPrefixLabel = true, DisableOnStop = true)] public bool stopLast;
	void StopLast() {
		if (item != null) {
			item.Stop();
		}
	}
	
	[Button("Stop All Immediate", "StopAllImmediate", NoPrefixLabel = true, DisableOnStop = true)] public bool stopAll;
	void StopAllImmediate() {
		PureData.StopAllImmediate();
	}
	
	void Start() {
		PureData.OpenPatch("!example");
	}
	
	void OnGUI(){
		if (GUILayout.Button("Play Sound")){
			PlaySound();
		}
		
		if (GUILayout.Button("PlayMono")){
			PlayMono();
		}
		
		if (GUILayout.Button("Play48KHz")){
			Play48KHz();
		}
	}
}
