using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalDetector : MonoBehaviour
{
    public GameObject sceneControllerGO;
    private SceneController sceneController;

    public GameObject notice;
    private Text noticeText;


    public GameObject goalCounter;
    private Text goalCounterText;


    private int goalCount;

    void Start()
    {
        goalCounterText = goalCounter.GetComponent<Text>();
        goalCount = 0;
        goalCounterText.text = goalCount.ToString();

        noticeText = notice.GetComponent<Text>();

        sceneController = sceneControllerGO.GetComponent<SceneController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            noticeText.text = "golaso";
            notice.SetActive(true);
            ++goalCount;
            goalCounterText.text = goalCount.ToString();

            sceneController.ResetAll();
        }
    }

    public void Reset()
    {
        gameObject.SetActive(true);
    }
}
