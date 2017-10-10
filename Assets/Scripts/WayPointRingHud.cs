using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WayPointRingHud : MonoBehaviour {

	public Transform arrow;
	public Transform player;

	public int MaxiumLaps = 3;

	public Text Playerpos;

	public Text PlayerLap;
	public Text PlayerCheckPoint;

	private shipCheckPoint ShipCheckPoint; 
	public CheckPoint checkpoint;

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

		DisplayPlayerPos ();
		DisplayLaps ();
		DisplayCheckPointNumber ();
		isGameFinished ();

	}

	void DisplayPlayerPos(){
		if (checkpoint.PlayersRacePos == 1) {
			Playerpos.text = "" + checkpoint.PlayersRacePos + "st";
		}else if (checkpoint.PlayersRacePos == 2) {
			Playerpos.text = "" + checkpoint.PlayersRacePos + "nd";
		}else if (checkpoint.PlayersRacePos == 3) {
			Playerpos.text = "" + checkpoint.PlayersRacePos + "rd";
		}else if (checkpoint.PlayersRacePos > 3) {
			Playerpos.text = "" + checkpoint.PlayersRacePos + "th";
		}

	}

	void DisplayLaps(){
		if (ShipCheckPoint.currentLap == 0) {
			PlayerLap.text = "LAP: 1"; 
		} else {
			PlayerLap.text = "LAP: " + ShipCheckPoint.currentLap;
		}
	}

	void DisplayCheckPointNumber(){
		PlayerCheckPoint.text = "CHECKPOINT: " + ShipCheckPoint.currentCheckPoint;
	}

	void isGameFinished(){
		if ( ShipCheckPoint.currentLap == MaxiumLaps + 1) {
			Debug.Log ("GAME FINISHED");
		}
	}
}
