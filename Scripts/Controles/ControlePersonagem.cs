using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ControlePersonagem : MonoBehaviour {

	//Aux
	private Click AuxClick;
	private Server AuxServer;
	private TempoPartida AuxTempoPartida;

	public TextMesh nick;
	public Animator anim;
	public AudioSource audio;
	public AudioClip[] audios;
	public Vector3 scala;

	private float velH = 90, velV;
	public float hM, vM, hPC, vPC, forcaPulo, t;
	public int statusAnim = 0, Masc1Femi2;
	public bool noChao;

	void Start () {
		if(PhotonNetwork.connectionStateDetailed == ClientState.Joined && GetComponent<PhotonView>().isMine == false){
			return;
		}
		AuxClick = GameObject.Find ("HUD").GetComponent<Click> ();
		AuxServer = GameObject.Find ("Server").GetComponent<Server> ();
		AuxTempoPartida = GameObject.Find ("TempoPartida").GetComponent<TempoPartida> ();
	}

	void Update () {
		noChao = Physics.Raycast (transform.position, -Vector3.up, 0.5f);
		//Animações
		if (noChao) {
			if (vPC != 0 || vM != 0 || hPC != 0 || hM != 0) {
				if (statusAnim != 0) {
					audio.pitch = 1.5f;
					audio.clip = audios [0];
					if (!audio.isPlaying) {
						audio.Play ();
					}
				}
			} else {
				audio.Stop ();
			}
		} else {
			audio.Stop ();
		}
	}

	void FixedUpdate () {

		if(PhotonNetwork.connectionStateDetailed == ClientState.Joined && GetComponent<PhotonView>().isMine == false){
			return;
		}

		//Nick
		nick.text = "JOGADOR" + GetComponent<PhotonView>().viewID.ToString();
		GetComponent<PhotonView> ().RPC ("Nick", PhotonTargets.All, nick.text.ToString());

		//Movimentar
		if (AuxTempoPartida.tempo == -1) {
			//Controles
			hM = CrossPlatformInputManager.GetAxis ("Horizontal");
			vM = CrossPlatformInputManager.GetAxis ("Vertical");

			hPC = Input.GetAxis ("Horizontal");
			vPC = Input.GetAxis ("Vertical");
		} else {
			hM = 0;
			vM = 0;
			hPC = 0;
			vPC = 0;
		}
		transform.Rotate (0, hM * Time.deltaTime * velH, 0);
		transform.Rotate (0, hPC * Time.deltaTime * velH, 0);
		transform.Translate (0, 0, vM * Time.deltaTime * velV);
		transform.Translate (0, 0, vPC * Time.deltaTime * velV);

		//Velocidade
		if (vPC < -0.2f || vM < -0.2f) {
			velV = 1.5f;
		} else {
			velV = 7f;
		}

		if (noChao && statusAnim != 3) {
			//Parado
			if (vPC == 0 || vM == 0 || hPC == 0 || hM == 0) {
				statusAnim = 0;
			}
			//Correr
			if (vPC > 0.2f || vM > 0.2f) {
				statusAnim = 1;
			} else {
				if (vPC < -0.2f || vM < -0.2f) {
					statusAnim = 2;
				} else {
					statusAnim = 0;
				}
			}
		}
			
		//Pular
		if (Input.GetKeyDown(KeyCode.Space) || AuxClick.clickPular != 0) {
			if (noChao && t == 0) {
				statusAnim = 3;
				t = 1;
			}
			AuxClick.clickPular = 0;
		}
		if (Physics.Raycast (transform.position, -Vector3.up, 0.1f)) {
			GetComponent<Rigidbody> ().velocity = new Vector3 (0, -forcaPulo * 2, 0);
			anim.enabled = true;
		}
		if (!noChao && statusAnim != 3 && anim.enabled == true) {
			GetComponent<Rigidbody> ().velocity = new Vector3 (0, -forcaPulo * 2, 0);
		}
		if (t >= 1 && t <= 1.8f) {
			t += Time.deltaTime;
			if (t >= 1.4f) {
				GetComponent<Rigidbody> ().velocity = new Vector3 (0, forcaPulo, 0);
			}
		}
		if (t >= 1.8f) {
			anim.enabled = false;
			statusAnim = 0;
			t = 0;
		}

		//Tipo
		if (Masc1Femi2 == 1) {
			anim.SetInteger ("AnimPMasc", statusAnim);
		} else {
			anim.SetInteger ("AnimPFemi", statusAnim);
		}

		//Animação em Rede
		GetComponent<PhotonView> ().RPC ("AnimPersonagem", PhotonTargets.All, statusAnim);
		GetComponent<PhotonView> ().RPC ("TipoPersonagem", PhotonTargets.All, Masc1Femi2);

	}

	[PunRPC]
	public void AnimPersonagem (int _anim) {
		if (Masc1Femi2 == 1) {
			anim.SetInteger ("AnimPMasc", _anim);
		} else {
			anim.SetInteger ("AnimPFemi", _anim);
		}
	}

	[PunRPC]
	public void Nick (string _nick) {
		nick.text = _nick;
		transform.name = _nick + GetComponent<PhotonView> ().viewID.ToString();
	}

	[PunRPC]
	public void TipoPersonagem (int _tipo) {
		Masc1Femi2 = _tipo;
	}

}