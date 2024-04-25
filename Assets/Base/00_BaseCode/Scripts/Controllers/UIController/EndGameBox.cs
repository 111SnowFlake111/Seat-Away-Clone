using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameBox : BaseBox
{
    private static EndGameBox instance;
    public static EndGameBox Setup(bool isSaveBox = false, Action actionOpenBoxSave = null)
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<EndGameBox>(PathPrefabs.END_GAME_BOX));
            instance.Init();
        }

        //instance.InitState();
        return instance;
    }

    public Text title;
    public Text moneyReward;
    [NonSerialized] public int moneyRewardValue;

    public Button claimRewardBtn;
    public Button claimDoubleRewardBtn;
    public Button backToMenuBtn;

    public void Init()
    {
        claimRewardBtn.onClick.AddListener(delegate { ClaimReward(); });
        claimDoubleRewardBtn.onClick.AddListener(delegate { ClaimDoubleReward(); });
        backToMenuBtn.onClick.AddListener(delegate { ReturnToMenu(); });

        InitState();
    }

    public void InitState()
    {
        if (GamePlayController.Instance.playerContain.lose)
        {
            title.text = "Level Failed";
            moneyReward.text = "+ 0";

            claimRewardBtn.gameObject.SetActive(false);
            claimDoubleRewardBtn.gameObject.SetActive(false);
            backToMenuBtn.gameObject.SetActive(true);
        }
        else if (GamePlayController.Instance.playerContain.win)
        {
            title.text = "Level Completed";

            moneyRewardValue = GamePlayController.Instance.gameScene.gameLevelController.level.GetComponent<LevelControllerNew>().moneyReward;
            moneyReward.text = "+ " + moneyRewardValue.ToString();

            claimRewardBtn.gameObject.SetActive(true);
            claimDoubleRewardBtn.gameObject.SetActive(true);
            backToMenuBtn.gameObject.SetActive(false);
        }
    }

    void ClaimReward()
    {
        UseProfile.Coin += moneyRewardValue;
        SceneManager.LoadScene("HomeScene");
    }

    void ClaimDoubleReward()
    {
        UseProfile.Coin += moneyRewardValue * 2;
        SceneManager.LoadScene("HomeScene");
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene("HomeScene");
    }
}
