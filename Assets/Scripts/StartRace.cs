using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRace : MonoBehaviour
{

    private GameObject[] ships;

    SpriteRenderer rend;

    AIMovement AIMove;
    PlayerController playerMove;

    bool done = false;

    Vector2 tempSpeed;

    // Use this for initialization
    void Start()
    {
        ships = GameObject.FindGameObjectsWithTag("ShipMesh");
        for (int i = 0; i < ships.Length; i++)
        {
            if (ships[i].name != "ShipMeshPlayer")
            {
                //ships[i].SetActive(false);
                AIMove = ships[i].GetComponent<AIMovement>();
                AIMove.enabled = false;
            }
            else if (ships[i].name == "ShipMeshPlayer")
            {
                playerMove = ships[i].GetComponent<PlayerController>();
                tempSpeed = playerMove.SpeedRange;

                playerMove.SpeedRange = new Vector2(0f,0f);

                playerMove.allowMovement = false;
                
            }


        }

        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame

    private void LateUpdate()
    {
        if (!rend.enabled && done == false)
        {
            done = true;
            for (int i = 0; i < ships.Length; i++)
            {
                if (ships[i].name != "ShipMeshPlayer")
                {
                    //ships[i].SetActive(false);
                    AIMove = ships[i].GetComponent<AIMovement>();
                    AIMove.enabled = true;
                }
                else if (ships[i].name == "ShipMeshPlayer")
                {
                    playerMove = ships[i].GetComponent<PlayerController>();

                    playerMove.SpeedRange = tempSpeed;

                    playerMove.allowMovement = true;

                }

            }


        }
    }
}
