using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : BaseScene
{
    public Camera cam;

    public TMP_Text timer;

    public Button btnSetting;

    public GameObject instantMoveTutorial;
    public Image timerFreezeEffect;

    public Button btnFreezeTimer;
    public Button btnInstantMove;
    public Button btnAddColumn;

    public GameLevelController gameLevelController;

    [NonSerialized] public bool firstChairMoved = false;

    bool instantMoveActivated = false;
    RaycastHit hit;
    Coroutine temp;
    Chair chosenChair = null;
    PathFindingAStar passenger = null;
    public void Init()
    {
        timerFreezeEffect.fillAmount = 0;
        instantMoveTutorial.SetActive(false);

        btnFreezeTimer.onClick.AddListener(delegate { FreezeTimer(); });
        btnInstantMove.onClick.AddListener(delegate { InstantMove(); });
        btnAddColumn.onClick.AddListener(delegate { AddColumn(); });
    }

    public void InitState()
    {
        int minute = gameLevelController.level.GetComponent<LevelControllerNew>().timeLimit / 60;
        int second = gameLevelController.level.GetComponent<LevelControllerNew>().timeLimit % 60;

        timer.text = string.Format("{0: 00}: {1: 00}", minute, second);

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
                        //StartCoroutine(WaitForSecondsInstantMove(3));
                    }
                }
                else
                {
                    instantMoveTutorial.SetActive(false);
                    btnInstantMove.interactable = true;
                    Debug.LogError("Not a valid chair");
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

    #region ButtonFunctions
    void FreezeTimer()
    {
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

    void InstantMove()
    {
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

    void AddColumn()
    {        
        gameLevelController.level.GetComponent<LevelControllerNew>().AddOneColumnToTheLeft();
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
