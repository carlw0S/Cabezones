using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public float resetTime = 1;

    public GameObject ballGO, lGoalDetectorGO, rGoalDetectorGO, noticeGO, lPlayerGO, rPlayerGO, lGoalCounterGO, rGoalCounterGO, timerGO;
    public GameMenu gameMenu;
    private Ball ball;
    private PlayerController lPlayer, rPlayer;
    private Text noticeText, timerText;
    private string winnerName = "";
    private bool updateTime = false;
    private float timeDelay;    // Time that has to be subtracted due to game halts (when a goal occurs, for example),
                                // since I can't set timeScale to 0

    void Awake()
    {
        ball = ballGO.GetComponent<Ball>();
        lPlayer = lPlayerGO.GetComponent<PlayerController>();
        rPlayer = rPlayerGO.GetComponent<PlayerController>();
        noticeText = noticeGO.GetComponent<Text>();
        timerText = timerGO.GetComponent<Text>();
        timerText.text = "0'00''000";

        if (GameOptions.goalLimit == 1)
            noticeText.text = "Golden Goal!";
        else
            noticeText.text = "First player to score " + GameOptions.goalLimit + " goals wins!";
        noticeGO.SetActive(true);

        timeDelay = resetTime + ball.kickoffDelay;
        Time.timeScale = 1;
        Invoke("StartUp", resetTime);
    }

    void Update()
    {
        if (updateTime)
        {
            float time = Time.timeSinceLevelLoad - timeDelay;
            int totalSeconds = Mathf.FloorToInt(time);
            int seconds = totalSeconds % 60;
            int minutes = totalSeconds / 60;
            int milliseconds = Mathf.FloorToInt((time - totalSeconds) * 1000);

            string timer = string.Format("{0}'{1:00}''{2:000}", minutes, seconds, milliseconds);
            timerText.text = timer;
        }
    }



    public void Goal(bool win, string playerName)
    {
        updateTime = false;
        timeDelay += (resetTime + ball.kickoffDelay);

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
        timerGO.SetActive(true);
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

        Invoke("UpdateTime", ball.kickoffDelay);
    }

    private void Win()
    {
        noticeText.text = winnerName + " wins!";
        noticeGO.SetActive(true);
        gameMenu.Win();
    }

    private void UpdateTime()
    {
        updateTime = true;
    }
}
