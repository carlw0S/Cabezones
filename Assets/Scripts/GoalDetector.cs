﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalDetector : MonoBehaviour
{
    public GameObject gameControllerGO, noticeGO, oppositeGoalCounterGO, oppositePlayerGO;

    private GameController gameController;
    private Text noticeText;
    private Text goalCounterText;



    private int goalCount;

    void Awake()
    {
        goalCounterText = oppositeGoalCounterGO.GetComponent<Text>();
        noticeText = noticeGO.GetComponent<Text>();
        gameController = gameControllerGO.GetComponent<GameController>();
        
        goalCount = 0;
        goalCounterText.text = goalCount.ToString();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            noticeText.text = "Goal for " + oppositePlayerGO.name + "!";
            noticeGO.SetActive(true);

            ++goalCount;
            goalCounterText.text = goalCount.ToString();

            gameController.Goal(goalCount == GameOptions.goalLimit, oppositePlayerGO.name);
        }
    }

    public void Reset()
    {
        gameObject.SetActive(true);
    }

    public int GetGoalCount()
    {
        return goalCount;
    }
}
