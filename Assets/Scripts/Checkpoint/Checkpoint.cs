using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class Checkpoint : StateLayer {
	
	SpriteRenderer sprite;
	public SpriteRenderer Sprite {
		get {
			if (sprite == null){
				sprite = GetComponentInChildren<SpriteRenderer>();
			}
			return sprite;
		}
	}
}
