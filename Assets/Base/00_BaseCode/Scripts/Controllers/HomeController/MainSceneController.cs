using EventDispatcher;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneController : SceneBase
{
    public List<Button> levelSelectBtns = new List<Button>();

    //public Button btnHome;
    //public RandomWatchVideo btnReward;
    public override void Init()
    {
        //btnHome.onClick.AddListener(delegate { OnClickPlay(); });
        //btnReward.Init();
        for(int i = 0; i < levelSelectBtns.Count; i++)
        {
            int number = i;
            levelSelectBtns[number].onClick.AddListener(delegate { PlayLevel(number); });
        }

        InitState();
        
        this.RegisterListener(EventID.LEVELCHANGE, UpdateUnlockedLevel);
    }

    public void InitState()
    {
        for (int i = 0; i <= UseProfile.CurrentLevel;i++)
        {
            if (i < levelSelectBtns.Count)
            {
                levelSelectBtns[i].interactable = true;
            }
        }

        for (int i = UseProfile.CurrentLevel + 1; i < levelSelectBtns.Count; i++)
        {
            if (i < levelSelectBtns.Count)
            {
                levelSelectBtns[i].interactable = false;
            }
        }
    }

    private void PlayLevel(int id)
    {     
        UseProfile.ChosenLevel = id;
        SceneManager.LoadScene("GamePlay");
    }

    //Event Listener Section
    void UpdateUnlockedLevel(object dam)
    {
        InitState();
    }
}
