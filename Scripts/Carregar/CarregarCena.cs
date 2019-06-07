using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CarregarCena : MonoBehaviour {

	public string nomeCena;
	public Text progresso_Texto;
	private float carregando;

	public void CarregarNovaCena () {
		GetComponent<Canvas> ().enabled = true;
		StartCoroutine (Carregar (nomeCena));
	}

	IEnumerator Carregar (string NomeCenaAtual){
		AsyncOperation carregamento;
		carregamento = SceneManager.LoadSceneAsync (NomeCenaAtual);
		while (!carregamento.isDone) {
			carregando = (int)(carregamento.progress * 100.0f);
			progresso_Texto.text = "CARREGANDO... " + carregando + "%";
			yield return null;
		}
	}

}