using EventDispatcher;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelController : MonoBehaviour
{
    public List<GameObject> levels;

    public GameObject level;
    void Start()
    {
        if(level != null)
        {
            Destroy(level);
        }

        if(UseProfile.CurrentLevel >= levels.Count)
        {
            level = Instantiate(levels[UnityEngine.Random.Range(0, levels.Count)], new Vector3 (0, -115f, 0), Quaternion.identity);
        }
        else
        {
            level = Instantiate(levels[UseProfile.CurrentLevel], new Vector3(0, -115f, 0), Quaternion.identity);
        }

        GamePlayController.Instance.gameScene.InitState();
        GamePlayController.Instance.playerContain.inputController.GetCurrentLevel();

        this.RegisterListener(EventID.LEVELCHANGE, UpdateLevel);
    }


    //Event Listener Section
    void UpdateLevel(object dam)
    {
        if (level != null)
        {
            Destroy(level);
        }

        if (UseProfile.CurrentLevel >= levels.Count)
        {
            level = Instantiate(levels[UnityEngine.Random.Range(0, levels.Count)], new Vector3(0, -115f, 0), Quaternion.identity);
        }
        else
        {
            level = Instantiate(levels[UseProfile.CurrentLevel], new Vector3(0, -115f, 0), Quaternion.identity);
        }

        GamePlayController.Instance.gameScene.InitState();
        GamePlayController.Instance.playerContain.inputController.GetCurrentLevel();
    }
}
