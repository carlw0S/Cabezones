using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    private Transform t;
    private Vector3 initialPos;
    private float joystickZ;
    private float joystickTilt;

    public GameController gameController;
    public float joystickSensitivity = 1f;

    void Awake()
    {
        t = GetComponent<Transform>();

        initialPos = t.position;
        joystickZ = initialPos.z;
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            Vector3 touchWorldPos = Camera.main.ScreenToWorldPoint(touch.position);
            touchWorldPos.z = joystickZ;
            if (Vector2.Distance(touchWorldPos, t.position) < (t.localScale.x * t.parent.localScale.x / 2 + 0.75f))    // Touch is within the range of the joystick
            {
                t.position = touchWorldPos;

                joystickTilt = Vector2.Distance(t.position, t.parent.position) * joystickSensitivity;

                if (joystickTilt > 1)
                    joystickTilt = 1;

                if (t.position.x < t.parent.position.x)
                    joystickTilt *= -1;

                if (touch.phase == TouchPhase.Ended)
                {
                    t.position = initialPos;
                    joystickTilt = 0;
                }
            }
        }
    }

    // void OnMouseDrag()
    // {
    //     Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     mouseWorldPos.z = joystickZ;
    //     t.position = mouseWorldPos;

    //     joystickTilt = Vector2.Distance(t.position, t.parent.position) * joystickSensitivity;

    //     if (joystickTilt > 1)
    //         joystickTilt = 1;

    //     if (t.position.x < t.parent.position.x)
    //         joystickTilt *= -1;
    // }

    // void OnMouseUpAsButton()
    // {
    //     t.position = initialPos;
    //     joystickTilt = 0;
    // }



    public float GetJoystickTilt()
    {
        return joystickTilt;
    }
}
