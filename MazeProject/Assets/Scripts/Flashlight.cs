using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour {

	public Light HeadLight;
	public Light mainLightSource;
	
	private void Awake()
	{
		HeadLight = this.GetComponent<Light>();
		HeadLight.enabled = false;
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Tab) && !HeadLight.enabled)
		{
			mainLightSource.enabled = false;
			HeadLight.enabled = true;
		}else if (Input.GetKeyDown(KeyCode.Tab) && HeadLight.enabled)
		{
			mainLightSource.enabled = true;
			HeadLight.enabled = false;
		}

	}
}
