using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour {

    Rigidbody rigidbody;
    public GameObject mesh;
    public PlayerController controls;

    [SerializeField]private bool m_AI = false;

    shipCheckPoint m_checkRespawnValue;
    AIMovement m_AINode;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        m_checkRespawnValue = GetComponent<shipCheckPoint>();
        if (m_AI)
        {
            m_AINode = GetComponent<AIMovement>();
        }
    }
	
	// Update is called once per frame
	void Update () {

  


	}

    IEnumerator WaitTeleoport()
    {
        
        yield return new WaitForSeconds(2);
        mesh.SetActive(true);
        this.transform.position = m_checkRespawnValue.Respawnpoints[m_checkRespawnValue.currentCheckPoint - 1].transform.position;
        this.transform.rotation = m_checkRespawnValue.Respawnpoints[m_checkRespawnValue.currentCheckPoint - 1].transform.rotation;
        controls.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Object")
        {
            Debug.Log("HIT");

            if (rigidbody.velocity.magnitude > 10)
            {
                //ship body to disapear
                //play explosion
                mesh.SetActive(false);
                controls.enabled = false;
                StartCoroutine(WaitTeleoport());
                //wait 2 seconds
                //teleport player
                //enable ship body 


          







                if (m_AI)
                {
                    for (int i = 0; i < m_checkRespawnValue.Respawnpoints.Length; i++)
                    {
                        if (m_checkRespawnValue.currentCheckPoint - 1 == i)
                        {
                            //needs fixing
                            m_AINode.currentNode = 2;
                        }
                    }


                    
                }
                Debug.Log("Player Explosion");
            }
        }
    }
}
