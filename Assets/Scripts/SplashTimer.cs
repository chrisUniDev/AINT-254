using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashTimer : MonoBehaviour {

	public float waitTime = 7f;
	private float timer = 0;
	public string mainMenu;

	void Start () {
		Time.timeScale = 1f;
		Cursor.visible = false;

	}
	void Update()
	{
		timer += Time.deltaTime;

		if (timer >= waitTime)
			Application.LoadLevel(mainMenu);
	}


}
