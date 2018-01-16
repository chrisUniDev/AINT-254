using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
 

        int i = 0;
        while (i < 2)
        {
            if (Mathf.Abs(Input.GetAxis("LeftHorizontal" + i)) > 0.2F || Mathf.Abs(Input.GetAxis("LeftHorizontal" + i)) > 0.2F)
                Debug.Log(Input.GetJoystickNames()[i] + " is moved");

            i++;
        }

    }
}
