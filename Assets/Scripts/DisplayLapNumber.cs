using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLapNumber : MonoBehaviour {

    shipCheckPoint FindLapNum;

    private int m_lapnum;

    [SerializeField]
    private Text m_displayLap;



    // Use this for initialization
    void Start () {
        FindLapNum = GetComponent<shipCheckPoint>();

    }
	
	// Update is called once per frame
	void Update () {
        m_lapnum = FindLapNum.GetLap;

        if (m_lapnum == 0)
        {
            m_displayLap.text = "Lap: 1 / 3";
        }
        else
        {
            m_displayLap.text = "Lap: " + m_lapnum + " / 3";
        }
      
	}
}
