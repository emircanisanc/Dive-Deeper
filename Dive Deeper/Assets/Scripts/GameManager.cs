using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public void WinGame()
    {
        Debug.Log("You Win");
    }

    public void LoseGame()
    {
        Debug.Log("You Lost");
    }
}
