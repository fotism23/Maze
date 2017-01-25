using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadingDestruction : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Animation anim = this.GetComponent<Animation>();
		anim.Play();
	}
}
