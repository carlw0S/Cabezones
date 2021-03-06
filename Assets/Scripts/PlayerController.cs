﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Direction facing;
    public string JumpButton;
    public string KickButton;
    public string HorizontalAxis;
    public Joystick joystick;
    public GameObject jumpTouchButtonGO, kickTouchButtonGO;

    public Vector2 jumpForce = new Vector2(0, 1150);    // Just enough to jump a "tile"
    public Vector2 walkForce = new Vector2(100, 0);
    public float maxWalkSpeed = 5;      // Speed cap, on ground and airborne
    public float horizontalDecceleration = 1.25f;     // To stop moving gradually without linear drag
    public float kickMotorSpeed = 1080;
    public float kickMaxMotorForce = 120;
    public float footDeadzone = 0.05f;
    public float footDecceleration = 1.75f;




    private Rigidbody2D rb;
    private HingeJoint2D foot;
    private bool onGround;
    private JointMotor2D m;
    private Transform t;
    private Vector3 initialPos, initialScale;
    private bool jumpTouchButtonPressed = false,
                 jumpTouchButtonHeld = false;       // To prevent repeated jumps by holding the virtual jump button
    private bool kickTouchButtonPressed = false,
                 kickTouchButtonPressedLastUpdate = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        foot = GetComponentInChildren<HingeJoint2D>();

        t = GetComponent<Transform>();
        initialPos = t.position;
        initialScale = t.localScale;

        m = foot.motor;
        m.maxMotorTorque = kickMaxMotorForce;

        // Change the side of the foot, depending on the direction the player is facing
        JointAngleLimits2D limits = foot.limits;
        if (facing == Direction.Right)
        {
            limits.min = -15;
            limits.max = 105;
            foot.limits = limits;
        }
        else
        {
            limits.min = 195;
            limits.max = 75;
            foot.limits = limits;
        }
    }

    void Start()
    {
        if (facing == Direction.Right)
            t.position = new Vector3(-15, 0, 0);
        else
            t.position = new Vector3(15, 0, 0);
    }

    void Update()
    {
        // This was in FixedUpdate initially, but it wouldn't get recognised with high FPS

        if (GameOptions.touchControls)
        {
            jumpTouchButtonPressed = DetectTouchButtonPress(jumpTouchButtonGO);

            if (jumpTouchButtonPressed)
            {
                if (!jumpTouchButtonHeld)
                {
                    jumpTouchButtonHeld = true;
                    Jump();
                }
            }
            else
            {
                if (onGround)
                    jumpTouchButtonHeld = false;
            }
        }
        else
        {
            if (Input.GetButtonDown(JumpButton))
            {
                Jump();
            }
        }
    }

    void FixedUpdate()
    {
        Move();
        Kick();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            onGround = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            onGround = false;
    }
    
    
    
    

    private void Move()
    {
        // Horizontal movement
        float hAxis;
        if (GameOptions.touchControls)
            hAxis = joystick.GetJoystickTilt();
        else
            hAxis = Input.GetAxis(HorizontalAxis);

        if (hAxis != 0)
        {
            Move(hAxis);
        }
        else
        {
            Stop();
        }
    }

    private void Jump()
    {
        if (onGround)
        {
            onGround = false;
            rb.AddForce(jumpForce);
        }
    }

    private void Move(float joystickTilt)
    {
        float xVelocity = rb.velocity.x;
        Vector2 wForce = walkForce * Mathf.Abs(Mathf.Pow(joystickTilt, 2));
        
        if (joystickTilt < 0)   // Moving left
        {
            xVelocity *= -1;
            wForce *= -1;
        }

        if (xVelocity < maxWalkSpeed)      // Speed cap
        {
            if (xVelocity < 0)                 // Artificially double the Player force when another one pushes them in the opposite direction, in order to stop both
                rb.AddForce(wForce * 2);        // (maybe kinda rough, but it works reasonably well lol)
            else rb.AddForce(wForce);
        }
    }

    private void Stop()
    {
        rb.velocity = new Vector2(rb.velocity.x / horizontalDecceleration, rb.velocity.y);    
        // Using velocity and not friction because the player has to stop in the air too
        // Furthermore, linear drag affects in all directions, not just horizontally
    }

    private void Kick()
    {
        kickTouchButtonPressed = DetectTouchButtonPress(kickTouchButtonGO);

        // To prevent the accumulation of force on the foot...
        // decrease the foot "strength" in each update
        if (m.maxMotorTorque > 2)
            m.maxMotorTorque /= 1.25f;

        // Reset the foot "strength" to its original value when starting or stopping kicking
        if (GameOptions.touchControls)
        {
            if ((kickTouchButtonPressed && !kickTouchButtonPressedLastUpdate) ||
                (!kickTouchButtonPressed && kickTouchButtonPressedLastUpdate))
            {
                m.maxMotorTorque = kickMaxMotorForce;
            }

            if (kickTouchButtonPressed)
                kickTouchButtonPressedLastUpdate = true;
            else
                kickTouchButtonPressedLastUpdate = false;
        }
        else
        {
            if (Input.GetButtonDown(KickButton) || Input.GetButtonUp(KickButton))
                m.maxMotorTorque = kickMaxMotorForce;
        }

        // The angles change depending on where the Player is facing
        float stretchLevel;
        if (facing == Direction.Right)
        {
            stretchLevel = 1 - ((foot.jointAngle + 15) / 120);     
        }
        else
        {
            stretchLevel = (foot.jointAngle - 75) / 120;
        }
        FootMovement(stretchLevel);

        // Only update the foot motor if there have been any changes since the last update
        if (m.motorSpeed != foot.motor.motorSpeed || m.maxMotorTorque != foot.motor.maxMotorTorque)
            foot.motor = m;
    }

    private void FootMovement(float stretchLevel)
    // stretchLevel is used to gradually increase/decrease the foot speed depending on the relative angle (0 is not stretched, 1 is fully stretched)
    {
        float fSpeed = kickMotorSpeed;
        if (facing == Direction.Left)
            fSpeed *= -1;


        bool kickButtonHeld = false;
        if (GameOptions.touchControls)
        {
            kickButtonHeld = kickTouchButtonPressed;
        }
        else
        {
            kickButtonHeld = Input.GetButton(KickButton);
        }

        if (kickButtonHeld)
        {
            // If the foot isn't fully stretched...
            if (stretchLevel < (1 - footDeadzone))    // The deadzone activates the else clause earlier (the foot speed is set to 0 before fully stretched)
                m.motorSpeed = -fSpeed * Mathf.Pow((1 - stretchLevel), footDecceleration);      // Gradually stretch the foot
            else    // If it's fully stretched, stop the foot
                m.motorSpeed = 0;
        }
        else
        {
            // If the foot is somewhat stretched...
            if (stretchLevel > footDeadzone)
                m.motorSpeed = fSpeed * Mathf.Pow(stretchLevel, footDecceleration);     // Gradually "retract" the foot
            else    // If it isn't stretched, stop the foot
                m.motorSpeed = 0;
        }
    }

    private bool DetectTouchButtonPress(GameObject touchButton)
    {
        bool touchButtonPressed = false;
        foreach (Touch touch in Input.touches)
        {
            Vector3 touchWorldPos = Camera.main.ScreenToWorldPoint(touch.position);
            if (touchWorldPos.x > (touchButton.transform.position.x - touchButton.transform.localScale.x / 2) &&
                touchWorldPos.x < (touchButton.transform.position.x + touchButton.transform.localScale.x / 2) &&
                touchWorldPos.y > (touchButton.transform.position.y - touchButton.transform.localScale.y / 2) &&
                touchWorldPos.y < (touchButton.transform.position.y + touchButton.transform.localScale.y / 2))    // Touch is within the range of the jump button
            {
                touchButtonPressed = true;
                break;
            }
        }

        return touchButtonPressed;
    }



    public void ResetPosition()
    {
        t.position = initialPos;
    }

    public void ResetSize(float delay)
    {
        Invoke("ResetSize", delay);
    }
    private void ResetSize()    // polymorphism is amazing
    {
        t.localScale = initialScale;
    }

}
