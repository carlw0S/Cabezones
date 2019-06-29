using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Direction { Right, Left };

    public Direction facing;
    public string LeftButton;
    public string RightButton;
    public string JumpButton;
    public string KickButton;

    public Vector2 baseJumpForce = new Vector2(0, 1000);
    public Vector2 baseWalkForce = new Vector2(100, 0);
    public float maxWalkSpeed = 5;
    public float decceleration = 1.25f;
    public float kickMotorSpeed = 1080;
    public float kickMaxMotorForce = 120;



    private Rigidbody2D rb;
    private HingeJoint2D foot;
    private bool onGround;
    private JointMotor2D m;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        foot = GetComponentInChildren<HingeJoint2D>();
        m = foot.motor;
        m.maxMotorTorque = kickMaxMotorForce;

        // change the side of the foot
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

    void FixedUpdate()
    {
        Move();      // amazing
        Kick();

        // VALORES RELATIVOSSSSSSSSSSSSSSSSSSSSSSSSSS ???
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
        // jumping
        if (Input.GetButtonDown(JumpButton))
        {
            Jump();
        }

        // walking
        if (Input.GetButton(LeftButton) && !Input.GetButton(RightButton))
        {
            MoveLeft();
        }
        else if (Input.GetButton(RightButton) && !Input.GetButton(LeftButton))
        {
            MoveRight();
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
            rb.AddForce(baseJumpForce);
        }
    }

    private void MoveLeft()
    {
        if (rb.velocity.x > -maxWalkSpeed)      // speed cap
        {
            if (rb.velocity.x > 0)                  // in order to counter a tackle...
                rb.AddForce(-baseWalkForce * 2);
            else rb.AddForce(-baseWalkForce);
        }
    }

    private void MoveRight()
    {
        if (rb.velocity.x < maxWalkSpeed)       // speed cap
        {
            if (rb.velocity.x < 0)                  // in order to counter a tackle...
                rb.AddForce(baseWalkForce * 2);
            else rb.AddForce(baseWalkForce);
        }
    }

    private void Stop()
    {
        rb.velocity = new Vector2(rb.velocity.x / decceleration, rb.velocity.y);    
        // using velocity and not friction because the player has to stop in the air too
        // furthermore, linear drag affects in all directions
    }

    private void Kick()
    {
        // to prevent the accumulation of force on the foot...
        if (m.maxMotorTorque > 2)
            m.maxMotorTorque /= 1.25f;
        if (Input.GetButtonDown(KickButton) || Input.GetButtonUp(KickButton))
            m.maxMotorTorque = kickMaxMotorForce;

        if (facing == Direction.Right)
        {
            if (Input.GetButton(KickButton))
            {
                if (foot.jointAngle > (foot.limits.min + 5))
                    m.motorSpeed = -kickMotorSpeed * Mathf.Pow(((foot.jointAngle + 15) / 120), 1.25f);
                else
                    m.motorSpeed = 0;
            }
            else
            {
                if (foot.jointAngle < (foot.limits.max - 5))
                    m.motorSpeed = kickMotorSpeed * Mathf.Pow((1 - ((foot.jointAngle + 15) / 120)), 1.25f);
                else
                    m.motorSpeed = 0;
            }
        }
        else
        {
            if (Input.GetButton(KickButton))
            {
                if (foot.jointAngle < (foot.limits.min - 5))
                    m.motorSpeed = kickMotorSpeed * Mathf.Pow((1 - ((foot.jointAngle - 75) / 120)), 1.25f);
                else
                    m.motorSpeed = 0;
            }
            else
            {
                if (foot.jointAngle > (foot.limits.max + 5))
                    m.motorSpeed = -kickMotorSpeed * Mathf.Pow(((foot.jointAngle - 75) / 120), 1.25f);
                else
                    m.motorSpeed = 0;
            }
        }

        if (m.motorSpeed != foot.motor.motorSpeed || m.maxMotorTorque != foot.motor.maxMotorTorque)
            foot.motor = m;     // only update the joint motor if there have been any changes
    }

}
