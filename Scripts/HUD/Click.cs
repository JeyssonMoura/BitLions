using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Click : MonoBehaviour {

	//Aux
	private HUD AuxHUD;
	private Server AuxServer;

	public int clickPular = 0;

	void Start () {
		if (Application.loadedLevelName != "Principal") {
			AuxServer = GameObject.Find ("Server").GetComponent<Server> ();
		}
		AuxHUD = GameObject.Find ("HUD").GetComponent<HUD> ();
	}
		
	void Update () {
		
	}

	public void ClickPular (int _clickPular) {
		clickPular = _clickPular;
	}

}