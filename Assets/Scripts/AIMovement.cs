using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour {

    Rigidbody rigidBody;

    public Transform path;

    private Transform Player;

    private List<Transform> nodes;
    private int currentNode = 0;

    public float distanceFromNode = 100;

    //public float movementSpeed = 10f;
    public float rotationalDamp = 0.5f;
    public float DodgeMultiplier = 10f;
    public AnimationCurve AccelerationCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    public Vector2 SpeedRange = new Vector2(50.0f, 100.0f);
    [SerializeField]
    private float BankAngleSmooth = 2.5f; //how fast you tilt when turning
    [SerializeField]
    private Vector3 Maneuverability = new Vector3(75.0f, 75.0f, -50.0f); // how fast you turn
    [SerializeField]
    private float MaxBankAngleOnTurn = 45.0f; //maxium tilt when turning

    private Vector3[] RotationDirections = { Vector3.right, Vector3.up, Vector3.forward };

    public float torque = 5;

    private Quaternion m_InitialRotation;

    Vector3 acceleration;
    Vector3 velocity;

    [SerializeField]
    float detectionDistance = 500;
    [SerializeField]
    float frontDetectionDistance = 580f;
    float rayCastOffset = 5f;
    

    private float m_angleY;
    private float m_angleZ;
    private float m_angleX;


    void Start() {

        rigidBody = GetComponent<Rigidbody>();
        Player = transform;
        m_InitialRotation = gameObject.transform.localRotation;
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++) {
            if (pathTransforms[i] != path.transform) {
                nodes.Add(pathTransforms[i]);
            }
        }
    }

    void FixedUpdate()
    {

        Debug.DrawLine(transform.position, nodes[currentNode].position);

        Pathfinding();
       
        CheckWayPointDistance();

        





    }

    public float SpeedFactor
    {
        get
        {
            float dis = Vector3.Distance(transform.position, nodes[currentNode].position);
   
            return AccelerationCurve.Evaluate(dis * 0.001f);
        }
    }

    public float CurrentSpeed
    {
        get
        {
            
            return Mathf.Lerp(SpeedRange.x, SpeedRange.y, SpeedFactor);
        }
    }

    Vector3 MoveTowardsNode(Vector3 target)
    {
        Vector3 distance = target - transform.position;

        if (distance.magnitude < 25)
        {
            return distance.normalized * -SpeedRange.y;
        }
        else
        {
            return distance.normalized * SpeedRange.y;
        }
    }


    void Pathfinding()
    {

        RaycastHit hit;
        Vector3 raycastOffset = Vector3.zero;
        float frontCenterAngle = 100f;

        //Front Sensor Array
        Vector3 left = transform.position - transform.right * rayCastOffset;
        Vector3 right = transform.position + transform.right * rayCastOffset;
        Vector3 up = transform.position + transform.up * rayCastOffset;
        Vector3 down = transform.position - transform.up * rayCastOffset;

        //Vertical Sensor Array
        Vector3 center = transform.position * rayCastOffset;
        Vector3 centerFront = transform.position + transform.forward * rayCastOffset;
        Vector3 centerBack = transform.position - transform.forward * rayCastOffset;

        //Horizontal Sensor Array
        Vector3 HorizonalCenter = transform.position * rayCastOffset;
        Vector3 HorizonalCenterFront = transform.position + transform.forward * rayCastOffset;
        Vector3 HorizonalCenterBack = transform.position - transform.forward * rayCastOffset;

                if (Physics.Raycast(left, transform.forward, out hit, frontDetectionDistance)) {
                    raycastOffset += Vector3.right;
                    Debug.DrawRay (left, transform.forward * frontDetectionDistance, Color.cyan);
                }else if (Physics.Raycast(right, transform.forward, out hit, frontDetectionDistance)) {
                    raycastOffset -= Vector3.right;
                    Debug.DrawRay (right, transform.forward * frontDetectionDistance, Color.cyan);
                }else if (Physics.Raycast(up, transform.forward, out hit, frontDetectionDistance)) {
                    raycastOffset -= Vector3.up;
                    Debug.DrawRay (up, transform.forward * detectionDistance, Color.cyan);
                }else if (Physics.Raycast(down, transform.forward, out hit, frontDetectionDistance)) {
                    raycastOffset += Vector3.up;
                    Debug.DrawRay (down, transform.forward * frontDetectionDistance, Color.cyan);
                }

                if (Physics.Raycast(center, transform.up, out hit, detectionDistance / 2) || Physics.Raycast(centerFront, transform.up, out hit, detectionDistance / 2) || Physics.Raycast(centerBack, transform.up, out hit, detectionDistance / 2)) {
                    raycastOffset -= Vector3.up;
                    Debug.DrawRay (center, transform.up * detectionDistance /2, Color.cyan);
                    Debug.DrawRay (centerFront, transform.up * detectionDistance /2, Color.cyan);
                    Debug.DrawRay (centerBack, transform.up * detectionDistance /2, Color.cyan);
                }else if (Physics.Raycast(center, -transform.up, out hit, detectionDistance / 2) ||Physics.Raycast(centerFront, -transform.up, out hit, detectionDistance / 2) || Physics.Raycast(centerBack, -transform.up, out hit, detectionDistance / 2)) {
                    raycastOffset += Vector3.up;
                    Debug.DrawRay (center, -transform.up * detectionDistance /2, Color.cyan);
                    Debug.DrawRay (centerFront, -transform.up * detectionDistance /2, Color.cyan);
                    Debug.DrawRay (centerBack, -transform.up * detectionDistance /2, Color.cyan);
                }

                if (Physics.Raycast(HorizonalCenter, transform.right, out hit, detectionDistance / 2) || Physics.Raycast(HorizonalCenterFront, transform.right, out hit, detectionDistance / 2) || Physics.Raycast(HorizonalCenterBack, transform.right, out hit, detectionDistance / 2)) {
                    raycastOffset -= Vector3.right;
                    Debug.DrawRay (HorizonalCenter, transform.right * detectionDistance /2, Color.cyan);
                    Debug.DrawRay (HorizonalCenterFront, transform.right * detectionDistance /2, Color.cyan);
                    Debug.DrawRay (HorizonalCenterBack, transform.right * detectionDistance /2, Color.cyan);
                }else if (Physics.Raycast(HorizonalCenter, -transform.right, out hit, detectionDistance / 2) ||Physics.Raycast(HorizonalCenterFront, -transform.right, out hit, detectionDistance / 2) || Physics.Raycast(HorizonalCenterBack, -transform.right, out hit, detectionDistance / 2)) {
                    raycastOffset += Vector3.right;
                    Debug.DrawRay (HorizonalCenter, -transform.right * detectionDistance /2, Color.cyan);
                    Debug.DrawRay (HorizonalCenterFront, -transform.right * detectionDistance /2, Color.cyan);
                    Debug.DrawRay (HorizonalCenterBack, -transform.right * detectionDistance /2, Color.cyan);
                }

                if (Physics.Raycast(HorizonalCenter, Quaternion.AngleAxis(frontCenterAngle, transform.forward)* transform.right, out hit, detectionDistance / 2) || Physics.Raycast(HorizonalCenterFront, Quaternion.AngleAxis(frontCenterAngle, transform.forward)* transform.right, out hit, detectionDistance / 2) || Physics.Raycast(HorizonalCenterBack, Quaternion.AngleAxis(frontCenterAngle, transform.forward)* transform.right, out hit, detectionDistance / 2)) {
                    raycastOffset -= Vector3.right;
                    raycastOffset += Vector3.up;
                    Debug.DrawRay (HorizonalCenter, Quaternion.AngleAxis(frontCenterAngle, transform.forward)* transform.right * detectionDistance /2, Color.cyan);
                    Debug.DrawRay (HorizonalCenterFront,Quaternion.AngleAxis(frontCenterAngle, transform.forward)* transform.right * detectionDistance /2, Color.cyan);
                    Debug.DrawRay (HorizonalCenterBack,Quaternion.AngleAxis(frontCenterAngle, transform.forward)* transform.right * detectionDistance /2, Color.cyan);
                }else if (Physics.Raycast(HorizonalCenter, Quaternion.AngleAxis(frontCenterAngle, transform.forward)* -transform.right, out hit, detectionDistance / 2) ||Physics.Raycast(HorizonalCenterFront, Quaternion.AngleAxis(frontCenterAngle, transform.forward)* -transform.right, out hit, detectionDistance / 2) || Physics.Raycast(HorizonalCenterBack, Quaternion.AngleAxis(frontCenterAngle, transform.forward)* -transform.right, out hit, detectionDistance / 2)) {
                    raycastOffset += Vector3.right;
                    raycastOffset -= Vector3.up;
                    Debug.DrawRay (HorizonalCenter, Quaternion.AngleAxis(frontCenterAngle, transform.forward)* -transform.right * detectionDistance /2, Color.cyan);
                    Debug.DrawRay (HorizonalCenterFront,Quaternion.AngleAxis(frontCenterAngle, transform.forward)* -transform.right * detectionDistance /2, Color.cyan);
                    Debug.DrawRay (HorizonalCenterBack,Quaternion.AngleAxis(frontCenterAngle, transform.forward)* -transform.right * detectionDistance /2, Color.cyan);
                }

                if (Physics.Raycast(HorizonalCenter, Quaternion.AngleAxis(-frontCenterAngle, transform.forward)* transform.right, out hit, detectionDistance / 2) || Physics.Raycast(HorizonalCenterFront, Quaternion.AngleAxis(-frontCenterAngle, transform.forward)* transform.right, out hit, detectionDistance / 2) || Physics.Raycast(HorizonalCenterBack, Quaternion.AngleAxis(-frontCenterAngle, transform.forward)* transform.right, out hit, detectionDistance / 2)) {
                    raycastOffset -= Vector3.right;
                    raycastOffset -= Vector3.up;
                    Debug.DrawRay (HorizonalCenter, Quaternion.AngleAxis(-frontCenterAngle, transform.forward)* transform.right * detectionDistance /2, Color.cyan);
                    Debug.DrawRay (HorizonalCenterFront,Quaternion.AngleAxis(-frontCenterAngle, transform.forward)* transform.right * detectionDistance /2, Color.cyan);
                    Debug.DrawRay (HorizonalCenterBack,Quaternion.AngleAxis(-frontCenterAngle, transform.forward)* transform.right * detectionDistance /2, Color.cyan);
                }else if (Physics.Raycast(HorizonalCenter, Quaternion.AngleAxis(-frontCenterAngle, transform.forward)* -transform.right, out hit, detectionDistance / 2) ||Physics.Raycast(HorizonalCenterFront, Quaternion.AngleAxis(-frontCenterAngle, transform.forward)* -transform.right, out hit, detectionDistance / 2) || Physics.Raycast(HorizonalCenterBack, Quaternion.AngleAxis(-frontCenterAngle, transform.forward)* -transform.right, out hit, detectionDistance / 2)) {
                    raycastOffset += Vector3.right;
                    raycastOffset += Vector3.up;
                    Debug.DrawRay (HorizonalCenter, Quaternion.AngleAxis(-frontCenterAngle, transform.forward)* -transform.right * detectionDistance /2, Color.cyan);
                    Debug.DrawRay (HorizonalCenterFront,Quaternion.AngleAxis(-frontCenterAngle, transform.forward)* -transform.right * detectionDistance /2, Color.cyan);
                    Debug.DrawRay (HorizonalCenterBack,Quaternion.AngleAxis(-frontCenterAngle, transform.forward)* -transform.right * detectionDistance /2, Color.cyan);
                }
                
                if (raycastOffset != Vector3.zero) {
            //transform.Rotate (raycastOffset * DodgeMultiplier * Time.deltaTime);
            //rigidBody.MoveRotation(rigidBody.rotation * raycastOffset * DodgeMultiplier * Time.deltaTime);
                //rigidBody.MovePosition();
                rigidBody.AddForce(raycastOffset * DodgeMultiplier * Time.deltaTime);
            rigidBody.AddTorque(raycastOffset * DodgeMultiplier * Time.deltaTime);
           
        }
        else
        {
            Quaternion desiredRotation = Quaternion.LookRotation(velocity);

            //rotation
            //transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * 3);
            //Quaternion rotation = Quaternion.LookRotation(forces);

            Vector3 m_targetpos = nodes[currentNode].position - transform.position;
            Vector3 m_localTarget = transform.InverseTransformPoint(nodes[currentNode + 2].position);


            m_angleY = Mathf.Atan2(m_localTarget.x, m_localTarget.z) * Mathf.Rad2Deg;
            m_angleX = Mathf.Atan2(m_localTarget.y, m_localTarget.z) * Mathf.Rad2Deg;
            m_angleZ = Mathf.Atan2(m_localTarget.x, m_localTarget.y) * Mathf.Rad2Deg;

            Vector3 eularAngleVelocity = new Vector3(0, m_angleY, m_angleX);

            Quaternion deltaRotation = Quaternion.Euler(eularAngleVelocity * Time.deltaTime);

            rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);


            Vector3 forces = MoveTowardsNode(nodes[currentNode].position);

            acceleration = forces;

            velocity += 2 * acceleration * Time.deltaTime;

            if (velocity.magnitude > SpeedRange.y)
            {
                velocity = velocity.normalized * SpeedRange.y;
            }

            //rigidBody.velocity = velocity;
            rigidBody.AddForce(transform.forward * CurrentSpeed);

        }
    }


    int m_cashedNode;
    
    private void CheckWayPointDistance()
    {
    

        if (currentNode == 0)
        {
            currentNode = 2;
        }
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < distanceFromNode)
        {
            if (currentNode == nodes.Count - 2 || currentNode >= nodes.Count - 2)
            {
                currentNode = 2;
            }
            else
            {
                currentNode++;
                m_cashedNode = currentNode;
            }
        }
        else if (Vector3.Distance(transform.position, nodes[currentNode].position) > distanceFromNode * 2 && m_cashedNode == currentNode)
        {
          
            currentNode = currentNode + 20;
        }
        if (currentNode == nodes.Count - 2 || currentNode >= nodes.Count - 2)
        {
            currentNode = 2;
        }
    }
            
            

       

    

    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(this.transform.position, detectionDistance);
    //}
}
