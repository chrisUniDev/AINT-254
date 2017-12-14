using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndRace : MonoBehaviour
{
    GameObject[] Ships;
    GameObject[] ShipsMesh;
    shipCheckPoint[] ShipCheckpoints;

    public GameObject mainCanvas;

    public GameObject[] gridStats;

    public CheckPoint checkpointSystem;

    public GameObject EndgameCanvas;

    int lap;
    public int maxiumLap;


    private void Start()
    {
        Ships = GameObject.FindGameObjectsWithTag("Ship");
        ShipsMesh = new GameObject[Ships.Length];

        for (int i = 0; i < ShipsMesh.Length; i++)
        {
            ShipsMesh[i] = GameObject.FindGameObjectWithTag("ShipMesh");
        }

        ShipCheckpoints = new shipCheckPoint[ShipsMesh.Length];
        //PlayerCheckpoints = Player.GetComponent<shipCheckPoint> ();


        for (int i = 0; i < Ships.Length; i++)
        {
            ShipCheckpoints[i] = Ships[i].GetComponentInChildren<shipCheckPoint>();
        }
    }


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
        for (int i = 0; i < Ships.Length; i++)
        {

            for (int j = 0; j < ShipCheckpoints.Length; j++)
            {
                if (ShipCheckpoints[j].racePosition == i)
                {
                    


                    if (gridStats[i].gameObject.transform.GetChild(1).name == "Name")
                    {

                        gridStats[i].gameObject.transform.GetChild(1).GetComponent<Text>().text = ShipCheckpoints[j].Name;
                    }


                    if (gridStats[i].GetComponentInChildren<Text>().name == "Pos")
                    {
                        gridStats[i].GetComponentInChildren<Text>().text = (ShipCheckpoints[j].racePosition + 1).ToString();
                    }

                    if (gridStats[i].gameObject.transform.GetChild(2).name == "Time")
                    {

                        gridStats[i].gameObject.transform.GetChild(2).GetComponent<Text>().text = "Coming Soon";
                    }



                }
            }

           
        }

        if (gridStats.Length > Ships.Length)
        {
            for (int i = Ships.Length; i < gridStats.Length; i++)
            {
                if (gridStats[i].gameObject.transform.GetChild(1).name == "Name")
                {

                    gridStats[i].gameObject.transform.GetChild(1).GetComponent<Text>().text = "";
                }


                if (gridStats[i].GetComponentInChildren<Text>().name == "Pos")
                {
                    gridStats[i].GetComponentInChildren<Text>().text = "";
                }

                if (gridStats[i].gameObject.transform.GetChild(2).name == "Time")
                {

                    gridStats[i].gameObject.transform.GetChild(2).GetComponent<Text>().text = "";
                }
            }
        }



        for (int i = 0; i < Ships.Length; i++)
        {
            Ships[i].SetActive(false);
        }
        mainCanvas.SetActive(false);
        float fadeTime = GameObject.Find("_Managers").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        //SceneManager.LoadScene("GameOver");
        GameObject.Find("_Managers").GetComponent<Fading>().enabled = false;
        EndgameCanvas.SetActive(true);

    }



}
