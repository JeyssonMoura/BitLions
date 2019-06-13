using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour {

	private int minhasMoedas;

	public int getMinhasMoedas () {
		return minhasMoedas;
	}

	public void setMinhasMoedas (int _minhasMoedas) {
		minhasMoedas += _minhasMoedas;
	}

}