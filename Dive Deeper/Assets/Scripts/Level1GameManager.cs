using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1GameManager : GameManager
{
    public GameObject doorToClose;
    public GameObject bossEnemy;

    public override void StartPhaseTwo()
    {
        Message.ShowMessageInstance("KILL THE BOSS");
        doorToClose.SetActive(false);
        bossEnemy.SetActive(true);
    }
}
