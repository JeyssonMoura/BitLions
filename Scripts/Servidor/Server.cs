using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Server : MonoBehaviour {

	//Aux
	private HUD AuxHUD;
	private Click AuxClick;
	private CameraAlvo AuxCameraAlvo;

	public string nomeServer;
	public GameObject meuPersonagem;
	public GameObject[] personagens, listaJogadores;

	void Start () {
		AuxHUD = GameObject.Find ("HUD").GetComponent<HUD> ();
		AuxClick = GameObject.Find ("HUD").GetComponent<Click> ();
		AuxCameraAlvo = GameObject.Find ("CameraPrincipal").GetComponent<CameraAlvo> ();
		Conectar ();
	}

	void Update () {
		listaJogadores = GameObject.FindGameObjectsWithTag ("Player");
	}

	public void SpawnPersonagem () {
		if (PhotonNetwork.connectionStateDetailed == ClientState.Joined) {
			if (meuPersonagem == null) {
				meuPersonagem = PhotonNetwork.Instantiate (personagens [0].transform.name.ToString (), transform.position, transform.rotation, 0);
				AuxCameraAlvo.Alvo = meuPersonagem;
				Respawn ();
			}
			Debug.Log ("Conectado!");
		}
	}

	public void Respawn () {
		meuPersonagem.GetComponent<Rigidbody> ().isKinematic = true;
		meuPersonagem.transform.position = transform.position;
		meuPersonagem.transform.rotation = transform.rotation;
		meuPersonagem.GetComponent<Rigidbody> ().isKinematic = false;
	}

	public void Conectar() {
		PhotonNetwork.ConnectUsingSettings ("v1.0");
		Debug.Log ("Conectando...");
	}

	public virtual void OnConnectedToMaster() {
		Debug.Log("Conectando: Master");
		PhotonNetwork.JoinOrCreateRoom(nomeServer, new RoomOptions(){ isVisible = true, maxPlayers = 20 } ,null);
	}

	public virtual void OnJoinedLobby() {
		Debug.Log("Conectando: Lobby");
		PhotonNetwork.JoinOrCreateRoom(nomeServer, new RoomOptions(){ isVisible = true, maxPlayers = 20 } ,null);
	}

	public virtual void OnPhotonJoinRoomFailed() {
		Debug.Log("Falha ao criar a sala!");
		PhotonNetwork.CreateRoom(nomeServer, new RoomOptions(){ isVisible = true, maxPlayers = 20 } ,null);
	}

	public virtual void OnFailedToConnectToPhoton(DisconnectCause cause) {
		Debug.LogError("Erro: " + cause);
	}

	public void OnJoinedRoom() {
		SpawnPersonagem ();
		Debug.Log("Entrou na sala!");
	}

}