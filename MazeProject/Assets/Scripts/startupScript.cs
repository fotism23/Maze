using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System;
using System.Linq;

public class startupScript : MonoBehaviour {
    int levels;
    int size;

    void Start() {
		ReadMazFile();
	}

    void CreateCube(float positionX, float positionY, float positionZ, string _texture) {

		Vector3 spawnLocation;
		GameObject cube;

		spawnLocation = new Vector3(positionX - size / 2.0f + 0.5f, positionY + 0.5f, positionZ - size / 2.0f + 0.5f);
		
		if (_texture.Equals("T1"))
		{
			cube = GameObject.Find("CubeT1");
			Instantiate(cube, spawnLocation, transform.rotation);
		}else if (_texture.Equals("T2"))
		{
			cube = GameObject.Find("CubeT2");
			Instantiate(cube, spawnLocation, transform.rotation);
		}else if (_texture.Equals("T3"))
		{
			cube = GameObject.Find("CubeT3");
			Instantiate(cube, spawnLocation, transform.rotation);
		}else if (_texture.Equals("R"))
		{
			cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			Renderer rend = cube.GetComponent<Renderer>();
			rend.material.color = new Color(1.0f, 0.0f, 0.0f);
		}else if (_texture.Equals("G"))
		{
			cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			Renderer rend = cube.GetComponent<Renderer>();
			rend.material.color = new Color(0.0f, 1.0f, 0.0f);
		}
		else if (_texture.Equals("B"))
		{
			cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			Renderer rend = cube.GetComponent<Renderer>();
			rend.material.color = new Color(0.0f, 0.0f, 1.0f);
		}
		else if (_texture.Equals("W"))
		{
			cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			Renderer rend = cube.GetComponent<Renderer>();
			rend.material.color = new Color(0.0f, 0.0f, 0.0f);
		}
		else
		{
			return;
		}
		cube.transform.position = spawnLocation;
	}

    void CreateCubes(string[] entries, int _row, int level)
    {
		for (var i = 0; i < size; i++)
		{
			CreateCube((float)i, (float)level, (float)_row, entries[i]);
		}
    }

    void ReadMazFile() {
        try {
            string line;
            StreamReader theReader = new StreamReader("file.maz", Encoding.Default);
			using (theReader)
			{
				line = theReader.ReadLine();
				levels = Int32.Parse(line.Substring(2));

				line = theReader.ReadLine();
			
				size = Int32.Parse(line.Substring(2));

				line = theReader.ReadLine();
				
				line = theReader.ReadLine();
				
				if (!line.Substring(0,5).Equals("LEVEL")) return;

				for (int i = 0; i < levels; i++)
				{
					for (int j = 0; j < size; j++)
					{
						line = theReader.ReadLine();
						string[] entries = line.Split(null);
						if (entries.Length > 0)
						{
							CreateCubes(entries, j, i);
						}
					}
					line = theReader.ReadLine();
				}
			}
        } catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
