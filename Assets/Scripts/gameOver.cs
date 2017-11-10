using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameOver : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ReturnToMenu () {
		StartCoroutine (FadeToMenu ());
	}

	public void ReplayLevel () {
		StartCoroutine (FadeToGame ());
	}

	IEnumerator FadeToMenu () {
		float fadeTime = GameObject.Find ("_Managers").GetComponent<Fading> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene("MainMenu");
	}

	IEnumerator FadeToGame () {
		float fadeTime = GameObject.Find ("_Managers").GetComponent<Fading> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene("RaceTrack1");
	}

}
