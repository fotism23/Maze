using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
	private int levels;
	private int size;
	private int hammers;
	private Text m_text;
	private bool hammersChanged = false;
	private int hammerHealth = 100;
	private int hammersUsed = 0;
	private Animator anim;
	private bool canHit = true;
	private int cameraMode = 0;
	private string[,,] mazeMap;
	private ArrayList emptyBoxes;
	private FirstPersonController fpc;
	private Camera secondaryCamera;
	private Camera mainCamera;
	private float timeStarted;
	private Text score_text;
	private bool orbitCamera = false;
	private bool transparent = false;
	private bool stopUpdate = false;
	private bool topLevel = false;
	private bool forceGameOver = false;
	private bool exit = false;
	private float exitTime;
	private GameObject wayPoint;
	private Vector3 wayPointPos;

	void Start()
	{
		emptyBoxes = new ArrayList();
		ReadMazFile();
		CreateBounds();

		m_text = GameObject.Find("AvailableHammers").GetComponent<Text>();
		m_text.text = hammers.ToString();

		score_text = GameObject.Find("Score").GetComponent<Text>();

		anim = GameObject.Find("FPSController").GetComponent<Animator>();

		System.Random r = new System.Random();
		int rInt = r.Next(0, emptyBoxes.Count);
		Vector3 randomPoint = (Vector3)emptyBoxes[rInt];

		randomPoint.x = ((randomPoint.x + (size / 2)) / (size / 2));
		randomPoint.z = ((randomPoint.z + (size / 2)) / (size / 2));

		randomPoint.y += .5f;
		GameObject.Find("FPSController").transform.position = randomPoint;

		fpc = GameObject.FindObjectOfType<FirstPersonController>();

		secondaryCamera = GameObject.FindGameObjectWithTag("SecondaryCamera").GetComponent<Camera>();
		InitializeSecondaryCamera();
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		fpc.enabled = false;
		canHit = false;
		secondaryCamera.enabled = true;
		mainCamera.enabled = false;
		DestroyObjects();

		wayPoint = GameObject.Find("Waypoint");
		wayPointPos.y = levels + 1;
		timeStarted = Time.realtimeSinceStartup;

	}

	private void Update()
	{

		if (GameOver())
		{
			ShowGameOverPanel();
		}
		else
		{
			if (Input.GetButtonDown("Fire1") && canHit)
			{
				anim.SetTrigger("Bash");
			}

			if (hammersChanged)
			{
				m_text.text = hammers.ToString();
				hammersChanged = false;
			}

			if (Input.GetKeyDown(KeyCode.V))
			{
				SwitchCamera();
			}

			if (Input.GetKeyDown(KeyCode.E) && topLevel)
			{
				forceGameOver = true;
				ShowGameOverPanel();
			}

			if (Input.GetKeyDown(KeyCode.T))
			{
				SwitchTrasparency();
			}

			if (Input.GetKeyDown(KeyCode.R))
			{
				SwitchOrbitMode();
			}

			if (Input.GetKeyDown(KeyCode.X))
			{
				ExitGame();
			}

			if (orbitCamera) secondaryCamera.transform.RotateAround(new Vector3(0, levels, 0), Vector3.up, -30 * Time.deltaTime);
		}

		if (exit)
		{
			if (Time.realtimeSinceStartup - exitTime >= 5.0f)
			{
#if UNITY_STANDALONE
				Application.Quit();
#endif
#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
#endif
			}
		}

		if (secondaryCamera)
		{
			wayPoint.transform.position = new Vector3(wayPointPos.x, wayPointPos.y + Mathf.Sin(Time.time * 3), wayPointPos.z);
		}

	}

	private void ExitGame()
	{
		ShowGameOverPanel();
		GameObject.Find("PlayAgainButton").SetActive(false);
		exitTime = Time.realtimeSinceStartup;
		exit = true;
	}


	private void SwitchTrasparency()
	{
		if (transparent)
		{
			foreach (GameObject c in GameObject.FindGameObjectsWithTag("Cube"))
			{
				Color col = c.GetComponent<MeshRenderer>().material.color;
				col.a = 1.0f;
				c.GetComponent<MeshRenderer>().material.color = col;
			}
		}
		else
		{
			foreach (GameObject c in GameObject.FindGameObjectsWithTag("Cube"))
			{
				Color col = c.GetComponent<MeshRenderer>().material.color;
				col.a = 0.8f;
				c.GetComponent<MeshRenderer>().material.color = col;
			}
		}
		transparent = !transparent;
	}

	private void SwitchOrbitMode()
	{
		if (secondaryCamera.enabled == true)
		{
			if (orbitCamera == false)
			{
				secondaryCamera.transform.rotation = Quaternion.Euler(35, 90, 0);
				Vector3 pos = secondaryCamera.transform.position;
				pos.x -= size;
				pos.y = levels * 3;
				secondaryCamera.transform.position = pos;
			}
			else
			{
				InitializeSecondaryCamera();
			}
			orbitCamera = !orbitCamera;
		}
	}

	private void CreateCube(float positionX, float positionY, float positionZ, string _texture)
	{
		GameObject cube = null;

		Vector3 spawnLocation = new Vector3(positionX - size / 2.0f + 0.5f, positionY + 0.5f, positionZ - size / 2.0f + 0.5f);

		if (_texture.Equals("T1"))
		{
			cube = GameObject.Find("CubeT1");
		}
		else if (_texture.Equals("T2"))
		{
			cube = GameObject.Find("CubeT2");
		}
		else if (_texture.Equals("T3"))
		{
			cube = GameObject.Find("CubeT3");
		}
		else if (_texture.Equals("R"))
		{
			cube = GameObject.Find("CubeR");
		}
		else if (_texture.Equals("G"))
		{
			cube = GameObject.Find("CubeG");
		}
		else if (_texture.Equals("B"))
		{
			cube = GameObject.Find("CubeB");
		}
		else if (_texture.Equals("W"))
		{
			cube = GameObject.Find("CubeW");
		}
		else if (_texture.Equals("E"))
		{
			mazeMap[(int)positionX, (int)positionY, (int)positionZ] = "E";
			Vector3 vec = new Vector3(positionX, positionY, positionZ);
			if (positionY < 1) emptyBoxes.Add(vec);
			return;
		}
		else
		{
			return;
		}
		mazeMap[(int)positionX, (int)positionY, (int)positionZ] = _texture;
		Instantiate(cube, spawnLocation, transform.rotation);
		cube.transform.position = spawnLocation;
	}

	private void CreateCubes(string[] entries, int _row, int level)
	{
		for (var i = 0; i < size; i++)
		{
			CreateCube((float)i, (float)level, (float)_row, entries[i]);
		}
	}

	private void CreateBounds()
	{
		GameObject cube_one = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube_one.transform.position = new Vector3(size, 0, 0);
		cube_one.transform.localScale = new Vector3(size, levels, size);
		cube_one.AddComponent<BoxCollider>();
		Vector3 colliderCenter = cube_one.GetComponent<BoxCollider>().center;
		colliderCenter.y += levels / 2;
		cube_one.GetComponent<BoxCollider>().center = colliderCenter;
		Vector3 colliderSize = cube_one.GetComponent<BoxCollider>().size;
		colliderSize.y = levels;
		cube_one.GetComponent<BoxCollider>().size = colliderSize;
		cube_one.GetComponent<MeshRenderer>().enabled = false;

		GameObject cube_two = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube_two.transform.position = new Vector3(0, 0, size);
		cube_two.transform.localScale = new Vector3(size, levels, size);
		cube_two.AddComponent<BoxCollider>();
		colliderCenter = cube_two.GetComponent<BoxCollider>().center;
		colliderCenter.y += levels / 2;
		cube_two.GetComponent<BoxCollider>().center = colliderCenter;
		colliderSize = cube_two.GetComponent<BoxCollider>().size;
		colliderSize.y = levels;
		cube_two.GetComponent<BoxCollider>().size = colliderSize;
		cube_two.GetComponent<MeshRenderer>().enabled = false;

		GameObject cube_three = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube_three.transform.position = new Vector3(0, 0, -size);
		cube_three.transform.localScale = new Vector3(size, levels, size);
		cube_three.AddComponent<BoxCollider>();
		colliderCenter = cube_three.GetComponent<BoxCollider>().center;
		colliderCenter.y += levels / 2;
		cube_three.GetComponent<BoxCollider>().center = colliderCenter;
		colliderSize = cube_three.GetComponent<BoxCollider>().size;
		colliderSize.y = levels;
		cube_three.GetComponent<BoxCollider>().size = colliderSize;
		cube_three.GetComponent<MeshRenderer>().enabled = false;

		GameObject cube_four = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube_four.transform.position = new Vector3(-size, 0, 0);
		cube_four.transform.localScale = new Vector3(size, levels, size);
		cube_four.AddComponent<BoxCollider>();
		colliderCenter = cube_four.GetComponent<BoxCollider>().center;
		colliderCenter.y += levels / 2;
		cube_four.GetComponent<BoxCollider>().center = colliderCenter;
		colliderSize = cube_four.GetComponent<BoxCollider>().size;
		colliderSize.y = levels;
		cube_four.GetComponent<BoxCollider>().size = colliderSize;
		cube_four.GetComponent<MeshRenderer>().enabled = false;
	}

	private void ReadMazFile()
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

				mazeMap = new string[size, levels, size];

				line = theReader.ReadLine();
				hammers = Int32.Parse(line.Substring(2));


				line = theReader.ReadLine();

				if (!line.Substring(0, 5).Equals("LEVEL")) return;

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
		}
		catch (System.Exception e)
		{
			Debug.Log(e.Message);
		}
	}

	private void SwitchCamera()
	{
		wayPoint.GetComponent<MeshRenderer>().enabled = !wayPoint.GetComponent<MeshRenderer>().enabled;

		wayPointPos = fpc.transform.position;
		wayPointPos.y = levels + 1;
		wayPoint.transform.position = wayPointPos;

		canHit = !canHit;
		fpc.enabled = !fpc.enabled;
		secondaryCamera.enabled = !secondaryCamera.enabled;
		mainCamera.enabled = !mainCamera.enabled;
		Vector3 camPos = secondaryCamera.transform.position;
		camPos.y += 9;
		camPos.y += levels;
		cameraMode++;
	}

	private void InitializeSecondaryCamera()
	{
		Vector3 camPos = secondaryCamera.transform.position;
		camPos.x = 0;
		camPos.z = 0;
		camPos.y = 27 + levels;
		secondaryCamera.transform.position = camPos;
		secondaryCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
	}

	private int CalculateScore()
	{
		int score;
		score = (int)Math.Pow(size, 2);
		score -= (int)Time.realtimeSinceStartup - (int)timeStarted;
		score -= hammersUsed * 50;
		if (score < 0) score = 0;
		return score;
	}

	private int UpdateScore()
	{
		int score = CalculateScore();
		score_text.text = score.ToString();
		return score;
	}

	private bool GameOver()
	{
		if (UpdateScore() <= 0) return true;
		if (forceGameOver) return true;
		if (fpc.transform.position.y >= levels + 0.9)
		{
			topLevel = true;
		}
		return false;
	}

	private void ShowGameOverPanel()
	{
		if (!stopUpdate)
		{
			fpc.enabled = false;
			GameObject.Find("HUD").GetComponent<Canvas>().enabled = false;
			GameObject.Find("GameOverPanel").GetComponent<Canvas>().enabled = true;
			GameObject.Find("PointsText").GetComponent<Text>().text = CalculateScore().ToString();
			Debug.Log("Game Over");
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		stopUpdate = true;
	}

	private void DestroyObjects()
	{
		Destroy(GameObject.Find("CubeR"));
		Destroy(GameObject.Find("CubeG"));
		Destroy(GameObject.Find("CubeB"));
		Destroy(GameObject.Find("CubeE"));
		Destroy(GameObject.Find("CubeT1"));
		Destroy(GameObject.Find("CubeT2"));
		Destroy(GameObject.Find("CubeT3"));
	}

	public void ReduceLife()
	{
		fpc.PlayMeleeSound();

		hammerHealth -= 10;
		if (hammerHealth <= 10 && hammers > 0)
		{
			hammers--;
			hammersUsed++;
			hammerHealth = 100;
		}

		if (hammers <= 0 && hammerHealth <= 0)
		{
			canHit = false;
		}
		hammersChanged = true;
	}

	public void PickHammer()
	{
		hammers++;
		hammersChanged = true;
	}

	public void PlayAgain()
	{
		Debug.Log("Clicked");
		SceneManager.LoadScene("main_scene");
	}
}

