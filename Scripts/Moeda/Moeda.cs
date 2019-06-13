using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moeda : MonoBehaviour {

	//Aux
	private Server AuxServer;

	public AudioClip audioColetar;
	public bool status;

	void Start () {
		AuxServer = GameObject.Find ("Server").GetComponent<Server> ();
		status = true;
	}

	public void OnTriggerEnter (Collider Interacao) {
		if (Interacao.tag == "Player" && status == true) {
			AuxServer.meuPersonagem.GetComponent<Inventario> ().setMinhasMoedas (1);
			GetComponent<BoxCollider> ().enabled = false;
			GetComponent<MeshRenderer> ().enabled = false;
			if (!GetComponent<AudioSource> ().isPlaying) {
				GetComponent<AudioSource> ().clip = audioColetar;
				GetComponent<AudioSource> ().Play ();
			}
			status = false;
		}
	}

}
