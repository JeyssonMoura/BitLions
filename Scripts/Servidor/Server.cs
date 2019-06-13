using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Server : MonoBehaviour {

	//Aux
	private TempoPartida AuxTempoPartida;
	private SpawnMoedas AuxSpawnMoedas;
	private CameraAlvo AuxCameraAlvo;

	public string nomeServer;
	public int sexoPersonagem;
	public GameObject meuPersonagem;
	public GameObject[] localInstJogador, personagens, listaJogadores;

	void Start () {
		AuxCameraAlvo = GameObject.Find ("CameraPrincipal").GetComponent<CameraAlvo> ();
		AuxSpawnMoedas = GameObject.Find ("LocaisInstMoedas").GetComponent<SpawnMoedas> ();
		AuxTempoPartida = GameObject.Find ("TempoPartida").GetComponent<TempoPartida> ();
		if (Application.internetReachability != NetworkReachability.NotReachable) {
			Conectar ();
		} else {
			PhotonNetwork.offlineMode = true;
			Conectar ();
		}
	}

	void Update () {
		listaJogadores = GameObject.FindGameObjectsWithTag ("Player");
		if (PhotonNetwork.connectionStateDetailed == ClientState.Joined) {
			if (AuxTempoPartida.tempo == -1) {
				for (int i = 0; i < listaJogadores.Length; i++) {
					if (listaJogadores [i].GetComponent<Inventario> ().getMinhasMoedas () >= 10) {
						if (meuPersonagem != null) {
							meuPersonagem.GetComponent<Inventario> ().setMinhasMoedas (-meuPersonagem.GetComponent<Inventario> ().getMinhasMoedas ());
						}
						AuxSpawnMoedas.ResetarMoedas ();
						Respawn ();
						AuxTempoPartida.tempo = 0;
					}
				}
			}
			if (AuxTempoPartida.tempo != -1) {
				if (meuPersonagem == null) {
					meuPersonagem = PhotonNetwork.Instantiate (personagens [sexoPersonagem].transform.name.ToString (), localInstJogador [listaJogadores.Length].transform.position, localInstJogador [listaJogadores.Length].transform.rotation, 0);
					AuxCameraAlvo.Alvo = meuPersonagem;
					Respawn ();
				}
			} else {
				if (meuPersonagem == null && listaJogadores.Length > 0) {
					AuxCameraAlvo.Alvo = listaJogadores [Random.Range (0, listaJogadores.Length)];
				}
			}
		}
	}

	public void Respawn () {
		meuPersonagem.GetComponent<Rigidbody> ().isKinematic = true;
		meuPersonagem.transform.position = transform.position;
		meuPersonagem.transform.rotation = transform.rotation;
		meuPersonagem.GetComponent<Rigidbody> ().isKinematic = false;
		print ("Parado!");
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
		Debug.Log("Entrou na sala!");
	}

}