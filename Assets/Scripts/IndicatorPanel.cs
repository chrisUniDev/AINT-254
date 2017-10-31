using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorPanel : MonoBehaviour {

    public GameObject indicatorPrefab;
    public GameObject indicatorArrowPrefab;

    public shipCheckPoint FindCheckpoint;

    public GameObject checkpoints;

    public List<Transform> checkpointNodes = new List<Transform>();


    // Use this for initialization
    void Start () {
        

        Transform[] nodes = checkpoints.GetComponentsInChildren<Transform>();

        for (int i = 0; i < nodes.Length; i++)
        {
            

            checkpointNodes.Add(nodes[i]);
        }
	}

    private void LateUpdate()
    {
        Paint();
    }

    // Update is called once per frame
    void Update () {
		
	}

    void Paint()
    {
        for (int i = 0; i < checkpointNodes.Count; i++)
        {
            if (i == FindCheckpoint.currentCheckPoint)
            {
                Instantiate(indicatorPrefab, checkpointNodes[i].transform);
            }
        }
    }
}
