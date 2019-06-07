using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ControlePersonagem : MonoBehaviour {

	//Aux
	private HUD AuxHUD;
	private Click AuxClick;
	private Server AuxServer;
	private CameraAlvo AuxCameraAlvo;

	public TextMesh nick;
	public Animator anim;
	public AudioSource audio;
	public AudioClip[] audios;
	public Vector3 scala;

	private float velH = 90, velV = 5f;
	public float hM, vM, hPC, vPC, forcaPulo, t;
	public int statusAnim = 0, Masc1Femi2 = 0;
	public bool noChao;

	void Start () {
		GetComponent<Rigidbody> ().isKinematic = false;
		AuxHUD = GameObject.Find ("HUD").GetComponent<HUD> ();
		AuxServer = GameObject.Find ("Server").GetComponent<Server> ();
		AuxClick = GameObject.Find ("HUD").GetComponent<Click> ();
		AuxCameraAlvo = GameObject.Find ("CameraPrincipal").GetComponent<CameraAlvo> ();
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
		if (GetComponent<PhotonView> ().isMine == false) {
			transform.position = transform.position;
			transform.rotation = transform.rotation;
		}
	}

	void FixedUpdate () {

		if(PhotonNetwork.connectionStateDetailed == ClientState.Joined && GetComponent<PhotonView>().isMine == false){
			return;
		}

		if (Physics.Raycast (transform.position, -Vector3.up, 0.1f)) {
			GetComponent<Rigidbody> ().velocity = new Vector3 (0, -forcaPulo*2, 0);
			anim.enabled = true;
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

		//Pular
		if (Input.GetButtonDown ("Jump") || AuxClick.clickPular != 0) {
			if (noChao && t == 0) {
				statusAnim = 3;
				t = 1;
			}
			AuxClick.clickPular = 0;
		}

		//Controles
		hM = CrossPlatformInputManager.GetAxis ("Horizontal");
		vM = CrossPlatformInputManager.GetAxis ("Vertical");

		hPC = Input.GetAxis ("Horizontal");
		vPC = Input.GetAxis ("Vertical");

		//Nick
		nick.text = "JOGADOR";
		GetComponent<PhotonView> ().RPC ("Nick", PhotonTargets.All, "JOGADOR");

		//Movimentar
		if (vPC > 0.2f || vM > 0.2f || vPC < -0.2f || vM < -0.2f) {
			transform.Rotate (0, hM * Time.deltaTime * velH, 0);
			transform.Rotate (0, hPC * Time.deltaTime * velH, 0);
		}
		transform.Translate (0, 0, vM * Time.deltaTime * velV);
		transform.Translate (0, 0, vPC * Time.deltaTime * velV);

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

	[PunRPC]
	public void ScalaPersonagem (Vector3 _scala) {
		transform.localScale = _scala;
	}

	[PunRPC]
	public void PosPersonagem (Vector3 _pos) {
		transform.localPosition = _pos;
	}

}