using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    public bool IsGameRunning = false;
    public event System.EventHandler OnGameStart;

    public Canvas GameStartScreen;
    public Text CountdownText;
    private bool _timerBegan;
    public float _countdownTime = 3;
    private float _timerStartTime;

    public Canvas GameRunningScreen;
    public float GameDuration;
    private float _gameTimer;
    public Text TimerText;

    public Canvas WinScreen;
    public Canvas LoseScreen;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
        Enable(GameStartScreen, true);
        Enable(GameRunningScreen, false);
        Enable(WinScreen, false);
        Enable(LoseScreen, false);
    }

    private void Update()
    {
        CheckStartTimer();
        if (IsGameRunning)
        {
            GameTimer();
        }
    }

    public void OnAnyKeyPressed(InputValue input) {
        if (!_timerBegan)
        {
            _timerBegan = true;
            _timerStartTime = Time.time;
        }
    }

    void CheckStartTimer() 
    {
        if (GameStartScreen.gameObject.activeSelf && _timerBegan)
        {
            float timer = _countdownTime - (Time.time - _timerStartTime);
            if (timer <= -1)
            {
                Enable(GameStartScreen, false);
            }
            else if (timer <= 0)
            {
                StartGame();
            }
            else
            {
                CountdownText.text = Mathf.CeilToInt(timer).ToString();
            }
        }
    }

    void StartGame()
    {
        CountdownText.text = "Go!";
        IsGameRunning = true;
        Enable(GameRunningScreen, true);
        _gameTimer = GameDuration;
        OnGameStart.Invoke(this, null);
    }

    void GameTimer() 
    {
        _gameTimer -= Time.deltaTime;
        if (_gameTimer <= 0) 
        {
            _gameTimer = 0;
            IsGameRunning = false;
            Enable(WinScreen, true);
            Enable(GameRunningScreen, false);
        }
        TimerText.text = Mathf.Floor(_gameTimer / 60).ToString() + ":" + Mathf.FloorToInt(_gameTimer % 60).ToString().PadLeft(2,'0');
    }


    public void GameLose(Person gameEnder) {
        //Camera zoom in on person.
        Enable(LoseScreen, true);
        Enable(GameRunningScreen, false);
        IsGameRunning = false;
    }

    public void GameWin() {
        _gameTimer = 0;
        IsGameRunning = false;
        Enable(WinScreen, true);
        Enable(GameRunningScreen, false);
    }

    void Enable(Canvas C,bool enable) {
        if (C)
            C.gameObject.SetActive(enable);
        else
            Debug.LogError("GameObject not set");
    }

    public void MainMenuButtonClicked() {
        SceneManager.LoadScene("MainMenu");
    }
}
