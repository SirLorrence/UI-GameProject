using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text timer;
    public Text resultTime;
    public float totalTime;
    public bool timerIsLive;

    private GameObject[] backgroundText;

    public Canvas mainMenu;
    public Canvas gameplay;
    public Canvas pauseMenu;
    public Canvas optionsMenu;
    public Canvas resultsMenu;

    public enum GameStates
    {
        main,
        game,
        pause,
        options,
        results,
    }

    public GameStates state;
    private bool runTimer;
    private bool darkMode = true;

    private static GameManager Instance { get; set; }

    #region Singleton

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this.gameObject);
        else Instance = this;
    }

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        backgroundText = GameObject.FindGameObjectsWithTag("CanvasText");

        ResetAll();
        state = GameStates.main;
        Manager((int) state);
    }

    // Update is called once per frame
    void Update()
    {
        if (runTimer) Timer();
    }


    public void Manager(int i)
    {
        switch (i)
        {
            case (int) GameStates.main:
                ResetAll();
                mainMenu.enabled = true;
                break;
            case (int) GameStates.game:
                Game();
                break;
            case (int) GameStates.pause:
                pauseMenu.enabled = true;
                Time.timeScale = 0;
                break;
            case (int) GameStates.options:
                ResetAll();
                optionsMenu.enabled = true;
                break;
            case (int) GameStates.results:
                GetResults();
                ResetAll();
                resultsMenu.enabled = true;
                break;

            default:
                ResetAll();
                break;
        }

        state = (GameStates) i;
    }

    public void DarkModeEnable()
    {
        if (darkMode)
        {
            Camera.main.backgroundColor = Color.black;
            foreach (var text in backgroundText)
            {
                text.GetComponent<Text>().color = Color.white;
            }

            darkMode = false;
        }
        else
        {
            Camera.main.backgroundColor = Color.white;
            foreach (var text in backgroundText)
            {
                text.GetComponent<Text>().color = Color.black;
            }

            darkMode = true;
        }
    }

    public void QuitGame()
    {
        Application.Quit(0);
    }


    private void Timer()
    {
        totalTime += Time.deltaTime;

        var seconds = (int) (totalTime % 60);
        var minutes = Mathf.FloorToInt(totalTime / 60);
        /*
         * the first number is the variable you want the second(00) is the placeholder
         */
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void GetResults()
    {
        runTimer = false;
        var seconds = (int) (totalTime % 60);
        var minutes = Mathf.FloorToInt(totalTime / 60);
        resultTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void Game()
    {
        if (gameplay.enabled != true)
        {
            ResetAll();
            gameplay.enabled = true;
        }

        pauseMenu.enabled = false;
        Time.timeScale = 1;
        runTimer = true;
    }

    void ResetAll()
    {
        mainMenu.enabled = false;
        gameplay.enabled = false;
        pauseMenu.enabled = false;
        optionsMenu.enabled = false;
        resultsMenu.enabled = false;
        totalTime = 0;
    }
}