using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuNav : MonoBehaviour {

	public GameObject mainMenu;
	public GameObject spMenu;
	public GameObject playDemo;

    public string Level;

	public Text progressText;

	int demoScene = 2;

	//Buttons
	public GameObject spButton;
	public GameObject playDemoButton;

	float timeLeft = 0.0f;
	string time;

	public Text counter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("joystick button 1") && spMenu.activeSelf) {
			SPBack ();
		}

		if (!spMenu.activeSelf && !mainMenu.activeSelf) {

			//StartCoroutine (LoadAsynchronously (demoScene));

			StartCoroutine (FadeToDemo ());

//			timeLeft += Time.deltaTime * 20;
//
//			time = timeLeft.ToString ("F0");
//
//			counter.text = time;
//
//			if(timeLeft >= 100)
//			{
//				timeLeft = 100;
//				StartCoroutine (FadeToDemo ());
//			}
		}

	}

	public void SPClick () {
		mainMenu.SetActive (false);
		spMenu.SetActive (true);
		EventSystem.current.SetSelectedGameObject(playDemoButton);
	}

	public void SPBack () {
		mainMenu.SetActive (true);
		spMenu.SetActive (false);
		EventSystem.current.SetSelectedGameObject(spButton);
	}

	public void DemoClick () {
		spMenu.SetActive (false);
		//playDemo.SetActive (true);
	}

	IEnumerator FadeToDemo () {
		float fadeTime = GameObject.Find ("_Managers").GetComponent<Fading> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		//SceneManager.LoadScene("Controller Scene");
		SceneManager.LoadScene(Level);
	}
		
	//OLD SCENE LOAD SCRIPT

//	IEnumerator LoadAsynchronously (int sceneIndex) {
//		AsyncOperation operation = SceneManager.LoadSceneAsync (sceneIndex);
//
//		while (!operation.isDone) {
//
//			float progress = Mathf.Clamp01 (operation.progress / 0.9f);
//
//			progressText.text = progress * 100.0f + "";
//
//			yield return null;
//		}
//	}


}
