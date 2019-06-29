using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public enum Direction { Right, Left };

    public Direction facing;

    // Start is called before the first frame update
    void Start()
    {
        SurfaceEffector2D se = GetComponent<SurfaceEffector2D>();
        if (facing == Direction.Left)
            se.speed *= -1;
    }
}
