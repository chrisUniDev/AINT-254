using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class shipCheckPoint : MonoBehaviour {

	public string Name = "NAME";
    private GameObject checkpoints;
    //public Transform[] checkPointArray;
	public int currentCheckPoint = 0;
    private int m_cashedCheckpoint;
    private int m_cashedLap;
    public int currentLap = 0;
	public Vector3 startPos;

    private int maxDistanceBeforeRespawn = 8000;

	public float DistanceToNextCheckPoint;

    [SerializeField] private AudioClip m_checkpointAudio;
    [SerializeField] private AudioClip m_lapAudio;

    private AudioSource audioSource;

	public int racePosition;

    public List<Transform> checkpointNodes = new List<Transform>();

    private GameObject[] tempRespawn;
    private GameObject[] Respawnpoints;

    // Use this for initialization
    void Start () 
	{
        audioSource = GetComponent<AudioSource>();
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

        tempRespawn = GameObject.FindGameObjectsWithTag("RespawnPoint");

        Respawnpoints = new GameObject[tempRespawn.Length];

        for (int i = 0; i < tempRespawn.Length; i++)
        {

            Respawnpoints[i] = tempRespawn[tempRespawn.Length -1 - i];
        }


	}

	void FixedUpdate(){
		DistanceToNextCheckPoint = Vector3.Distance (transform.position, checkpointNodes[currentCheckPoint].transform.position);

        

        TraveledToFar();

    }

	public int GetRacePos{
		set{ racePosition = value;
		}
        get
        {
            return racePosition;
        }
	}

    public int GetLap
    {
        get
        {
            return currentLap;
        }
    }

    private void Update()
    {
        if (m_cashedLap != currentLap)
        {
            m_cashedLap = currentLap;
            if (currentLap != 1)
            {
                audioSource.PlayOneShot(m_lapAudio);
            }

        }

        if (m_cashedCheckpoint != currentCheckPoint && m_cashedLap == currentLap && currentCheckPoint != 1 && currentCheckPoint != checkpointNodes.Count && currentCheckPoint != 0)
        {
            m_cashedCheckpoint = currentCheckPoint;
            audioSource.PlayOneShot(m_checkpointAudio);
            
        }

     
    }

    //If the player flys too far away from the checkpoint
    void TraveledToFar()
    {
        if (DistanceToNextCheckPoint > maxDistanceBeforeRespawn)
        {
            this.transform.position = Respawnpoints[currentCheckPoint - 1].transform.position;
            this.transform.rotation = Respawnpoints[currentCheckPoint - 1].transform.rotation;

        }
    }



}
