using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WayPointRingHud : MonoBehaviour {

	public Transform arrow;
	public Transform player;


	private shipCheckPoint ShipCheckPoint; 

	// Use this for initialization
	void Start () {
		ShipCheckPoint = player.GetComponent<shipCheckPoint> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 dir = player.InverseTransformPoint (ShipCheckPoint.checkpointNodes[ShipCheckPoint.currentCheckPoint].position);
		float a = Mathf.Atan2 (dir.x, dir.z) * Mathf.Rad2Deg;
		a += 180;
		arrow.transform.localEulerAngles = new Vector3 (0,180,a);
	}


}
