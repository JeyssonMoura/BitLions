using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempoPartida : MonoBehaviour {

	//Aux
	private SpawnMoedas AuxSpawnMoedas;

	public float tempo, maxTempo;

	void Start () {
		AuxSpawnMoedas = GameObject.Find ("LocaisInstMoedas").GetComponent<SpawnMoedas> ();
	}

	void Update () {
		if (PhotonNetwork.connectionStateDetailed == ClientState.Joined) {
			if (PhotonNetwork.isMasterClient) {
				if (tempo >= 0 && tempo <= maxTempo) {
					tempo += Time.deltaTime;
				} else {
					if (tempo >= maxTempo) {
						AuxSpawnMoedas.Spawnar ();
						tempo = -1;
					}
				}
				GetComponent<PhotonView> ().RPC ("EnviarTempo", PhotonTargets.All, tempo);
			}
		}
	}

	[PunRPC]
	public void EnviarTempo (float _tempo) {
		tempo = _tempo;
	}
}
