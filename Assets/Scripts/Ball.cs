using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public int kickoffSpeed = 2;
    public float maxKickoffAngle = 30;
    public float kickoffDelay = 1;

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
        t.position = new Vector3(15, 0, 0);
    }



    public void Reset()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.gravityScale = 0;
        
        t.position = initialPos;

        Invoke("KickOff", kickoffDelay);
    }

    private void KickOff()
    {
        float angle = Random.Range(-maxKickoffAngle, maxKickoffAngle);
        float rad = angle * Mathf.PI / 180;
        Vector2 kickoff = new Vector2(kickoffSpeed * Mathf.Sin(rad), kickoffSpeed * Mathf.Cos(rad));

        rb.gravityScale = 1;
        rb.AddForce(kickoff);
    }
}
