using EventDispatcher;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneController : SceneBase
{
    public List<Button> levelSelectBtns = new List<Button>();
    public Sprite levelAvailable;
    public Sprite levelNotAvailable;

    [Space]
    [Header("Dev Section")]
    public Button resetProgress;
    public Button unlockAllLevels;

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

        resetProgress.onClick.AddListener(delegate { ResetProgress(); });
        unlockAllLevels.onClick.AddListener(delegate { UnlockAllLevels(); });

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
                levelSelectBtns[i].GetComponent<Image>().sprite = levelAvailable;
            }
        }

        for (int i = UseProfile.CurrentLevel + 1; i < levelSelectBtns.Count; i++)
        {
            if (i < levelSelectBtns.Count)
            {
                levelSelectBtns[i].interactable = false;
                levelSelectBtns[i].GetComponent<Image>().sprite = levelNotAvailable;
            }
        }
    }

    private void PlayLevel(int id)
    {     
        UseProfile.ChosenLevel = id;
        GameController.Instance.musicManager.PlayClickSound();
        SceneManager.LoadScene("GamePlay");
    }

    //Dev Section
    void ResetProgress()
    {
        GameController.Instance.musicManager.PlayClickSound();
        UseProfile.CurrentLevel = 0;
    }

    void UnlockAllLevels()
    {
        GameController.Instance.musicManager.PlayClickSound();
        UseProfile.CurrentLevel = levelSelectBtns.Count - 1;
    }

    //Event Listener Section
    void UpdateUnlockedLevel(object dam)
    {
        InitState();
    }
}
