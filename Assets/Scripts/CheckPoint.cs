using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckPoint : MonoBehaviour {


	GameObject[] Ships;
	GameObject[] ShipsMesh;

	public bool disableUI;

	shipCheckPoint[] ShipCheckpoints;

	//public GameObject currentCheckpointUI;
	//public GameObject nextCheckpointUI;

	public bool firstUIOnMarker;

	public int PlayersRacePos;
	int checkpoint;
	public int getLap;
	int lap;
	int EndCheck;
	bool newLap;
	bool firstlap;
	bool notLastCheckpoint;
	float[] DistanceToNextCheckpoint;
	float temp;


	// Use this for initialization
	void Awake () 
	{
		lap = 0;
		checkpoint = 0;
		PlayersRacePos = 0;
		EndCheck = 0;
		newLap = false;
		firstlap = true;
		notLastCheckpoint = true;
		disableUI = false;
		//Add all AI Ships aswell as the player!
		Ships = GameObject.FindGameObjectsWithTag ("Ship");

		ShipsMesh = new GameObject[Ships.Length];

		DistanceToNextCheckpoint = new float[Ships.Length];

		for (int i = 0; i < ShipsMesh.Length; i++) {
			ShipsMesh [i] = GameObject.FindGameObjectWithTag ("ShipMesh");
		}

		ShipCheckpoints = new shipCheckPoint[ShipsMesh.Length];
		//PlayerCheckpoints = Player.GetComponent<shipCheckPoint> ();
	

		for (int i = 0; i < Ships.Length; i++) {
			ShipCheckpoints [i] = Ships [i].GetComponentInChildren<shipCheckPoint> ();
		}

		if (firstUIOnMarker) {
			//currentCheckpointUI.SetActive (true);
			firstUIOnMarker = false;
		}
	}


	void Update(){
		FindLeadingCheckpoint ();
		CheckDistanceToNextCheckPoint ();
		OrderPostions ();
		CheckDistanceWithShips ();

		getLap = lap;
	}


	void OnTriggerEnter(Collider coll)
	{
		
		//if (coll.gameObject.tag == "ShipMesh") {
			//Debug.Log(coll.transform.parent.name);
			for (int i = 0; i < Ships.Length; i++) {

				if (coll.gameObject.transform.parent.name == Ships [i].name) {
					


					if (transform == ShipCheckpoints [i].checkpointNodes[ShipCheckpoints [i].currentCheckPoint].transform) {

						if (ShipCheckpoints [i].currentCheckPoint + 1 < ShipCheckpoints [i].checkpointNodes.Count) {

							if (ShipCheckpoints [i].currentCheckPoint == 0)
								ShipCheckpoints [i].currentLap++;

							ShipCheckpoints [i].currentCheckPoint++;
						} else {
							ShipCheckpoints [i].currentCheckPoint = 0;
							checkpoint = 0;

							newLap = false;
						
						}

					}

					if (Ships[i].name == "Player") {
						//currentCheckpointUI.SetActive (false);
						//nextCheckpointUI.SetActive (true);
					}

					if (disableUI == true) {
						//currentCheckpointUI.SetActive (false);
					}
				}
			}

		//}
			
	}

	void FindLeadingCheckpoint(){


		//Finds the Leading checkpoint
		for (int i = 0; i < Ships.Length; i++) {
			

				if (lap < ShipCheckpoints [i].currentLap) {
					lap = ShipCheckpoints [i].currentLap;
					checkpoint = 0;
					EndCheck = 0;
					notLastCheckpoint = true;

				}
			

			if (checkpoint == 8) {
				notLastCheckpoint = false;
				//checkpoint = 0;
			}




			if (checkpoint < ShipCheckpoints[i].currentCheckPoint && ShipCheckpoints [i].currentLap == lap ) {
				checkpoint = ShipCheckpoints [i].currentCheckPoint;

			}






		}
	}


	void CheckDistanceToNextCheckPoint()
	{
		//Gets the distance to the Leading checkpoint from each ship
		for (int i = 0; i <  Ships.Length; i++) {
			if (ShipCheckpoints [i].currentLap == lap) {
				if (ShipCheckpoints [i].currentCheckPoint == checkpoint) {
					DistanceToNextCheckpoint [i] = ShipCheckpoints [i].DistanceToNextCheckPoint;
				} else if(ShipCheckpoints [i].currentCheckPoint < checkpoint) {
					for (int j = ShipCheckpoints [i].currentCheckPoint; j < checkpoint; j++) {
						DistanceToNextCheckpoint [i] = ShipCheckpoints [i].DistanceToNextCheckPoint += Vector3.Distance (ShipCheckpoints [i].checkpointNodes[j].transform.position, ShipCheckpoints [i].checkpointNodes[j + 1].transform.position);
						//ShipCheckpoints[i].Distance += Vector3.Distance (ShipCheckpoints [i].checkPointArray [j].transform.position, ShipCheckpoints [i].checkPointArray [j + 1].transform.position);
					}
	
					//DistanceToNextCheckpoint [i] = ShipCheckpoints [i].DistanceToNextCheckPoint + Vector3.Distance (ShipCheckpoints [i].checkPointArray [ShipCheckpoints [i].currentCheckPoint].transform.position, ShipCheckpoints [i].checkPointArray [checkpoint].transform.position);
				}
			} else if(ShipCheckpoints [i].currentLap < lap) {

				for (int k = ShipCheckpoints [i].currentLap; k < lap; k++) {
					//DistanceToNextCheckpoint [i] = ShipCheckpoints [i].DistanceToNextCheckPoint + Vector3.Distance (ShipCheckpoints [i].checkPointArray [ShipCheckpoints [i].currentCheckPoint].transform.position, ShipCheckpoints [i].checkPointArray [checkpoint].transform.position) * ((lap*1) * 1000);
					for (int j = ShipCheckpoints [i].currentCheckPoint; j < checkpoint; j++) {
						DistanceToNextCheckpoint [i] += Vector3.Distance (ShipCheckpoints [i].checkpointNodes[j].transform.position, ShipCheckpoints [i].checkpointNodes[j + 1].transform.position);
						//ShipCheckpoints[i].Distance += Vector3.Distance (ShipCheckpoints [i].checkPointArray [j].transform.position, ShipCheckpoints [i].checkPointArray [j + 1].transform.position);
					}
					DistanceToNextCheckpoint[i] += ShipCheckpoints [i].DistanceToNextCheckPoint * 1000; 
				}

			}
		}
	}

	void OrderPostions(){
		//Sorts the Positions in order
		for (int i = 0; i < DistanceToNextCheckpoint.Length; i++) {
			for (int j = 0; j < DistanceToNextCheckpoint.Length - 1; j++) {
				if (DistanceToNextCheckpoint[j] > DistanceToNextCheckpoint[j + 1]) {
					temp = DistanceToNextCheckpoint [j + 1];
					DistanceToNextCheckpoint [j + 1] = DistanceToNextCheckpoint [j];
					DistanceToNextCheckpoint [j] = temp;
				}
			}
		}
	}

	void CheckDistanceWithShips(){
		//Adds what ship is at what distance
		for (int i = 0; i < Ships.Length; i++) {
			for (int j = 0; j < Ships.Length; j++) {
					if (ShipCheckpoints [i].DistanceToNextCheckPoint == DistanceToNextCheckpoint [j]) {
						ShipCheckpoints [i].GetRacePos = j + 1;
						;
						if (Ships [i].name == "Player") {
							PlayersRacePos = (j + 1);
							//Debug.Log (Ships[i].name + " Pos: " + (j+ 1));
						}
						//DistanceToNextCheckpoint [j] = 0;

					}
				
			}

		}
	}


}