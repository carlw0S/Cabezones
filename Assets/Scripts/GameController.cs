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
    private GoalDetector lGoalDetector, rGoalDetector;
    private Text noticeText, timerText;
    private string winnerName = "";
    private bool updateTime = false,
                 timedGame = true;
    private float timeDelay;    // Time that has to be subtracted due to game halts (when a goal occurs, for example),
                                // since I can't set timeScale to 0

    void Awake()
    {
        ball = ballGO.GetComponent<Ball>();
        lPlayer = lPlayerGO.GetComponent<PlayerController>();
        rPlayer = rPlayerGO.GetComponent<PlayerController>();
        lGoalDetector = lGoalDetectorGO.GetComponent<GoalDetector>();
        rGoalDetector = rGoalDetectorGO.GetComponent<GoalDetector>();
        noticeText = noticeGO.GetComponent<Text>();
        timerText = timerGO.GetComponent<Text>();

        if (GameOptions.goalLimit == 1)
            noticeText.text = "Golden Goal!";
        else
            noticeText.text = "First player to score " + GameOptions.goalLimit + " goals wins!";
        noticeGO.SetActive(true);

        if (GameOptions.timeLimit == 0)
            timedGame = false;

        timeDelay = resetTime + ball.kickoffDelay;
        Time.timeScale = 1;
        Invoke("StartUp", resetTime);
    }

    void Update()
    {
        if (updateTime && timedGame)
        {
            float time = GameOptions.timeLimit - (Time.timeSinceLevelLoad - timeDelay);
            if (time <= 0)
            {
                lGoalDetectorGO.SetActive(false);
                rGoalDetectorGO.SetActive(false);

                updateTime = false;
                UpdateTime(0);

                noticeText.text = "Time!";
                noticeGO.SetActive(true);

                if (lGoalDetector.getGoalCount() > rGoalDetector.getGoalCount())
                    winnerName = lPlayer.name;
                else if (lGoalDetector.getGoalCount() < rGoalDetector.getGoalCount())
                    winnerName = rPlayer.name;
                else
                    winnerName = "Everyone";

                Invoke("Win", resetTime);
            }
            else
                UpdateTime(time);
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
        if (GameOptions.timeLimit != 0)
        {
            UpdateTime(GameOptions.timeLimit);
            timerGO.SetActive(true);
        }
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

    private void UpdateTime(float time)
    {
        int totalSeconds = Mathf.FloorToInt(time);
        int seconds = totalSeconds % 60;
        int minutes = totalSeconds / 60;
        int milliseconds = Mathf.FloorToInt((time - totalSeconds) * 1000);

        string timer = string.Format("{0}'{1:00}''{2:000}", minutes, seconds, milliseconds);
        timerText.text = timer;
    }
}
