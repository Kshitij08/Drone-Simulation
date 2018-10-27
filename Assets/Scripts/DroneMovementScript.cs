using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DroneMovementScript : MonoBehaviour {
    Rigidbody ourDrone;
    public VirtualJoystick leftJoystick;
    public VirtualJoystick rightJoystick;


    private bool rightJsJ;
    private bool rightJsL;
    private bool rightJsI;
    private bool rightJsK;


    private void Awake()
    {
        ourDrone = GetComponent<Rigidbody>();
        droneSound = gameObject.transform.Find("drone_sound").GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        MovementUpDown();
        MovementForward();
        Rotation();
        ClampingSpeedValues();
        Swerve();
        DroneSound();
        Joystick();

        ourDrone.AddRelativeForce(Vector3.up * upForce);
        ourDrone.rotation = Quaternion.Euler(
                new Vector3(tiltAmountForward, currentYRotation, tiltAmountSideways)
        );

    }

    private void Joystick()
    {
        //JUGAAD for Right Joystick
        if (rightJoystick.InputDirection.x  < -0.5)
        {
            rightJsJ = true;
        }
        else rightJsJ = false;

        if (rightJoystick.InputDirection.x  > 0.5)
        {
            rightJsL = true;
        }
        else rightJsL = false;

        if (rightJoystick.InputDirection.z  > 0.5)
        {
            rightJsI= true;
        }
        else rightJsI = false;

        if (rightJoystick.InputDirection.z < -0.5)
        {
            rightJsK = true;
        }
        else rightJsK = false;
    }

    public float upForce;
    void MovementUpDown()
    {
        if ((Mathf.Abs(leftJoystick.InputDirection.z) > 0.4f || Mathf.Abs(leftJoystick.InputDirection.x) > 0.4f)) //0.2f
        {
            if (rightJsI || rightJsK)
            {
                ourDrone.velocity = ourDrone.velocity;
            }
            if (!rightJsI && !rightJsK && !rightJsJ && !rightJsL)
            {
                ourDrone.velocity = new Vector3(ourDrone.velocity.x, Mathf.Lerp(ourDrone.velocity.y, 0, Time.deltaTime * 5), ourDrone.velocity.z);
                upForce = 281;
            }
            if (!rightJsI && !rightJsK && (rightJsJ || rightJsL))
            {
                ourDrone.velocity = new Vector3(ourDrone.velocity.x, Mathf.Lerp(ourDrone.velocity.y, 0, Time.deltaTime * 5), ourDrone.velocity.z);
                upForce = 110;
            }
            if (rightJsJ || rightJsL)
            {
                upForce = 410;
            }
        }

        if (Mathf.Abs(leftJoystick.InputDirection.z) < 0.4f && Mathf.Abs(leftJoystick.InputDirection.x) > 0.4f) //0.2f
        {
            upForce = 135;
        }



        if (rightJsI)
        {
            upForce = 450;
            if (Mathf.Abs(leftJoystick.InputDirection.x) > 0.4f) //0.2f
            {
                upForce = 500;
            }
        }
        else if (rightJsK)
        {
            upForce = -200;
        }
        else if (!rightJsI && !rightJsK && (Mathf.Abs(leftJoystick.InputDirection.z) < 0.4f && Mathf.Abs(leftJoystick.InputDirection.x) < 0.4f)) //0.2f
        {
            upForce = 98.1f;
        }
    }

    private float movementForwardSpeed = 500.0f;
    private float tiltAmountForward = 0;
    private float tiltVelocityForward;
    void MovementForward()
    {
        if (leftJoystick.InputDirection.z != 0)
        {
            ourDrone.AddRelativeForce(Vector3.forward * leftJoystick.InputDirection.z * movementForwardSpeed);
            tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, 20 * leftJoystick.InputDirection.z, ref tiltVelocityForward, 0.1f); //20 inplace of 0.5f
        }
    }

    private float wantedYRotation;
    [HideInInspector] public float currentYRotation;
    private float rotateAmountByKeys = 2.5f;
    private float rotationYVelocity;
    void Rotation()
    {
        if (rightJsJ)
        {
            wantedYRotation -= rotateAmountByKeys;
        }
        if (rightJsL)
        {
            wantedYRotation += rotateAmountByKeys;
        }

        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, 0.25f);
    }

    private Vector3 velocityToSmoothDampToZero;
    void ClampingSpeedValues()
    {
        if (Mathf.Abs(leftJoystick.InputDirection.z) > 0.4f && Mathf.Abs(leftJoystick.InputDirection.x) > 0.4f) //0.2f
        {
            ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(leftJoystick.InputDirection.z) > 0.4f && Mathf.Abs(leftJoystick.InputDirection.x) < 0.4f) //0.2f
        {
            ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(leftJoystick.InputDirection.z) < 0.4f && Mathf.Abs(leftJoystick.InputDirection.x) > 0.4f) //0.2f
        {
            ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 5.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(leftJoystick.InputDirection.z) < 0.4f && Mathf.Abs(leftJoystick.InputDirection.x) < 0.4f) //0.2f
        {
            ourDrone.velocity = Vector3.SmoothDamp(ourDrone.velocity, Vector3.zero, ref velocityToSmoothDampToZero, 0.95f);
        }
    }

    private float sideMovementAmount = 300.0f;
    private float tiltAmountSideways;
    private float tiltAmountVelocity;
    void Swerve()
    {
        if (Mathf.Abs(leftJoystick.InputDirection.x) > 0.4f) //0.2f
        {
            ourDrone.AddRelativeForce(Vector3.right * leftJoystick.InputDirection.x * sideMovementAmount);
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, -50 * leftJoystick.InputDirection.x, ref tiltAmountVelocity, 0.1f);
        }
        else
        {
            tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, 0, ref tiltAmountVelocity, 0.1f);
        }
    }

    private AudioSource droneSound;
    void DroneSound()
    {
        droneSound.pitch = 1 + (ourDrone.velocity.magnitude / 100);
    }

    //Rigidbody ourDrone;
    //public VirtualJoystick leftJoystick;
    //public VirtualJoystick rightJoystick;

    //private void Awake()
    //{
    //    ourDrone = GetComponent<Rigidbody>();
    //    droneSound = gameObject.transform.Find("drone_sound").GetComponent<AudioSource>();
    //}

    //private void FixedUpdate()
    //{
    //    MovementUpDown();
    //    MovementForward();
    //    Rotation();
    //    ClampingSpeedValues();
    //    Swerve();
    //    DroneSound();

    //    ourDrone.AddRelativeForce(Vector3.up * upForce);
    //    ourDrone.rotation = Quaternion.Euler(
    //            new Vector3(tiltAmountForward, currentYRotation, tiltAmountSideways)
    //    );
    //}

    //public float upForce;
    //void MovementUpDown()
    //{
    //    if ((Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f))
    //    {
    //        if (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.K))
    //        {
    //            ourDrone.velocity = ourDrone.velocity;
    //        }
    //        if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && !Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.L))
    //        {
    //            ourDrone.velocity = new Vector3(ourDrone.velocity.x, Mathf.Lerp(ourDrone.velocity.y, 0, Time.deltaTime * 5), ourDrone.velocity.z);
    //            upForce = 281;
    //        }
    //        if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L)))
    //        {
    //            ourDrone.velocity = new Vector3(ourDrone.velocity.x, Mathf.Lerp(ourDrone.velocity.y, 0, Time.deltaTime * 5), ourDrone.velocity.z);
    //            upForce = 110;
    //        }
    //        if (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.L))
    //        {
    //            upForce = 410;
    //        }
    //    }

    //    if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
    //    {
    //        upForce = 135;
    //    }



    //    if (Input.GetKey(KeyCode.I))
    //    {
    //        upForce = 450;
    //        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
    //        {
    //            upForce = 500;
    //        }
    //    }
    //    else if (Input.GetKey(KeyCode.K))
    //    {
    //        upForce = -200;
    //    }
    //    else if (!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f))
    //    {
    //        upForce = 98.1f;
    //    }
    //}

    //private float movementForwardSpeed = 500.0f;
    //private float tiltAmountForward = 0;
    //private float tiltVelocityForward;
    //void MovementForward()
    //{
    //    if (Input.GetAxis("Vertical") != 0)
    //    {
    //        ourDrone.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * movementForwardSpeed);
    //        tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, 0.5f * Input.GetAxis("Vertical"), ref tiltVelocityForward, 0.1f); //20 inplace of 0.5f
    //    }
    //}

    //private float wantedYRotation;
    //[HideInInspector] public float currentYRotation;
    //private float rotateAmountByKeys = 2.5f;
    //private float rotationYVelocity;
    //void Rotation()
    //{
    //    if (Input.GetKey(KeyCode.J))
    //    {
    //        wantedYRotation -= rotateAmountByKeys;
    //    }
    //    if (Input.GetKey(KeyCode.L))
    //    {
    //        wantedYRotation += rotateAmountByKeys;
    //    }

    //    currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, 0.25f);
    //}

    //private Vector3 velocityToSmoothDampToZero;
    //void ClampingSpeedValues()
    //{
    //    if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
    //    {
    //        ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
    //    }
    //    if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
    //    {
    //        ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
    //    }
    //    if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
    //    {
    //        ourDrone.velocity = Vector3.ClampMagnitude(ourDrone.velocity, Mathf.Lerp(ourDrone.velocity.magnitude, 5.0f, Time.deltaTime * 5f));
    //    }
    //    if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
    //    {
    //        ourDrone.velocity = Vector3.SmoothDamp(ourDrone.velocity, Vector3.zero, ref velocityToSmoothDampToZero, 0.95f);
    //    }
    //}

    //private float sideMovementAmount = 300.0f;
    //private float tiltAmountSideways;
    //private float tiltAmountVelocity;
    //void Swerve()
    //{
    //    if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
    //    {
    //        ourDrone.AddRelativeForce(Vector3.right * Input.GetAxis("Horizontal") * sideMovementAmount);
    //        tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, -50 * Input.GetAxis("Horizontal"), ref tiltAmountVelocity, 0.1f);
    //    }
    //    else
    //    {
    //        tiltAmountSideways = Mathf.SmoothDamp(tiltAmountSideways, 0, ref tiltAmountVelocity, 0.1f);
    //    }
    //}

    //private AudioSource droneSound;
    //void DroneSound()
    //{
    //    droneSound.pitch = 1 + (ourDrone.velocity.magnitude / 100);
    //}

}





