using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject noticeGO;
    private Text notice;

    public GameObject pauseGO, continueGO, exitGO;



    void Start()
    {
        notice = noticeGO.GetComponent<Text>();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        notice.text = "Game Paused";
        noticeGO.SetActive(true);
        pauseGO.SetActive(false);
        continueGO.SetActive(true);
        exitGO.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        noticeGO.SetActive(false);
        pauseGO.SetActive(true);
        continueGO.SetActive(false);
        exitGO.SetActive(false);
    }

    public void GotoMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
