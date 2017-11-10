using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Camera TargetCam; //Players Camera
    public Transform Player; //The transfrom mesh of the player
    public Vector4 Response; //how fast the input is interpolated

    [SerializeField] private AudioClip m_audioStarUp;
    [SerializeField] private AudioClip m_audioMain;
    [SerializeField] private AudioClip m_audioSlow;
    [SerializeField] private AudioClip m_audioIdol;

    private AudioSource m_audioSource;


    private float CameraDistanceSmooth = 0.85f;
    public Transform m_transform { get; private set; }

    private Vector3[] RotationDirections = { Vector3.right, Vector3.up, Vector3.forward };
    [SerializeField]
    private float m_cameraAngle = 18.0f; //Angle of the camera
    [SerializeField]
    private float m_cameraOffset = 44.0f; //Distance between the camera and the spaceship
    [SerializeField]
    private float m_cameraPosSmooth = 10.0f; //How fast the camera follows the spaceship's position
    [SerializeField]
    private float m_cameraRotationSmooth = 5.0f; //How fast the camera follows the spaceship's rotation
    [SerializeField]
    private float m_camreaOnRollCompensationFactor = 0.5f; //Tilt of the camera when the spaceship is doing a roll

    private Vector2 OnIdle = new Vector2(0.0f, 10.0f); //Offset of the look-at point (relative to the spaceship) when flying straight with a minimum speed
    private Vector2 Smooth = new Vector2(30.0f, 30.0f); //How fast the look-at point interpolates to the desired value. Higher = faster.
    private Vector2 OnMaxSpeed = new Vector2(50.0f, -50.0f); //Offset of the look-at point (relative to the spaceship) when flying or turning with a maximum speed
    private Vector2 OnTurn = new Vector2(30.0f, -30.0f); //Offset of the look-at point (relative to the spaceship) when turning with a minimum speed

    private float m_CameraDistance; //the idle camera distance
    private Quaternion m_InitialRotation; //the players initial rotation
    private float m_CameraFOV; // the cameras initial Feild of view

    private Transform m_cameraTransform; //cached cameras transform

    private Vector2 m_lookAtPointOffset; 
    
    private Vector4 SmoothedInput;

    //SpaceShip settings
    [SerializeField]
    private AnimationCurve m_spaceshipAccelerationCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f); //speed changes over time
    [SerializeField]
    private float BankAngleSmooth = 2.5f; //how fast you tilt when turning
    [SerializeField]
    private Vector3 Maneuverability = new Vector3(75.0f, 75.0f, -50.0f); // how fast you turn
    [SerializeField]
    private float MaxBankAngleOnTurn = 45.0f; //maxium tilt when turning
    
    public Vector2 SpeedRange = new Vector2(100.0f, 800.0f); //min and max speed
    [SerializeField]
    private float m_controllerSensitivity; //Controller sensitivity when flying with a minimum speed
    [SerializeField]
    private float m_controllerSensitivityOnMaxSpeed; //Controller sensitivity when flying with a maximum speed

    public string ContAxisX; //Rotation around x-axis
    public string ContAxisY; //Rotation around y-axis
    public string ContAxisZ; //Rotation around z-axis
    public string ContThrottle; //Speed control (speed increase)
    public string ContThrottleReduct; //Speed control (speed reduction)

    void Start ()
    {
        SmoothedInput = Vector4.zero;
        m_transform = transform;
        m_cameraTransform = TargetCam.transform;
        m_CameraDistance = CameraOffsetVector.magnitude;
        m_InitialRotation = Player.localRotation;
        m_CameraFOV = TargetCam.fieldOfView;
        m_lookAtPointOffset = OnIdle;
        m_audioSource = GetComponent<AudioSource>();
        m_cameraTransform.position = m_transform.position + CameraOffsetVector;
    }

    private void LateUpdate()
    {
        UpdateCamera();
    }

    private void Update ()
    {
        ShipAudio();
        UpdateInput();
        UpdateOrientationAndPos();
        


    }

    bool isMain = false;
    bool slowdownsound = false;

    private void ShipAudio()
    {

        if (Input.GetAxis(ContThrottle) > 0 && Input.GetAxis(ContThrottle) < 0.5f  && m_audioSource.isPlaying == false)
        {
            m_audioSource.PlayOneShot(m_audioStarUp);
            slowdownsound = false;

        }
        else if (Input.GetAxis(ContThrottle) >= 0.5f && m_audioSource.isPlaying == false)
        {
            m_audioSource.Stop();
            m_audioSource.clip = m_audioMain;
            m_audioSource.Play();
            isMain = true;
            slowdownsound = false;


        }
        else if (Input.GetAxis(ContThrottle) > 0 && Input.GetAxis(ContThrottle) < 0.5f && isMain == true)
        {
            m_audioSource.Stop();
            m_audioSource.clip = m_audioSlow;
            m_audioSource.Play();
            isMain = false;
            slowdownsound = true;


        }
        else if (Input.GetAxis(ContThrottleReduct) > 0 && CurrentSpeed < 10)
        {
            m_audioSource.Stop();
            m_audioSource.clip = m_audioSlow;
            m_audioSource.Play();
            slowdownsound = true;
            isMain = false;
        }


   

    }

    public Vector3 CameraOffsetVector
    {
        get
        {
            return new Vector3(0.0f, Mathf.Sin(m_cameraAngle * Mathf.Deg2Rad) * m_cameraOffset, -m_cameraOffset);
        }
    }

    public float SpeedFactor
    {
        get
        {
            
            return m_spaceshipAccelerationCurve.Evaluate(SmoothedInput.w);
            
        }
    }

    public float CurrentSpeed
    {
        get
        {
            return Mathf.Lerp(SpeedRange.x, SpeedRange.y, SpeedFactor);
        }
    }

    private void UpdateCamera()
    {
        Vector2 focalPointOnMoveOffset = Vector2.Lerp(OnTurn, OnMaxSpeed, SpeedFactor);
        //changes the position at which the camera looks at the players ship according to the angle and tilt of the ship
        m_lookAtPointOffset.x = Mathf.Lerp(m_lookAtPointOffset.x, Mathf.Lerp(OnIdle.x, focalPointOnMoveOffset.x * Mathf.Sign(SmoothedInput.y), Mathf.Abs(SmoothedInput.y)), Smooth.x * Time.smoothDeltaTime);
        m_lookAtPointOffset.y = Mathf.Lerp(m_lookAtPointOffset.y, Mathf.Lerp(OnIdle.y, focalPointOnMoveOffset.y * Mathf.Sign(SmoothedInput.x), Mathf.Abs(SmoothedInput.x)), Smooth.y * Time.smoothDeltaTime);

        Vector3 lookTargetPos = m_transform.position + m_transform.right * m_lookAtPointOffset.x + m_transform.up * m_lookAtPointOffset.y;

        Vector3 lookTargetVector = (m_transform.up + m_transform.right * SmoothedInput.z * m_camreaOnRollCompensationFactor).normalized;

        Quaternion targetCameraRoation = Quaternion.LookRotation(lookTargetPos - m_cameraTransform.position, lookTargetVector);
        //Rotates the camera 
        m_cameraTransform.rotation = Quaternion.Slerp(m_cameraTransform.rotation, targetCameraRoation, m_cameraRotationSmooth * Time.smoothDeltaTime);

        Vector3 cameraOffset = m_transform.TransformDirection(CameraOffsetVector);

        m_cameraTransform.position = Vector3.Lerp(m_cameraTransform.position, m_transform.position + cameraOffset, m_cameraPosSmooth * Time.smoothDeltaTime);

        float cameraDistance = cameraOffset.magnitude + (cameraOffset.normalized * SpeedRange.x * Time.smoothDeltaTime / m_cameraPosSmooth).magnitude;
        //Keeps the camera in the ideal distance from the player
        m_CameraDistance = Mathf.Lerp(m_CameraDistance, cameraDistance, CameraDistanceSmooth * Time.deltaTime);

        float baseFrustumHeight = 2.0f * m_CameraDistance * Mathf.Tan(m_CameraFOV * 0.5f * Mathf.Deg2Rad);

        TargetCam.fieldOfView = 2.0f * Mathf.Atan(baseFrustumHeight * 0.5f / Vector3.Distance(m_transform.position, m_cameraTransform.position)) * Mathf.Rad2Deg;
    }

    private void UpdateInput()
    { 
        float currentControllerSensitivity = Mathf.Lerp(m_controllerSensitivity, m_controllerSensitivityOnMaxSpeed, SpeedFactor);

        float currentSpinSpeed = Mathf.Lerp(m_controllerSensitivity, 5.0f, SpeedFactor);

        //Calculate raw input
        Vector4 currentRawInput = Vector4.zero;

        currentRawInput.x = Input.GetAxis(ContAxisY) * currentControllerSensitivity;
        currentRawInput.y = Input.GetAxis(ContAxisX) * currentControllerSensitivity;
        currentRawInput.z = Input.GetAxis(ContAxisZ) * currentSpinSpeed;

        if (Input.GetAxis(ContThrottle) != 0)
        {
            currentRawInput.w = Input.GetAxis(ContThrottle) * 1.0f;
        }

        if (Input.GetAxis(ContThrottleReduct) != 0)
        {
            currentRawInput.w = Input.GetAxis(ContThrottleReduct) * -0.5f;
        }

        //Calculate the smooth input
        Vector4 currentSmoothedInput = Vector4.zero;

        for (int i = 0; i < 4; ++i)
        {
            currentSmoothedInput[i] = Mathf.Lerp(SmoothedInput[i], currentRawInput[i], Response[i] * Time.deltaTime);
        }

        SmoothedInput = currentSmoothedInput;
    }

    private void UpdateOrientationAndPos()
    {
        for (int i = 0; i < 3; ++i)
        {
            m_transform.localRotation *= Quaternion.AngleAxis(SmoothedInput[i] * Maneuverability[i] * Time.deltaTime, RotationDirections[i]);
        }

        m_transform.localPosition += m_transform.forward * CurrentSpeed * Time.deltaTime;

        Player.localRotation = Quaternion.Slerp(Player.localRotation, m_InitialRotation * Quaternion.AngleAxis(-SmoothedInput.y * MaxBankAngleOnTurn, Vector3.forward), BankAngleSmooth * Time.deltaTime);
    }
}
