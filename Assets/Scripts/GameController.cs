using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float resetTime = 3;

    public GameObject ballGO, lGoalDetectorGO, rGoalDetectorGO, noticeGO, lPlayerGO, rPlayerGO;
    private Ball ball;
    private PlayerController lPlayer, rPlayer;

    void Awake()
    {
        ball = ballGO.GetComponent<Ball>();
        lPlayer = lPlayerGO.GetComponent<PlayerController>();
        rPlayer = rPlayerGO.GetComponent<PlayerController>();

        Time.timeScale = 1;
    }

    public void Goal()
    {
        lGoalDetectorGO.SetActive(false);
        rGoalDetectorGO.SetActive(false);

        Invoke("ResetAll", resetTime);
    }

    private void ResetAll()
    {
        lPlayer.Reset();
        rPlayer.Reset();
        ball.Reset();
        lGoalDetectorGO.SetActive(true);
        rGoalDetectorGO.SetActive(true);
        noticeGO.SetActive(false);
    }
}
