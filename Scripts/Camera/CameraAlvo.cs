using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CameraAlvo : MonoBehaviour {

	//Aux
	private Server AuxServer;

	public GameObject Alvo;
	public float velRot, x, y, tempoClick;
	private Vector2 vecInput;

	void Start () {
		AuxServer = GameObject.Find("Server").GetComponent<Server> ();
	}

	void Update () {
		if (Alvo != null) {
			x = CrossPlatformInputManager.GetAxis ("Mouse X");
			y = CrossPlatformInputManager.GetAxis ("Mouse Y");
			vecInput += new Vector2 (x * velRot, -y * velRot);
			if (x != 0 && y != 0) {
				if (Input.GetKey (KeyCode.Mouse0)) {
					if (tempoClick >= 0) {
						tempoClick += Time.deltaTime;
					}
				}
				if (vecInput.y > 2) {
					if (vecInput.y < 90) {
						transform.localRotation = Quaternion.Euler (vecInput.y, vecInput.x, 0);
					} else {
						vecInput.y = 90;
					}
				} else {
					vecInput.y = 2;
				}
				transform.localPosition = Alvo.transform.position - (transform.localRotation * Vector3.forward * 5);
			} else {
				if (Alvo.GetComponent<ControlePersonagem> ()) {
					if (Alvo.GetComponent<ControlePersonagem> ().statusAnim != 0) {
						tempoClick = 0;
					}
					if (tempoClick == 0) {
						transform.position = Vector3.Lerp (transform.position, Alvo.transform.GetChild (0).transform.position, velRot * Time.deltaTime);
						Debug.DrawLine (Alvo.transform.position, Alvo.transform.GetChild (0).transform.position);
						transform.LookAt (Alvo.transform);
					}
				}
			}
		}

	}
}