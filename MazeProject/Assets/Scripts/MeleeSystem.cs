using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class MeleeSystem : MonoBehaviour {

	private FirstPersonController fpc;

	void Start()
	{
		fpc = GameObject.FindObjectOfType<FirstPersonController>();
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Cube" && fpc.IsHitting())
		{
			GameObject.Find("GameManager").SendMessage("ReduceLife");
			col.gameObject.SendMessage("Destroy");
		}
	}
}
