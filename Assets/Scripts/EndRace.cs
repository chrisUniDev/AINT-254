using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndRace : MonoBehaviour
{


    public CheckPoint checkpointSystem;

    int lap;
    public int maxiumLap;



    // Update is called once per frame
    void Update()
    {
        lap = checkpointSystem.getLap;

        if (lap == maxiumLap + 1)
        {
            StartCoroutine(FadeToExitGameScreen());
        }
    }


    IEnumerator FadeToExitGameScreen()
    {
        float fadeTime = GameObject.Find("_Managers").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene("GameOver");
    }



}
