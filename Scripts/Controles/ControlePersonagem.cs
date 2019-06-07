using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Photon.Pun;
using Photon.Realtime;

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

	void Start () {
		GetComponent<Rigidbody> ().isKinematic = false;
		AuxHUD = GameObject.Find ("HUD").GetComponent<HUD> ();
		AuxServer = GameObject.Find ("Server").GetComponent<Server> ();
		AuxClick = GameObject.Find ("HUD").GetComponent<Click> ();
		AuxCameraAlvo = GameObject.Find ("CameraPrincipal").GetComponent<CameraAlvo> ();
	}

	void Update () {
		//Animações
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
		if (GetComponent<PhotonView> ().IsMine == false) {
			transform.position = transform.position;
			transform.rotation = transform.rotation;
		}
	}

	void FixedUpdate () {

		if(PhotonNetwork.NetworkClientState == ClientState.Joined && GetComponent<PhotonView>().IsMine == false){
			return;
		}

		//Base Caiu
		Vector3 fwd = transform.TransformDirection ( -Vector3.up );

		if (!Physics.Raycast (transform.position, fwd, 0.5f)) {
			statusAnim = 0;
		} else {
			//Pular
			if (Input.GetKey (KeyCode.Space) || AuxClick.clickPular != 0) {
				//GetComponent<Rigidbody> ().AddForce (Vector3.up * forcaPulo);
				statusAnim = 3;
			} else {
				//Controles
				hM = CrossPlatformInputManager.GetAxis ("Horizontal");
				vM = CrossPlatformInputManager.GetAxis ("Vertical");

				hPC = Input.GetAxis ("Horizontal");
				vPC = Input.GetAxis ("Vertical");

				//GetComponent<Rigidbody> ().AddForce (Vector3.up * (forcaPulo)*-1);
			}
		}

		//Nick
		nick.text = "JOGADOR";
		GetComponent<PhotonView> ().RPC ("Nick", RpcTarget.All, "JOGADOR");

		//Movimentar
		if (vPC > 0.2f || vM > 0.2f || vPC < -0.2f || vM < -0.2f) {
			transform.Rotate (0, hM * Time.deltaTime * velH, 0);
			transform.Rotate (0, hPC * Time.deltaTime * velH, 0);
		}
		transform.Translate (0, 0, vM * Time.deltaTime * velV);
		transform.Translate (0, 0, vPC * Time.deltaTime * velV);

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

		//Tipo
		if (Masc1Femi2 == 1) {
			anim.SetInteger ("AnimPMasc", statusAnim);
		} else {
			anim.SetInteger ("AnimPFemi", statusAnim);
		}

		//Animação em Rede
		GetComponent<PhotonView> ().RPC ("AnimPersonagem", RpcTarget.All, statusAnim);
		GetComponent<PhotonView> ().RPC ("TipoPersonagem", RpcTarget.All, Masc1Femi2);

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
		transform.name = _nick + GetComponent<PhotonView> ().ViewID.ToString ();
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