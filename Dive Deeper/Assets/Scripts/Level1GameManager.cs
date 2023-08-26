using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1GameManager : GameManager
{
    public GameObject doorToClose;
    public GameObject bossEnemy;

    public override void StartPhaseTwo()
    {
        if (!phaseOne)
            return;
        
        phaseOne = false;
        ImportantMessager.Instance.ShowMessage("KILL THE BOSS");
        if (doorToClose)
        {
            doorToClose.SetActive(false);
        }
        bossEnemy.SetActive(true);
    }
}
