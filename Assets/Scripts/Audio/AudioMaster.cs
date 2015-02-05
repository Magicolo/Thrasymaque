using UnityEngine;
using System.Collections;

public class AudioMaster : MonoBehaviour {

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
	
	
	void Start() {
		PureData.OpenPatch("_Main");
		PureData.Send("Tempo", tempo);
	}
}
