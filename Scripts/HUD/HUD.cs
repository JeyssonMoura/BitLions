using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HUD : MonoBehaviour {

	//Aux 
	private Server AuxServer;
	private CarregarCena AuxCarregarCena;
	private TempoPartida AuxTempoPartida;

	public Text ranking, tempoPartida, statusConexao;
	public AudioClip audioClick, audioAviso;
	public GameObject[] boxHUD;

	void Start () {
		QualitySettings.SetQualityLevel (1);
		GetComponent<Canvas> ().enabled = false;
		GetComponent<GraphicRaycaster> ().enabled = false;
		AuxServer = GameObject.Find ("Server").GetComponent<Server> ();
		AuxTempoPartida = GameObject.Find ("TempoPartida").GetComponent<TempoPartida> ();
		AuxCarregarCena = GameObject.Find ("CarregarNovaCena").GetComponent<CarregarCena> ();
	}

	void Update () {
		//Conexão
		if (PhotonNetwork.connectionStateDetailed == ClientState.Joined) {
			GetComponent<Canvas> ().enabled = true;
			GetComponent<GraphicRaycaster> ().enabled = true;
		}

		if (AuxServer.meuPersonagem != null) {
			if (AuxServer.listaJogadores.Length > 1) {
				for (int i = 0; i < AuxServer.listaJogadores.Length - 1; i++) {
					if (AuxServer.listaJogadores [i].GetComponent<Inventario> ().getMinhasMoedas () >
					    AuxServer.listaJogadores [i + 1].GetComponent<Inventario> ().getMinhasMoedas ()) {
						ranking.text = "1º " + AuxServer.listaJogadores [i].GetComponent<ControlePersonagem> ().nick.text.ToString ()
						+ " - " + AuxServer.listaJogadores [i].GetComponent<Inventario> ().getMinhasMoedas () + "/10 Moedas\n";
					}
				}
			} else {
				if (AuxServer.listaJogadores.Length == 1) {
					ranking.text = "1º " + AuxServer.listaJogadores [0].GetComponent<ControlePersonagem> ().nick.text.ToString ()
					+ " - " + AuxServer.listaJogadores [0].GetComponent<Inventario> ().getMinhasMoedas () + "/10 Moedas\n";
				}
			}
		}

		if (AuxTempoPartida.tempo < AuxTempoPartida.maxTempo-1) {
			tempoPartida.text = Mathf.Round (AuxTempoPartida.maxTempo - AuxTempoPartida.tempo).ToString ();
		} else {
			tempoPartida.text = "VAI!";
		}

		if (AuxTempoPartida.tempo == -1) {
			tempoPartida.text = "";
		}

	}

	public void abrirBox (int idBox) {
		boxHUD [idBox].GetComponent<Canvas> ().enabled = true;
		if (boxHUD [idBox].GetComponent<GraphicRaycaster> () != null) {
			boxHUD [idBox].GetComponent<GraphicRaycaster> ().enabled = true;
		}
		if (idBox == 0) {
			StartCoroutine ("Avisos");	
		}
	}

	public void fecharBox (int idBox) {
		boxHUD [idBox].GetComponent<Canvas> ().enabled = false;
		if (boxHUD [idBox].GetComponent<GraphicRaycaster> () != null) {
			boxHUD [idBox].GetComponent<GraphicRaycaster> ().enabled = false;
		}
	}

	IEnumerator Avisos () {
		yield return new WaitForSeconds(3.5f);
		fecharBox (0);
	}

}