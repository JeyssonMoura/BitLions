using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HUD : MonoBehaviour {

	//Aux 
	private Click AuxClick;
	private Server AuxServer;
	private CarregarCena AuxCarregarCena;

	public Text statusConectar;
	public AudioClip audioClick, audioAviso;
	public GameObject[] boxHUD;

	void Start () {
		QualitySettings.SetQualityLevel (1);
		AuxCarregarCena = GameObject.Find ("CarregarNovaCena").GetComponent<CarregarCena> ();
		if (Application.loadedLevelName != "Principal") {
			AuxClick = GameObject.Find ("HUD").GetComponent<Click> ();
			AuxServer = GameObject.Find ("Server").GetComponent<Server> ();
			GetComponent<Canvas> ().enabled = false;
			GetComponent<GraphicRaycaster> ().enabled = false;
		}
	}

	void Update () {
		if (Application.loadedLevelName != "Principal") {
			//Conexão
			if (PhotonNetwork.connectionStateDetailed == ClientState.Joined) {
				GetComponent<Canvas> ().enabled = true;
				GetComponent<GraphicRaycaster> ().enabled = true;
			}
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
		//Fecha Botão de opções
		if (idBox == 5) {
			boxHUD [5].transform.GetChild (1).GetComponent<Canvas> ().enabled = false;
			boxHUD [5].transform.GetChild (1).GetComponent<GraphicRaycaster> ().enabled = false;
		}
	}

	IEnumerator Avisos () {
		yield return new WaitForSeconds(3.5f);
		fecharBox (0);
	}

}