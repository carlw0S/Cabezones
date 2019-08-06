using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Direction facing;

    private SurfaceEffector2D se;

    void Awake()
    {
        se = GetComponent<SurfaceEffector2D>();
    }

    void Start()
    {
        // Change the direction in which the ball will be moved if it lands over the goal
        if (facing == Direction.Left)
            se.speed *= -1;
    }
}
