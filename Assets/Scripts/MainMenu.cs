using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject timeLimitGO, goalLimitGO, touchControlsButtonGO, itemsButtonGO;
    private Text timeLimitText, goalLimitText, touchControlsText, itemsButtonText;

    public float timeLimit = 120;
    public float maxTimeLimit = 300;

    public int goalLimit = 7;
    public int maxGoalLimit = 11;

    public bool touchControls = false;

    public bool items = true;



    private string easterEgg = new string('w', 5);
    private uint easterEggIndex = 0;

    void Awake()
    {
        if (Application.platform == RuntimePlatform.Android)
            Application.targetFrameRate = 60;

        timeLimitText = timeLimitGO.GetComponent<Text>();
        goalLimitText = goalLimitGO.GetComponent<Text>();
        touchControlsText = touchControlsButtonGO.GetComponentInChildren<Text>();
        itemsButtonText = itemsButtonGO.GetComponentInChildren<Text>();
        UpdateTimeLimit();
        UpdateGoalLimit();

        if (Input.touchSupported)
            ToggleTouchControls();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            easterEggIndex = 0;

        if (Input.GetKeyDown(KeyCode.W))
        {
            
        }
    }



    public void GotoGameScene()
    {
        GameOptions.timeLimit = timeLimit;
        GameOptions.goalLimit = goalLimit;
        GameOptions.touchControls = touchControls;
        GameOptions.items = items;
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void IncreaseTimeLimit()
    {
        if (timeLimit < maxTimeLimit)
            timeLimit += 30;
        else
            timeLimit = 0;
        UpdateTimeLimit();
    }

    public void DecreaseTimeLimit()
    {
        if (timeLimit > 0)
            timeLimit -= 30;
        else
            timeLimit = maxTimeLimit;
        UpdateTimeLimit();
    }

    private void UpdateTimeLimit()
    {
        if (timeLimit == 0)
            timeLimitText.text = "∞";
        else
            timeLimitText.text = string.Format("{0}:{1:00}", (int) timeLimit / 60, timeLimit % 60);
    }



    public void IncreaseGoalLimit()
    {
        if (goalLimit < maxGoalLimit)
            goalLimit += 1;
        UpdateGoalLimit();
    }

    public void DecreaseGoalLimit()
    {
        if (goalLimit > 1)
            goalLimit -= 1;
        UpdateGoalLimit();
    }

    private void UpdateGoalLimit()
    {
        // if (goalLimit == 1)
        //     goalLimitText.text = "Golden Goal";
        // else
        //     goalLimitText.text = goalLimit.ToString();
        
        goalLimitText.text = goalLimit.ToString();
    }



    public void ToggleTouchControls()
    {
        if (touchControls = !touchControls)     // fancy
            touchControlsText.text = "ON";
        else
            touchControlsText.text = "OFF";
    }



    public void ToggleItems()
    {
        if (items = !items)
            itemsButtonText.text = "ON";
        else
            itemsButtonText.text = "OFF";
    }
}
