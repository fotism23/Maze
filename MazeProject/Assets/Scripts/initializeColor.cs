using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initializeColor : MonoBehaviour {

	[SerializeField] private float color_r;
	[SerializeField] private float color_g;
	[SerializeField] private float color_b;

	private Color color;
	private Renderer rend;

	// Use this for initialization
	void Start () {
		color = new Color(color_r, color_g, color_b, 0.0f);
		rend = this.GetComponent<MeshRenderer>();
		rend.material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
		if (rend.material.color.a > 0)
		{
			color = rend.material.color;
			color.a -= 5.0f * Time.deltaTime;
			rend.material.color = color;
		}	
	}
	
}
