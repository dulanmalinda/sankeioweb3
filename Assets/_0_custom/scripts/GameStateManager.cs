using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [Header("states")]
    public bool isPlaying;
    public bool isGameOver;

    [Header("Menu Attachments")]
    public GameObject menuUI;
    public GameObject inPlayUI;
    public GameObject gameOverUI;

    [Header("Attachments")]
    public GameObject snakemanager;

    public static GameStateManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        isPlaying = false;
        isGameOver = false;

        menuUI.SetActive(true);
        inPlayUI.SetActive(false);
        gameOverUI.SetActive(false);
    }

    void Update()
    {
        if (isGameOver && isPlaying)
        {
            isPlaying = false;
        }
    }

    public void enablePlay()
    {
        isPlaying = true;
        snakemanager.SetActive(true);

        menuUI.SetActive(false);
        inPlayUI.SetActive(true);
        gameOverUI.SetActive(false);
    }

    public void endGame()
    {
        menuUI.SetActive(false);
        inPlayUI.SetActive(false);
        gameOverUI.SetActive(true);

        isGameOver = true;
    }
}
