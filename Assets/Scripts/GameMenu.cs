using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject noticeGO;
    private Text noticeText;

    public GameObject pauseButtonGO, continueButtonGO, exitButtonGO, rematchButtonGO;



    void Awake()
    {
        noticeText = noticeGO.GetComponent<Text>();
    }

    public void Pause()
    {
        Time.timeScale = 0;

        noticeText.text = "Game Paused";
        noticeGO.SetActive(true);

        pauseButtonGO.SetActive(false);
        continueButtonGO.SetActive(true);
        rematchButtonGO.SetActive(true);
        exitButtonGO.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1;

        noticeGO.SetActive(false);
        
        pauseButtonGO.SetActive(true);
        continueButtonGO.SetActive(false);
        rematchButtonGO.SetActive(false);
        exitButtonGO.SetActive(false);
    }

    public void Rematch()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void Win()
    {
        pauseButtonGO.SetActive(false);
        rematchButtonGO.SetActive(true);
        exitButtonGO.SetActive(true);
    }
}
