using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSceneController : SceneBase
{
    public Text timeFreezeUses;
    public Text instantMoveUses;
    public Text sizeIncreaseUses;

    public Text timeFreezeBuyCost;
    public Text instantMoveBuyCost;
    public Text sizeIncreaseBuyCost;

    public Button timeFreezeBuyBtn;
    public Button instantMoveBuyBtn;
    public Button sizeIncreaseBuyBtn;

    public Button IAPMoney500Btn;
    public Button IAPMoney2000Btn;
    public Button IAPMoney4000Btn;
    public Button IAPPackBtn;

    public AudioClip buy;

    [Space]
    [Header("Dev Section")]
    public Button resetBoosters;
    public Button resetMoney;
    public Button add10000Money;

    public override void Init()
    {
        IAPMoney500Btn.onClick.AddListener(delegate { GetMoney(500); });
        IAPMoney2000Btn.onClick.AddListener(delegate { GetMoney(2000); });
        IAPMoney4000Btn.onClick.AddListener(delegate { GetMoney(4000); });
        IAPPackBtn.onClick.AddListener(delegate { GetPack(5, 5, 5, 5000); });

        timeFreezeBuyBtn.onClick.AddListener(delegate { BuyBooster("TimeFreeze", int.Parse(timeFreezeBuyCost.text)); });
        instantMoveBuyBtn.onClick.AddListener(delegate { BuyBooster("InstantMove", int.Parse(instantMoveBuyCost.text)); });
        sizeIncreaseBuyBtn.onClick.AddListener(delegate { BuyBooster("IncreaseSize", int.Parse(sizeIncreaseBuyCost.text)); });

        resetMoney.onClick.AddListener(delegate { ResetMoney(); });
        add10000Money.onClick.AddListener(delegate { Add10000Money(); });
        resetBoosters.onClick.AddListener(delegate { ResetBoosters(); });

        InitState();
    }

    public void InitState()
    {
        timeFreezeUses.text = UseProfile.TimeFreezeUses.ToString();
        instantMoveUses.text = UseProfile.InstantMoveUses.ToString();
        sizeIncreaseUses.text = UseProfile.IncreaseSizeUses.ToString();

        if(int.Parse(timeFreezeBuyCost.text) > UseProfile.Coin)
        {
            timeFreezeBuyBtn.interactable = false;
        }
        else
        {
            timeFreezeBuyBtn.interactable = true;
        }

        if (int.Parse(instantMoveBuyCost.text) > UseProfile.Coin)
        {
            instantMoveBuyBtn.interactable = false;
        }
        else
        {
            instantMoveBuyBtn.interactable = true;
        }

        if (int.Parse(sizeIncreaseBuyCost.text) > UseProfile.Coin)
        {
            sizeIncreaseBuyBtn.interactable = false;
        }
        else
        {
            sizeIncreaseBuyBtn.interactable = true;
        }
    }

    void GetMoney(int value)
    {
        UseProfile.Coin += value;
        GameController.Instance.musicManager.PlayClickSound();
        InitState();
    }

    void GetPack(int? timeFreezeUse, int? instantMoveUse, int? increaseSizeUse, int? coin)
    {
        GameController.Instance.musicManager.PlayClickSound();

        if (timeFreezeUse.HasValue)
        {
            UseProfile.TimeFreezeUses += timeFreezeUse.Value;
        }

        if (instantMoveUse.HasValue)
        {
            UseProfile.InstantMoveUses += instantMoveUse.Value;
        }

        if (increaseSizeUse.HasValue)
        {
            UseProfile.IncreaseSizeUses += increaseSizeUse.Value;
        }
        
        if (coin.HasValue)
        {
            UseProfile.Coin += coin.Value;
        }

        InitState();
    }

    void BuyBooster(string boosterName, int cost)
    {
        if (cost > UseProfile.Coin)
        {
            Debug.LogError("Not enough money");
            return;
        }
        else
        {
            switch (boosterName)
            {
                case "TimeFreeze":
                    UseProfile.Coin -= cost;
                    UseProfile.TimeFreezeUses += 1;
                    break;
                case "InstantMove":
                    UseProfile.Coin -= cost;
                    UseProfile.InstantMoveUses += 1;
                    break;
                case "IncreaseSize":
                    UseProfile.Coin -= cost;
                    UseProfile.IncreaseSizeUses += 1;
                    break;
            }

            GameController.Instance.musicManager.PlayOneShot(buy);
            InitState();
        }
    }

    //Dev Section
    void ResetMoney()
    {
        GameController.Instance.musicManager.PlayClickSound();
        UseProfile.Coin = 0;
        InitState();
    }

    void Add10000Money()
    {
        GameController.Instance.musicManager.PlayClickSound();
        UseProfile.Coin += 10000;
        InitState();
    }

    void ResetBoosters()
    {
        GameController.Instance.musicManager.PlayClickSound();
        UseProfile.TimeFreezeUses = 0;
        UseProfile.InstantMoveUses = 0;
        UseProfile.IncreaseSizeUses = 0;
        InitState();
    }
}
