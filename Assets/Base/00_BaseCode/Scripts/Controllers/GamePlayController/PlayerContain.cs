using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContain : MonoBehaviour
{
    public InputController inputController;
    [NonSerialized] public bool gameStart = false;
    [NonSerialized] public bool lose = false;
    [NonSerialized] public bool win = false;

    public void StartGame()
    {
        gameStart = true;
        GamePlayController.Instance.gameScene.StartTimer();
    }
}
