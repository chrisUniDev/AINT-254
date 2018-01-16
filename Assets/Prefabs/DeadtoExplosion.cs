using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadtoExplosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        wait();

    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
   
}
