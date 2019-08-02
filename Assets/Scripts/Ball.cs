using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public int kickoffSpeed = 2;
    public float maxKickoffAngle = 30;

    private Transform t;
    private Rigidbody2D rb;
    private Vector3 initialPos;

    void Awake()
    {
        t = GetComponent<Transform>();
        initialPos = t.position;
        
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Reset();
    }



    public void Reset()
    {
        rb.velocity = new Vector2();
        t.position = initialPos;

        float angle = Random.Range(-maxKickoffAngle, maxKickoffAngle);
        float rad = angle * Mathf.PI / 180;

        Vector2 kickoff = new Vector2(kickoffSpeed * Mathf.Sin(rad), kickoffSpeed * Mathf.Cos(rad));
        rb.AddForce(kickoff);
    }
}
