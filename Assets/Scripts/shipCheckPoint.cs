using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class shipCheckPoint : MonoBehaviour {

	public string Name = "NAME";
    private GameObject checkpoints;
    //public Transform[] checkPointArray;
	public int currentCheckPoint = 0;
	public int currentLap = 0;
	public Vector3 startPos;

	public float DistanceToNextCheckPoint;

	public int racePosition;

    public List<Transform> checkpointNodes = new List<Transform>();


    // Use this for initialization
    void Start () 
	{
        checkpoints = GameObject.FindGameObjectWithTag("checkpoints");
        startPos = transform.position;
        Transform[] nodes = checkpoints.GetComponentsInChildren<Transform>();

        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i].parent == checkpoints.transform)
            {
                Transform[] childNodes = nodes[i].GetComponentsInChildren<Transform>();

                checkpointNodes.Add(childNodes[1]);
            }
        }
	}

	void FixedUpdate(){
		DistanceToNextCheckPoint = Vector3.Distance (transform.position, checkpointNodes[currentCheckPoint].transform.position);
	}

	public int GetRacePos{
		set{ racePosition = value;
		}
	}



}
