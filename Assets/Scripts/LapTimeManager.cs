using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapTimeManager : MonoBehaviour {

    public static int Minute;
    public static int Second;
    public static float Milli;
    public string millDisplay;

    public GameObject MinuteBox;
    public GameObject SecondBox;
    public GameObject milliBox;

    public PlayerController player;



    void Update ()
    {
        if (player.allowMovement == true)
        {
            Milli += Time.deltaTime * 10;
            millDisplay = Milli.ToString("F0");
            milliBox.GetComponent<Text>().text = "" + millDisplay;

            if (Milli >= 10)
            {
                Milli = 0;
                Second += 1;
            }

            if (Second <= 9)
            {
                SecondBox.GetComponent<Text>().text = "0" + Second + ".";
            }
            else
            {
                SecondBox.GetComponent<Text>().text = "" + Second + ".";
            }

            if (Second >= 60)
            {
                Second = 0;
                Minute += 1;
            }

            if (Minute <= 9)
            {
                MinuteBox.GetComponent<Text>().text = "0" + Minute + ".";
            }
            else
            {
                MinuteBox.GetComponent<Text>().text = "" + Minute + ".";
            }
        }


       
	}
}
