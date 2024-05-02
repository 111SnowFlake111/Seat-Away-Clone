using EventDispatcher;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelController : MonoBehaviour
{
    public List<GameObject> levels = new List<GameObject>();

    public GameObject level;
    void Start()
    {
        if(level != null)
        {
            Destroy(level);
        }

        level = Instantiate(levels[UseProfile.ChosenLevel], new Vector3(0, -115f, 0), Quaternion.identity);

        GamePlayController.Instance.gameScene.InitState();
        GamePlayController.Instance.playerContain.inputController.GetCurrentLevel();
    }


    
}
