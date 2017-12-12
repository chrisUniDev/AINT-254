using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IncreasseNodes : MonoBehaviour {

    public GameObject startnode;
    public GameObject finalNode;

    public GameObject newnode;

    public int numberOfNewNodes = 10;

    private Vector3 distanceBetweenNodes;

    private float dis;

    private float disX;
    private float disY;
    private float disZ;





    // Use this for initialization
    void Start () {

        distanceBetweenNodes = (finalNode.transform.position - startnode.transform.position) / numberOfNewNodes;

        //Debug.Log(distanceBetweenNodes);

        for (int i = 0; i < numberOfNewNodes; i++)
        {
            Instantiate(startnode, new Vector3(startnode.transform.position.x + (distanceBetweenNodes.x * i), startnode.transform.position.y + (distanceBetweenNodes.y * i), startnode.transform.position.z + (distanceBetweenNodes.z * i)), Quaternion.identity);

        }

    }
	
	// Update is called once per frame
	void Update () {

   

	}
}
