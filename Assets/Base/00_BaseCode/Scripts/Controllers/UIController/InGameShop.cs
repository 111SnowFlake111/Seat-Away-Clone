using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameShop : BaseBox
{
    private static InGameShop instance;

    public static InGameShop Setup(bool isSaveBox = false, Action actionOpenBoxSave = null)
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<InGameShop>(PathPrefabs.IN_GAME_SHOP));
            instance.Init();
        }

        instance.InitState();
        return instance;
    }

    public Text moneyValue;

    public Text timeFreezeUses;
    public Text instantMoveUses;
    public Text increaseSizeUses;

    public Text timeFreezeCost;
    public Text instantMoveCost;
    public Text increaseSizeCost;

    public Button timeFreezeBuyBtn;
    public Button instantMoveBuyBtn;
    public Button increaseSizeBuyBtn;

    public Button closeBtn;

    public AudioClip buy;

    public void Init()
    {
        timeFreezeBuyBtn.onClick.AddListener(delegate { BuyBooster("TimeFreeze", int.Parse(timeFreezeCost.text)); });
        instantMoveBuyBtn.onClick.AddListener(delegate { BuyBooster("InstantMove", int.Parse(instantMoveCost.text)); });
        increaseSizeBuyBtn.onClick.AddListener(delegate { BuyBooster("IncreaseSize", int.Parse(increaseSizeCost.text)); });

        closeBtn.onClick.AddListener(delegate { CloseShop(); });
    }

    public void InitState()
    {
        moneyValue.text = UseProfile.Coin.ToString();

        //Booster Uses
        timeFreezeUses.text = "Owned: " + UseProfile.TimeFreezeUses.ToString();
        instantMoveUses.text = "Owned: " + UseProfile.InstantMoveUses.ToString();
        increaseSizeUses.text = "Owned: " + UseProfile.IncreaseSizeUses.ToString();

        if(UseProfile.TimeFreezeUses <= 0)
        {
            timeFreezeUses.color = Color.red;
        }
        else
        {
            timeFreezeUses.color = Color.green;
        }

        if (UseProfile.InstantMoveUses <= 0)
        {
            instantMoveUses.color = Color.red;
        }
        else
        {
            instantMoveUses.color = Color.green;
        }

        if (UseProfile.IncreaseSizeUses <= 0)
        {
            increaseSizeUses.color = Color.red;
        }
        else
        {
            increaseSizeUses.color = Color.green;
        }

        //Booster Price checker
        if (int.Parse(timeFreezeCost.text) > UseProfile.Coin)
        {
            timeFreezeBuyBtn.interactable = false;
        }
        else
        {
            timeFreezeBuyBtn.interactable = true;
        }

        if (int.Parse(instantMoveCost.text) > UseProfile.Coin)
        {
            instantMoveBuyBtn.interactable = false;
        }
        else
        {
            instantMoveBuyBtn.interactable = true;
        }

        if (int.Parse(increaseSizeCost.text) > UseProfile.Coin)
        {
            increaseSizeBuyBtn.interactable = false;
        }
        else
        {
            increaseSizeBuyBtn.interactable = true;
        }
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

    void CloseShop()
    {
        GameController.Instance.musicManager.PlayClickSound();
        Time.timeScale = 1;
        GamePlayController.Instance.gameScene.InitState();
        Close();
    }
}
