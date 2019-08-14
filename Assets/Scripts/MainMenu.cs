using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject timeLimitGO;
    private Text timeLimitText;

    public float timeLimit = 120;
    public float maxTimeLimit = 300;

    

    void Awake()
    {
        timeLimitText = timeLimitGO.GetComponent<Text>();
        updateTimeLimit();
    }



    public void GotoGameScene()
    {
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
}
