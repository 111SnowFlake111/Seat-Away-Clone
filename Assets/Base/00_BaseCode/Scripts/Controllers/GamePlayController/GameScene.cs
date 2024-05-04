using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : BaseScene
{
    #region PublicVar
    public Camera cam;

    public Text timer;

    public Button btnSetting;

    public GameObject instantMoveTutorial;
    public Image timerFreezeEffect;

    public Button btnFreezeTimer;
    public Button btnInstantMove;
    public Button btnAddColumn;

    public Text freezeTimerUses;
    public Text instantMoveUses;
    public Text increaseSizeUses;

    public GameLevelController gameLevelController;

    public AudioClip timeFreeze;
    public AudioClip instantMove;
    public AudioClip sizeIncrease;

    [NonSerialized] public bool firstChairMoved = false;
    #endregion

    #region PrivateVar
    bool instantMoveActivated = false;
    RaycastHit hit;
    Coroutine temp;
    Chair chosenChair = null;
    PathFindingAStar passenger = null;
    #endregion

    public void Init()
    {
        timerFreezeEffect.fillAmount = 0;
        instantMoveTutorial.SetActive(false);

        btnFreezeTimer.onClick.AddListener(delegate { FreezeTimer(); });
        btnInstantMove.onClick.AddListener(delegate { InstantMove(); });
        btnAddColumn.onClick.AddListener(delegate { AddColumn(); });

        btnSetting.onClick.AddListener(delegate { OpenSetting(); });
    }

    public void InitState()
    {
        int minute = gameLevelController.level.GetComponent<LevelControllerNew>().timeLimit / 60;
        int second = gameLevelController.level.GetComponent<LevelControllerNew>().timeLimit % 60;

        timer.text = string.Format("{0: 00}: {1: 00}", minute, second);

        //Booster Uses
        if (UseProfile.TimeFreezeUses <= 0)
        {
            freezeTimerUses.color = Color.red;
        }
        else
        {
            freezeTimerUses.color = Color.green;
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
        freezeTimerUses.text = UseProfile.TimeFreezeUses.ToString();
        instantMoveUses.text = UseProfile.InstantMoveUses.ToString();
        increaseSizeUses.text = UseProfile.IncreaseSizeUses.ToString();

        //Time Freeze Booster only clickable after the game has begun
        if (GamePlayController.Instance.playerContain.gameStart)
        {
            btnFreezeTimer.interactable = true;
        }
        else
        {
            btnFreezeTimer.interactable = false;
        }

        //IncreaseSize Booster Usage Limiter
        if (gameLevelController.level.GetComponent<LevelControllerNew>().addColumnUsageLimit > 0)
        {
            btnAddColumn.interactable = true;
        }
        else
        {
            btnAddColumn.interactable = false;
        }
    }

    IEnumerator TimerCountDown()
    {
        while(gameLevelController.level.GetComponent<LevelControllerNew>().timeLimit > 0)
        {
            if(GamePlayController.Instance.playerContain.win || GamePlayController.Instance.playerContain.lose)
            {
                break;
            }

            yield return new WaitForSeconds(1);
            gameLevelController.level.GetComponent<LevelControllerNew>().timeLimit -= 1;
            InitState();
        }

        if (gameLevelController.level.GetComponent<LevelControllerNew>().timeLimit <= 0)
        {
            gameLevelController.level.GetComponent<LevelControllerNew>().CheckWinCondition();
        }
    }

    private void Update()
    {
        if (instantMoveActivated)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor")))
                {
                    if(hit.collider.GetComponent<MapTile>().chair != null)
                    {
                        instantMoveTutorial.SetActive(false);
                        chosenChair = hit.collider.GetComponent<MapTile>().chair.GetComponent<Chair>();
                    }
                    else
                    {
                        instantMoveTutorial.SetActive(false);
                        btnInstantMove.interactable = true;
                        Debug.LogError("Not a valid chair");
                        instantMoveActivated = false;
                        GamePlayController.Instance.playerContain.inputController.gameObject.SetActive(true);
                        //StartCoroutine(WaitForSecondsInstantMove(3));
                    }
                }
                else
                {
                    instantMoveTutorial.SetActive(false);
                    btnInstantMove.interactable = true;
                    Debug.LogError("Not a valid chair");
                    instantMoveActivated = false;
                    GamePlayController.Instance.playerContain.inputController.gameObject.SetActive(true);
                    //StartCoroutine(WaitForSecondsInstantMove(3));
                }
            }
        }

        if (chosenChair != null)
        {
            if (!chosenChair.occupied)
            {
                if (passenger.GetComponent<Passenger>().colorID == 1)
                {
                    if (chosenChair.colorID == 0 || chosenChair.colorID == 1)
                    {
                        passenger.goal = chosenChair;
                        passenger.goalTileHit = hit;
                        StartCoroutine(passenger.MoveToChair(0));

                        instantMoveTutorial.SetActive(false);
                        StartCoroutine(WaitForSecondsInstantMove(3));

                        chosenChair = null;
                        passenger = null;

                        UseProfile.InstantMoveUses--;
                        InitState();
                    }
                    else
                    {
                        Debug.LogError("Chair color doesn't match passenger's color");
                        instantMoveTutorial.SetActive(false);
                        btnInstantMove.interactable = true;
                        //StartCoroutine(WaitForSecondsInstantMove(3));

                        chosenChair = null;
                        passenger = null;
                    }
                }
                else if (chosenChair.colorID == passenger.GetComponent<Passenger>().colorID)
                {
                    passenger.goal = chosenChair;
                    passenger.goalTileHit = hit;
                    StartCoroutine(passenger.MoveToChair(0));

                    instantMoveTutorial.SetActive(false);
                    StartCoroutine(WaitForSecondsInstantMove(3));

                    chosenChair = null;
                    passenger = null;

                    UseProfile.InstantMoveUses--;
                    InitState();
                }
                else
                {
                    Debug.LogError("Chair color doesn't match passenger's color");
                    instantMoveTutorial.SetActive(false);
                    btnInstantMove.interactable = true;
                    //StartCoroutine(WaitForSecondsInstantMove(3));                   

                    chosenChair = null;
                    passenger = null;
                }
            }
            else
            {
                Debug.LogError("This chair is occupied");
                instantMoveTutorial.SetActive(false);
                btnInstantMove.interactable = true;
                //StartCoroutine(WaitForSecondsInstantMove(3));

                chosenChair = null;
                passenger = null;
            }

            instantMoveActivated = false;
            GamePlayController.Instance.playerContain.inputController.gameObject.SetActive(true);
        }
    }

    public void StartTimer()
    {
        temp = StartCoroutine(TimerCountDown());
    }

    public void StopTimer()
    {
        StopCoroutine(temp);
    }

    public void OpenSetting()
    {
        GameController.Instance.musicManager.PlayClickSound();
        Time.timeScale = 0;
        SettingBox.Setup().Show();
    }

    #region BoostersFunctions
    void FreezeTimer()
    {
        if (UseProfile.TimeFreezeUses <= 0)
        {
            GameController.Instance.musicManager.PlayClickSound();
            Time.timeScale = 0;
            InGameShop.Setup().Show();
        }
        else
        {
            GameController.Instance.musicManager.PlayOneShot(timeFreeze);

            UseProfile.TimeFreezeUses--;
            InitState();

            timerFreezeEffect.fillAmount = 1;
            timerFreezeEffect.DOFillAmount(0, 7f).SetEase(Ease.Linear)
                .OnStart(delegate
                {
                    btnFreezeTimer.interactable = false;
                    StopTimer();

                })
                .OnComplete(delegate
                {
                    btnFreezeTimer.interactable = true;
                    StartTimer();
                });
        }
    }

    void InstantMove()
    {
        if (UseProfile.InstantMoveUses <= 0)
        {
            GameController.Instance.musicManager.PlayClickSound();
            Time.timeScale = 0;
            InGameShop.Setup().Show();
        }
        else
        {
            GameController.Instance.musicManager.PlayOneShot(instantMove);

            if (gameLevelController.level.GetComponent<LevelControllerNew>().passengerMoving)
            {
                GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp
                    (
                        btnInstantMove.transform.position + new Vector3(0, 30, 0),
                        "Please wait till all passengers have stopped moving",
                        Color.red
                    );
                return;
            }

            btnInstantMove.interactable = false;

            for (int i = 0; i < gameLevelController.level.GetComponent<LevelControllerNew>().passengers.Count; i++)
            {
                if (gameLevelController.level.GetComponent<LevelControllerNew>().passengers[i] != null)
                {
                    if (gameLevelController.level.GetComponent<LevelControllerNew>().passengers[i].GetComponent<PathFindingAStar>().isAtEntrance && !gameLevelController.level.GetComponent<LevelControllerNew>().passengers[i].GetComponent<PathFindingAStar>().isSeated)
                    {
                        passenger = gameLevelController.level.GetComponent<LevelControllerNew>().passengers[i].GetComponent<PathFindingAStar>();
                        break;
                    }
                }
            }

            if (passenger == null)
            {
                Debug.LogError("No valid Passenger at the entrance");
                StartCoroutine(WaitForSecondsInstantMove(3));

                return;
            }

            instantMoveTutorial.SetActive(true);
            instantMoveActivated = true;

            GamePlayController.Instance.playerContain.inputController.gameObject.SetActive(false);
        }
    }

    void AddColumn()
    {
        if (UseProfile.IncreaseSizeUses <= 0)
        {
            GameController.Instance.musicManager.PlayClickSound();
            Time.timeScale = 0;
            InGameShop.Setup().Show();
        }
        else
        {
            GameController.Instance.musicManager.PlayOneShot(sizeIncrease);
            UseProfile.IncreaseSizeUses--;
            InitState();
            gameLevelController.level.GetComponent<LevelControllerNew>().AddOneColumnToTheLeft();
        }       
    }
    #endregion


    IEnumerator WaitForSecondsInstantMove(float second)
    {
        yield return new WaitForSeconds(second);
        btnInstantMove.interactable = true;
        //instantMoveTutorial.SetActive(false);
    }

    public override void OnEscapeWhenStackBoxEmpty()
    {

    }
}
