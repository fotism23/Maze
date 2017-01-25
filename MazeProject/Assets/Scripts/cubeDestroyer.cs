using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeDestroyer : MonoBehaviour {

	void OnCollisionEnter(Collision col)
	{
		//Debug.Log(col.gameObject.tag);

		if (col.gameObject.tag == "Cube")
		{
			Debug.Log(col.gameObject.tag);
			col.gameObject.SendMessage("destroy");
			//Destroy(col.gameObject);
		}
	}
}
