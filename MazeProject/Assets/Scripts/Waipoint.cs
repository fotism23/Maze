using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waipoint : MonoBehaviour {

	void Start () {
		Color color = new Color(1.0f, 1.0f, .0f, 0.8f);
		GetComponent<Renderer>().material.color = color;
	}
}
