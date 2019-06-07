using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Server : MonoBehaviourPunCallbacks {

	//Aux
	private HUD AuxHUD;
	private Click AuxClick;
	private CameraAlvo AuxCameraAlvo;

	public string statusConexao, nomeServer;
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
		if (PhotonNetwork.NetworkClientState == ClientState.Joined) {
			if (meuPersonagem == null) {
				meuPersonagem = PhotonNetwork.Instantiate (personagens [0].transform.name.ToString (), transform.position, transform.rotation, 0);
				AuxCameraAlvo.Alvo = meuPersonagem;
				Respawn ();
			}
		}
	}

	public void Respawn () {
		meuPersonagem.GetComponent<Rigidbody> ().isKinematic = true;
		meuPersonagem.transform.position = transform.position;
		meuPersonagem.transform.rotation = transform.rotation;
		meuPersonagem.GetComponent<Rigidbody> ().isKinematic = false;
	}

	public void Conectar() {
		PhotonNetwork.ConnectUsingSettings ();
		PhotonNetwork.GameVersion = "v1.0";
		statusConexao = "Conectando...";
		Debug.Log ("Conectando...");
	}

	public override void OnConnectedToMaster() {
		Debug.Log("Conectando ao Master...");
		PhotonNetwork.JoinRoom (nomeServer);
	}

	public override void OnJoinedLobby() {
		Debug.Log("Entrou no Lobby...");
		PhotonNetwork.JoinRoom (nomeServer);
	}

	public override void OnJoinRoomFailed(short returnCode, string message) {
		Debug.Log("Criando sala...");
		PhotonNetwork.CreateRoom(nomeServer, new RoomOptions() { MaxPlayers = 10 }, null);
	}
		
	public override void OnDisconnected(DisconnectCause cause) {
		statusConexao = "Problema de conexão!";
		Debug.Log("Error:("+cause+")");
	}

	public override void OnJoinedRoom() {
		statusConexao = "Conectado!";
		SpawnPersonagem ();
		Debug.Log("Entrou na sala!");
	}
}
