using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointDirectionHud : MonoBehaviour
{

    //public GameObject goTarget;
    private SpriteRenderer m_srenderer;
    private Transform m_Tranform;

    public GameObject player;

    private shipCheckPoint m_checkpointSystem;


    void Start()
    {
        m_Tranform = transform;
        m_srenderer = GetComponent<SpriteRenderer>();

        m_checkpointSystem = player.GetComponent<shipCheckPoint>();
    }


    void LateUpdate()
    {
        PositionArrow();
    }
    void PositionArrow()
    {
        m_srenderer.enabled = false;

   
                Vector3 m_Pos = Camera.main.WorldToViewportPoint(m_checkpointSystem.checkpointNodes[m_checkpointSystem.currentCheckPoint].transform.position);

                if (m_Pos.z < Camera.main.nearClipPlane)
                {
                    //return; // Object is behind the camera
                }

                if (m_Pos.x >= 0.0f && m_Pos.x <= 1.0f && m_Pos.y >= 0.0f && m_Pos.y <= 1.0f)
                    return; // Object center is visible
                
                m_srenderer.enabled = true;
                m_Pos.x -= 0.5f; // Translate to use center of viewport 
                m_Pos.y -= 0.5f;
                m_Pos.z = 0; // I think I can do this rather than do a // a full projection onto the plane 
                float fAngle = Mathf.Atan2 (m_Pos.x, m_Pos.y);

                m_Tranform.eulerAngles = new Vector3(0.0f, 0.0f, -fAngle * Mathf.Rad2Deg);
                m_Pos.x = 0.5f * Mathf.Sin (fAngle) + 0.5f; // Place on ellipse touching
                m_Pos.y = 0.5f * Mathf.Cos (fAngle) + 0.5f; // side of viewport 
                m_Pos.z = Camera.main.nearClipPlane + 1f; // Looking from neg to pos Z; 
                m_Tranform.position = Camera.main.ViewportToWorldPoint(m_Pos);
    }
     
    
 }




