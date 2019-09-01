using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public float resetTime = 1;
    public static float itemEffectDuration = 10f;

    public GameObject ballGO, lGoalDetectorGO, rGoalDetectorGO, noticeGO, lPlayerGO, rPlayerGO, lGoalCounterGO, rGoalCounterGO, timerGO, touchControlsGO,
                      groundGO, ceilingGO, leftWallGO, rightWallGO;
    public GameMenu gameMenu;
    public GameObject itemPrefab;
    private Ball ball;
    private static PlayerController lPlayer, rPlayer;
    private GoalDetector lGoalDetector, rGoalDetector;
    private Text noticeText, timerText;
    private string winnerName = "";
    private bool updateTime = false,
                 timedGame = true;
    private float timeDelay;    // Time that has to be subtracted due to game halts (when a goal occurs, for example),
                                // since I can't set timeScale to 0
    private int itemSpawnProbability = 25;

    void Awake()
    {
        ball = ballGO.GetComponent<Ball>();
        lPlayer = lPlayerGO.GetComponent<PlayerController>();
        rPlayer = rPlayerGO.GetComponent<PlayerController>();
        lGoalDetector = lGoalDetectorGO.GetComponent<GoalDetector>();
        rGoalDetector = rGoalDetectorGO.GetComponent<GoalDetector>();
        noticeText = noticeGO.GetComponent<Text>();
        timerText = timerGO.GetComponent<Text>();

        if (GameOptions.touchControls)
            touchControlsGO.SetActive(true);
        else
            touchControlsGO.SetActive(false);

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

        if (GameOptions.items)
            InvokeRepeating("SpawnItem", 1f, 1f);
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

                if (lGoalDetector.GetGoalCount() > rGoalDetector.GetGoalCount())
                    winnerName = lPlayer.name;
                else if (lGoalDetector.GetGoalCount() < rGoalDetector.GetGoalCount())
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
        lPlayer.ResetPosition();
        rPlayer.ResetPosition();
        ball.ResetPosition();
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
        // int milliseconds = Mathf.FloorToInt((time - totalSeconds) * 1000);

        // string timer = string.Format("{0}'{1:00}''{2:000}", minutes, seconds, milliseconds);
        string timer = string.Format("{0}:{1:00}", minutes, seconds);
        timerText.text = timer;
    }




    /* ITEMS */

    private void SpawnItem()
    {
        if (Random.Range(0, itemSpawnProbability) == 0)
        {
            itemSpawnProbability += 25;

            Vector3 position = Vector3.zero;
            float width = itemPrefab.transform.localScale.x;
            float height = itemPrefab.transform.localScale.y;
            // The item will spawn in the upper half of the game field
            position.x = Random.Range((leftWallGO.transform.position.x + leftWallGO.transform.localScale.x / 2) + (width / 2), 
                                    (rightWallGO.transform.position.x - rightWallGO.transform.localScale.x / 2) - (width / 2));
            position.y = Random.Range(0f + (height / 2), 
                                    (ceilingGO.transform.position.y - ceilingGO.transform.localScale.y / 2) - (height / 2));

            Instantiate(itemPrefab, position, Quaternion.identity);
        }
        else
            if (itemSpawnProbability > 0)
                itemSpawnProbability--;
    }

    public static void ActivateItem(int itemID, string playerTag)
    {
        PlayerController player;
        if (playerTag.Equals("Player1"))
            player = lPlayer;
        else
            player = rPlayer;

        switch (itemID)
        {
            case 1: Grow(player);   break;
            case 2: Shrink(player); break;

            default: Debug.Log("Wrong item ID: " + itemID); break;
        }
    }

    private static void Grow(PlayerController player)
    {
        player.gameObject.transform.localScale *= 1.5f;
        player.ResetSize(itemEffectDuration);
    }

    private static void Shrink(PlayerController player)
    {
        player.gameObject.transform.localScale /= 1.5f;
        player.ResetSize(itemEffectDuration);
    }
}
