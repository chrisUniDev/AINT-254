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

    private GameObject[] ships;





	// Use this for initialization
	void Start () {


        ships = GameObject.FindGameObjectsWithTag("Ship");
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("joystick button 7")) {
			Paused ();
            for (int i = 0; i < ships.Length; i++)
            {
                ships[i].SetActive(false);
            }
		}
	}

	public void Paused () {

		//Time.timeScale = 0f;

		HUD.SetActive (false);
		arrow.SetActive (false);



		pauseCanvas.SetActive (true);
		EventSystem.current.SetSelectedGameObject(resumeBtn);
	}

	public void Resume () {
		//Time.timeScale = 1f;

		HUD.SetActive (true);
		arrow.SetActive (true);

        for (int i = 0; i < ships.Length; i++)
        {
            ships[i].SetActive(true);
        }


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
		SceneManager.LoadScene("RaceTrackSinglePlayer");
	}

}