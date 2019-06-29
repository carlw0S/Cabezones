using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetector : MonoBehaviour
{

    private int goalCount;

    void Start()
    {
        goalCount = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            ++goalCount;
            Debug.Log(goalCount);
        }
    }
}
