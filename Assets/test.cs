using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    string[] controllers;

	// Use this for initialization
	void Start () {
        controllers = Input.GetJoystickNames();
        Debug.Log(controllers.Length);

        for (int i = 0; i < controllers.Length; i++)
        {
            
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
