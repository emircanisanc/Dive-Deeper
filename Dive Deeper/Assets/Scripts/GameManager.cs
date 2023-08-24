using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    bool isGameEnd;
    bool isGamePaused;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }

    public void WinGame()
    {
        isGameEnd = true;
        Debug.Log("You Win");
    }

    public void LoseGame()
    {
        isGameEnd = true;
        Debug.Log("You Lost");
    }

    public void PauseGame()
    {
        if (isGameEnd)
            return;

        if (isGamePaused)
            return;

        isGamePaused = true;
        Time.timeScale = 0f;
    }

    public void UnPauseGame()
    {
        if (isGameEnd)
            return;

        if (!isGamePaused)
            return;

        isGamePaused = false;
        Time.timeScale = 1f;
    }
}
