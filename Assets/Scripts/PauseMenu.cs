using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject noticeGO;
    private Text noticeText;

    public GameObject pauseButtonGO, continueButtonGO, exitButtonGO;



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
        exitButtonGO.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1;

        noticeGO.SetActive(false);
        
        pauseButtonGO.SetActive(true);
        continueButtonGO.SetActive(false);
        exitButtonGO.SetActive(false);
    }

    public void GotoMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
