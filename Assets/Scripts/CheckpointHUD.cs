using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHUD : MonoBehaviour {

	public Transform target;
	float orginalSize = 0.02f;


	void Update () 
	{
		//Scales with distance to player
		float dis = Vector3.Distance (target.position, transform.position);
		float newScale = dis * orginalSize;

		transform.localScale = new Vector3 (newScale,newScale,0);

		Vector3 lookDir = target.position - transform.position;
		//Vector3 lookDir = transform.InverseTransformPoint(target.position);
		//Vector3 lookDir = transform.TransformPoint(target.position);
		//player.InverseTransformPoint (ShipCheckPoint.checkPointArray [ShipCheckPoint.currentCheckPoint].position);
		lookDir.y = 0;
		transform.rotation = Quaternion.LookRotation (lookDir);

		//float a = Mathf.Atan2 (lookDir.x, lookDir.z) * Mathf.Rad2Deg;
		//transform.localEulerAngles = new Vector3 (lookDir.x,lookDir.y,lookDir.z);

		//looks at player
		transform.LookAt (target);


	}
}
