using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public enum Direction { Right, Left };

    public Direction facing;
    public GameObject goalCounter;

    void Start()
    {
        SurfaceEffector2D se = GetComponent<SurfaceEffector2D>();

        // Change the direction in which the ball will be moved if it lands over the goal
        if (facing == Direction.Left)
            se.speed *= -1;
    }
}
