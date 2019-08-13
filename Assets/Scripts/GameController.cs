using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public float resetTime = 1;
    public int goalsToWin = 7;

    public GameObject ballGO, lGoalDetectorGO, rGoalDetectorGO, noticeGO, lPlayerGO, rPlayerGO, lGoalCounterGO, rGoalCounterGO;
    public GameMenu gameMenu;
    private Ball ball;
    private PlayerController lPlayer, rPlayer;
    private Text noticeText;
    private string winnerName;

    void Awake()
    {
        ball = ballGO.GetComponent<Ball>();
        lPlayer = lPlayerGO.GetComponent<PlayerController>();
        rPlayer = rPlayerGO.GetComponent<PlayerController>();
        noticeText = noticeGO.GetComponent<Text>();

        noticeText.text = "First player to score " + goalsToWin + " goals wins!";
        noticeGO.SetActive(true);

        Time.timeScale = 1;
        Invoke("StartUp", resetTime);
    }



    public void Goal(bool win, string playerName)
    {
        lGoalDetectorGO.SetActive(false);
        rGoalDetectorGO.SetActive(false);

        if (win)
        {
            winnerName = playerName;
            Invoke("Win", resetTime);
        }
        else
        {
            Invoke("ResetAll", resetTime);
        }
    }

    private void StartUp()
    {
        lGoalCounterGO.SetActive(true);
        rGoalCounterGO.SetActive(true);
        ResetAll();
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

    private void Win()
    {
        noticeText.text = winnerName + " wins!";
        noticeGO.SetActive(true);
        gameMenu.Win();
    }
}
