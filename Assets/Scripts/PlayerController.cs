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

    public Vector2 jumpForce = new Vector2(0, 1150);    // Just enough to jump a "tile"
    public Vector2 walkForce = new Vector2(100, 0);
    public float maxWalkSpeed = 5;      // Speed cap, grounded and airborne
    public float decceleration = 1.25f;     // To stop moving gradually without linear drag
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
        // Jumping
        if (Input.GetButtonDown(JumpButton))
        {
            Jump();
        }

        // Horizontal movement
        if (Input.GetButton(RightButton) && !Input.GetButton(LeftButton))
        {
            Move(Direction.Right);
        }
        else if (Input.GetButton(LeftButton) && !Input.GetButton(RightButton))
        {
            Move(Direction.Left);
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

    private void Move(Direction dir)
    {
        float xVelocity = rb.velocity.x;
        Vector2 wForce = walkForce;
        
        if (dir == Direction.Left)
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
        rb.velocity = new Vector2(rb.velocity.x / decceleration, rb.velocity.y);    
        // Using velocity and not friction because the player has to stop in the air too
        // Furthermore, linear drag affects in all directions, not just horizontally
    }

    private void Kick()
    {
        // To prevent the accumulation of force on the foot...
        if (m.maxMotorTorque > 2)
            m.maxMotorTorque /= 1.25f;
        if (Input.GetButtonDown(KickButton) || Input.GetButtonUp(KickButton))
            m.maxMotorTorque = kickMaxMotorForce;

        // The angles change depending on where the Player is facing
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

        // Only update the joint motor if there have been any changes since last update
        if (m.motorSpeed != foot.motor.motorSpeed || m.maxMotorTorque != foot.motor.maxMotorTorque)
            foot.motor = m;     
    }

}
