using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupItem : MonoBehaviour {

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "FPSController")
		{
			GameObject.Find("GameManager").SendMessage("PickHammer");
			Destroy(gameObject);
		}
	}
}
