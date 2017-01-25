using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class initializeCameraPosition : MonoBehaviour {

	private int levels;
	private int size;

	public void Start()
	{
		readFile();
		moveCamera();

	}

	private void readFile()
	{
		try
		{
			string line;
			StreamReader theReader = new StreamReader("file.maz", Encoding.Default);
			using (theReader)
			{
				line = theReader.ReadLine();
				levels = Int32.Parse(line.Substring(2));

				line = theReader.ReadLine();

				size = Int32.Parse(line.Substring(2));
			}
		}
		catch (System.Exception e)
		{
			Debug.Log(e.Message);
		}
	}

	private void moveCamera()
	{
		transform.position = new Vector3(0.0f, (float)levels * 2, (float)-size);
		transform.Rotate(30.0f, 0.0f, 0.0f, Space.Self);
	}
}
