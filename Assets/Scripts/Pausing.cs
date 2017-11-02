using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Pausing : MonoBehaviour {

	public GameObject pauseCanvas;
	public GameObject HUD;
	public GameObject arrow;

	public GameObject resumeBtn;

	public GameObject playerShip;
	public GameObject AI1;
	public GameObject AI2;
	public GameObject AI3;
	public GameObject AI4;
	public GameObject AI5;
	public GameObject AI6;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("joystick button 7")) {
			Paused ();
		}
	}

	public void Paused () {

		//Time.timeScale = 0f;

		HUD.SetActive (false);
		arrow.SetActive (false);

		playerShip.SetActive (false);
		AI1.SetActive (false);
		AI2.SetActive (false);
		AI3.SetActive (false);
		AI4.SetActive (false);
		AI5.SetActive (false);
		AI6.SetActive (false);

		pauseCanvas.SetActive (true);
		EventSystem.current.SetSelectedGameObject(resumeBtn);
	}

	public void Resume () {
		//Time.timeScale = 1f;

		HUD.SetActive (true);
		arrow.SetActive (true);

		playerShip.SetActive (true);
		AI1.SetActive (true);
		AI2.SetActive (true);
		AI3.SetActive (true);
		AI4.SetActive (true);
		AI5.SetActive (true);
		AI6.SetActive (true);

		pauseCanvas.SetActive (false);
	}

	public void QuitGame () {
		StartCoroutine (FadeToMenu ());
	}

	public void Restartlevel () {
		StartCoroutine (FadeToRestart ());
	}

	IEnumerator FadeToMenu () {
		float fadeTime = GameObject.Find ("_Managers").GetComponent<Fading> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene("MainMenu");
	}

	IEnumerator FadeToRestart () {
		float fadeTime = GameObject.Find ("_Managers").GetComponent<Fading> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene("figureOfEight");
	}

}