using UnityEngine;
using UnityEngine.UI;  // add to the top
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class Teleport : MonoBehaviour
{
	float disableTimer = 0;
	public CanvasGroup myCG;
	private bool flash = false;
	private FirstPersonController fpc;

	void Start()
	{
		fpc = GameObject.FindObjectOfType<FirstPersonController>();
		myCG = GameObject.Find("Flash").GetComponent<CanvasGroup>();
		myCG.alpha = 0;
	}


	void Update()
	{

		if (disableTimer > 0) disableTimer -= Time.deltaTime;

		if (flash)
		{
			myCG.alpha = myCG.alpha - Time.deltaTime;
			if (myCG.alpha <= 0)
			{
				myCG.alpha = 0;
				flash = false;
			}
		}
	}
	
	
	private void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "FPSController" && disableTimer <= 0)
		{

			Teleport[] tpList = new Teleport[20];
			int count = 0;

			foreach (Teleport tp in FindObjectsOfType<Teleport>())
			{
				if (Mathf.Abs(tp.transform.position.y - fpc.transform.position.y) < 1 && tp != this)
				{
					tpList[count] = tp;
					count++;
					if (count == 19) break;
				}
			}

			System.Random r = new System.Random();
			int rInt = r.Next(0, count);

			if (count > 0)
			{
				flash = true;
				myCG.alpha = 1;
				fpc.PlayTeleportSound();
				tpList[rInt].disableTimer = 2;
				Vector3 position = tpList[rInt].gameObject.transform.position;
				position.y += 0.5f;
				col.gameObject.transform.position = position;
			
			}

		}	
	}
}