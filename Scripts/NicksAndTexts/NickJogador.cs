using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NickJogador : MonoBehaviour {

	void Update () {
		transform.LookAt (Camera.main.transform);
	}
}