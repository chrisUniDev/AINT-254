using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour {

    Rigidbody rigidBody;

    public Transform path;

    private Transform Player;

    private List<Transform> nodes;
    private int currentNode = 0;

    public float distanceFromNode = 5;

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

    private Quaternion m_InitialRotation;

    float detectionDistance = 20f;
    float frontDetectionDistance = 180f;
    float rayCastOffset = 5f;
    

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

    void Update()
    {

        Debug.DrawLine(transform.position, nodes[currentNode].position);

        Pathfinding();
        Move();
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

    void Turn()
    {
        Vector3 rotationsdirections = new Vector3(1,1,1);
        Debug.Log(CurrentSpeed);

        for (int i = 0; i < 3; ++i)
        {
            //Player.localRotation *= Quaternion.AngleAxis(rotationsdirections[i] * Maneuverability[i] * Time.deltaTime, RotationDirections[i]);
        }


       // Player.localPosition += Player.forward * CurrentSpeed * Time.deltaTime;


        Vector3 pos = nodes[currentNode].position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(pos);

      

        float angle = Vector3.Angle(Player.position, pos);
        float sinAngle = Mathf.Sign(angle);

        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 50)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation * Quaternion.AngleAxis(-sinAngle * MaxBankAngleOnTurn, Vector3.forward), BankAngleSmooth * Time.deltaTime);
        }
        else
        {

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationalDamp * Time.deltaTime);
        }

        
        //Player.localRotation = Quaternion.Slerp(rotation, m_InitialRotation * Quaternion.AngleAxis(-rotation.y* MaxBankAngleOnTurn, Vector3.forward), BankAngleSmooth * Time.deltaTime);
    }

    void Move()
    {

        transform.position += transform.forward * CurrentSpeed * Time.deltaTime;

    }

    void Pathfinding()
    {

        RaycastHit hit;
        Vector3 raycastOffset = Vector3.zero;
        float frontCenterAngle = 30f;

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


        //if (Physics.SphereCast(transform.position, detectionDistance, transform.forward, out hit, 50))
        //{
        //    Debug.Log("Opps we Hit");
            
            
        //}




        
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
                    transform.Rotate (raycastOffset * DodgeMultiplier * Time.deltaTime);
                } else {
                    Turn ();
                }
            }
            
    
    private void CheckWayPointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 60f)
        {
            if (currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            }
            else
            {
                currentNode++;
                
            }
        }
    }
            
            

       

    

    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(this.transform.position, detectionDistance);
    //}
}
