using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalDetector : MonoBehaviour
{
    public GameObject gameControllerGO, noticeGO, goalCounterGO;

    private GameController gameController;
    private Text noticeText;
    private Text goalCounterText;



    private int goalCount;

    void Awake()
    {
        goalCounterText = goalCounterGO.GetComponent<Text>();
        noticeText = noticeGO.GetComponent<Text>();
        gameController = gameControllerGO.GetComponent<GameController>();
    }

    void Start()
    {
        goalCount = 0;
        goalCounterText.text = goalCount.ToString();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            noticeText.text = "golaso";
            noticeGO.SetActive(true);

            ++goalCount;
            goalCounterText.text = goalCount.ToString();

            gameController.Goal();
        }
    }

    public void Reset()
    {
        gameObject.SetActive(true);
    }
}
