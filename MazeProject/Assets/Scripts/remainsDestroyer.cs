using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class remainsDestroyer : MonoBehaviour {

	private void Start()
	{
		if (getRandom())
		{
			GameObject hammer = GameObject.Find("Pickup Hammer");
			//hammer.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			Instantiate(hammer, transform.position, transform.rotation);
		}
		
	}

	void Destroy()
	{
		Destroy(gameObject, 5);
	}

	private bool getRandom()
	{
		return (Random.value > 0.5f);
	}
}
