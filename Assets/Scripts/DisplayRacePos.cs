using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayRacePos : MonoBehaviour {

    shipCheckPoint FindRacePos;
    private int m_racePos;
    [SerializeField]
    private Text m_displayPos; 

	// Use this for initialization
	void Start () {
        FindRacePos = GetComponent<shipCheckPoint>();
	}
	
	// Update is called once per frame
	void Update () {
        m_racePos = FindRacePos.GetRacePos;

        if (m_racePos == 1)
        {
            m_displayPos.text = m_racePos.ToString() + "st";
        }
        else if (m_racePos ==  2)
        {
            m_displayPos.text = m_racePos.ToString() + "nd";
        }
        else if (m_racePos == 3)
        {
            m_displayPos.text = m_racePos.ToString() + "rd";
        }
        else if (m_racePos  > 3)
        {
            m_displayPos.text = m_racePos.ToString() + "th";
        }

    }
}
