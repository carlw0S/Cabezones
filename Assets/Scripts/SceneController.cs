using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public float resetTime = 3;

    public GameObject ballGO, lGoalDetectorGO, rGoalDetectorGO, noticeGO, lPlayerGO, rPlayerGO;
    private Ball ball;
    private GoalDetector lGoalDetector, rGoalDetector;
    private Notice notice;
    private PlayerController lPlayer, rPlayer;

    void Start()
    {
        ball = ballGO.GetComponent<Ball>();
        lGoalDetector = lGoalDetectorGO.GetComponent<GoalDetector>();
        rGoalDetector = rGoalDetectorGO.GetComponent<GoalDetector>();
        notice = noticeGO.GetComponent<Notice>();
        lPlayer = lPlayerGO.GetComponent<PlayerController>();
        rPlayer = rPlayerGO.GetComponent<PlayerController>();
    }

    public void ResetAll()
    {
        lGoalDetectorGO.SetActive(false);
        rGoalDetectorGO.SetActive(false);

        Invoke("Reset", resetTime);
    }

    private void Reset()
    {
        ball.Reset();
        lGoalDetector.Reset();
        rGoalDetector.Reset();
        lPlayer.Reset();
        rPlayer.Reset();
        notice.Reset();
    }
}
