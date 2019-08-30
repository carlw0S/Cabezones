using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject timeLimitGO, goalLimitGO, touchControlsButtonGO;
    private Text timeLimitText, goalLimitText, touchControlsText;

    public float timeLimit = 120;
    public float maxTimeLimit = 300;

    public int goalLimit = 7;
    public int maxGoalLimit = 11;

    public bool touchControls = false;



    void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
            Application.targetFrameRate = 60;

        timeLimitText = timeLimitGO.GetComponent<Text>();
        goalLimitText = goalLimitGO.GetComponent<Text>();
        touchControlsText = touchControlsButtonGO.GetComponentInChildren<Text>();
        updateTimeLimit();
        updateGoalLimit();

        if (Input.touchSupported)
            toggleTouchControls();
    }



    public void GotoGameScene()
    {
        GameOptions.timeLimit = timeLimit;
        GameOptions.goalLimit = goalLimit;
        GameOptions.touchControls = touchControls;
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void increaseTimeLimit()
    {
        if (timeLimit < maxTimeLimit)
            timeLimit += 30;
        else
            timeLimit = 0;
        updateTimeLimit();
    }

    public void decreaseTimeLimit()
    {
        if (timeLimit > 0)
            timeLimit -= 30;
        else
            timeLimit = maxTimeLimit;
        updateTimeLimit();
    }

    private void updateTimeLimit()
    {
        if (timeLimit == 0)
            timeLimitText.text = "∞";
        else
            timeLimitText.text = string.Format("{0}:{1:00}", (int) timeLimit / 60, timeLimit % 60);
    }



    public void increaseGoalLimit()
    {
        if (goalLimit < maxGoalLimit)
            goalLimit += 1;
        updateGoalLimit();
    }

    public void decreaseGoalLimit()
    {
        if (goalLimit > 1)
            goalLimit -= 1;
        updateGoalLimit();
    }

    private void updateGoalLimit()
    {
        // if (goalLimit == 1)
        //     goalLimitText.text = "Golden Goal";
        // else
        //     goalLimitText.text = goalLimit.ToString();
        
        goalLimitText.text = goalLimit.ToString();
    }



    public void toggleTouchControls()
    {
        if (touchControls = !touchControls)     // fancy
            touchControlsText.text = "ON";
        else
            touchControlsText.text = "OFF";
    }
}
