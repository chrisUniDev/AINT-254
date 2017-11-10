using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPowerUp : MonoBehaviour {

	[SerializeField]
	GameObject player;

	PlayerController controller;

	// Use this for initialization
	void Start () {

		controller = player.GetComponent<PlayerController> ();


	}

	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Boost () {
		
		controller.SpeedRange = new Vector2 (2000.0f, 2000.0f);
		yield return new WaitForSeconds (0.1f);
		controller.SpeedRange = new Vector2 (100.0f, 800.0f);
	}

	void OnTriggerEnter (Collider col) {
		Debug.Log (controller.SpeedRange);
		StartCoroutine (Boost ());
	}

}
