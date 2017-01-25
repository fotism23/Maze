using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


public class destructionController : MonoBehaviour {

	public GameObject remains;
	public int timesHitted;
	private FirstPersonController fpc;

	private void Start()
	{
		fpc = GameObject.FindObjectOfType<FirstPersonController>();
		timesHitted = 0;
	}

	void Destroy()
	{
		timesHitted++;
		if (timesHitted == 3)
		{
			Vector3 spawn = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
			GameObject rem = Instantiate(remains, spawn, transform.rotation);
			fpc.PlatDemolitionSound();
			rem.SendMessage("Destroy");
			Destroy(gameObject);
		}
		
	}
	
}
