using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMovement : MonoBehaviour {

    private Transform m_transform;
    private ParticleSystemRenderer m_particleSystem;
    [SerializeField]
    private Vector2 m_scaleRange;
    [SerializeField]
    private float m_posSmooth;
    [SerializeField]
    private float m_rotSmooth;
    [SerializeField]
    private PlayerController m_target;

	// Use this for initialization
	void Start ()
    {
        m_transform = transform;
        m_particleSystem = GetComponent<ParticleSystemRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_transform.position = Vector3.Lerp(m_transform.position, m_target.transform.position, Time.deltaTime * m_posSmooth);

        m_transform.rotation = Quaternion.Slerp(m_transform.rotation, m_target.transform.rotation, Time.deltaTime * m_rotSmooth);

        m_particleSystem.lengthScale = Mathf.Lerp(m_scaleRange.x, m_scaleRange.y, m_target.SpeedFactor);
    }
}
