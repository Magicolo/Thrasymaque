using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class RunnerIdle : State {
	
    Runner Layer {
    	get { return ((Runner)layer); }
    }
	
	public override void OnEnter() {
		SwitchState<RunnerRunning>();
	}
}
