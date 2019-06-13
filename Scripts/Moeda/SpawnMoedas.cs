using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMoedas : MonoBehaviour {

	//Aux
	private Server AuxServer;

	public int localAleatorio;
	public GameObject moeda, moedaInst;
	public GameObject[] localInstMoeda01, localInstMoeda02, localInstMoeda03, listarMoedas;

	void Start () {
		AuxServer = GameObject.Find ("Server").GetComponent<Server> ();
	}

	void Update () {
		listarMoedas = GameObject.FindGameObjectsWithTag ("Moeda");
	}

	public void Spawnar () {
		if (PhotonNetwork.isMasterClient && PhotonNetwork.connectionStateDetailed == ClientState.Joined) {
			localAleatorio = Random.Range (1, 4);
			GetComponent<PhotonView> ().RPC ("EnviarLocalMoeda", PhotonTargets.All, localAleatorio);
		}
		if (localAleatorio != 0) {
			switch (localAleatorio) {
			case 1:
				for (int i = 0; i < localInstMoeda01.Length; i++) {
					moedaInst = (GameObject)Instantiate (moeda, localInstMoeda01 [i].transform.position, localInstMoeda01 [i].transform.rotation);
					moedaInst.transform.parent = localInstMoeda01 [i].transform;
				}
				break;
			case 2:
				for (int i = 0; i < localInstMoeda02.Length; i++) {
					moedaInst = (GameObject)Instantiate (moeda, localInstMoeda02 [i].transform.position, localInstMoeda02 [i].transform.rotation);
					moedaInst.transform.parent = localInstMoeda02 [i].transform;
				}
				break;
			case 3:
				for (int i = 0; i < localInstMoeda03.Length; i++) {
					moedaInst = (GameObject)Instantiate (moeda, localInstMoeda03 [i].transform.position, localInstMoeda03 [i].transform.rotation);
					moedaInst.transform.parent = localInstMoeda03 [i].transform;
				}
				break;
			}
		}
	}

	public void ResetarMoedas () {
		for (int i = 0; i < listarMoedas.Length; i++) {
			Destroy (listarMoedas[i]);
		}
	}

	[PunRPC]
	public void EnviarLocalMoeda (int _localAleatorio) {
		localAleatorio = _localAleatorio;
	}
}