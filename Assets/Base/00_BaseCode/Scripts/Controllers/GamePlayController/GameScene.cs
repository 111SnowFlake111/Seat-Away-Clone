using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : BaseScene
{
    public Text timer;

    public Button btnSetting;

    public GameObject instantMoveTutorial;
    public Image timerFreezeEffect;

    public Button btnFreezeTimer;
    public Button btnInstantMove;
    public Button btnAddColumn;

    public GameLevelController gameLevelController;

    [NonSerialized] public bool firstChairMoved = false;
    public void Init()
    {
        timerFreezeEffect.fillAmount = 0;
        instantMoveTutorial.SetActive(false);

        btnFreezeTimer.onClick.AddListener(delegate { FreezeTimer(); });
        btnInstantMove.onClick.AddListener(delegate { InstantMove(); });
        btnAddColumn.onClick.AddListener(delegate { AddColumn(); });
    }

    private void InitState()
    {
        int minute = gameLevelController.level.GetComponent<LevelControllerNew>().timeLimit / 60;
        int second = gameLevelController.level.GetComponent<LevelControllerNew>().timeLimit % 60;

        timer.text = string.Format("{0: 00}: {1: 00}", minute, second);
    }

    IEnumerator TimerCountDown()
    {
        while(gameLevelController.level.GetComponent<LevelControllerNew>().timeLimit > 0)
        {
            yield return new WaitForSeconds(1);
            gameLevelController.level.GetComponent<LevelControllerNew>().timeLimit -= 1;
            InitState();
        }

        if (gameLevelController.level.GetComponent<LevelControllerNew>().timeLimit <= 0)
        {
            
        }
    }

    public void StartTimer()
    {

    }

    public void StopTimer()
    {
        
    }

    #region ButtonFunctions
    void FreezeTimer()
    {
        timerFreezeEffect.fillAmount = 1;
        timerFreezeEffect.DOFillAmount(0, 7f)
            .OnStart(delegate
            {
                btnFreezeTimer.interactable = false;

            })
            .OnComplete(delegate
            {
                btnFreezeTimer.interactable = true;
            });
    }

    void InstantMove()
    {
        btnInstantMove.interactable = false;

        Chair chosenChair = null;
        PathFindingAStar passenger = null;

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
            StartCoroutine(WaitForThreeSeconds());
            btnInstantMove.interactable = true;
            instantMoveTutorial.SetActive(false);
            return;
        }

        RaycastHit hit;
        instantMoveTutorial.SetActive(true);
        while(chosenChair == null)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(mousePos, Vector3.down, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Chair")))
                {
                    chosenChair = hit.collider.GetComponent<Chair>();
                    break;
                }
                else
                {
                    Debug.LogError("Not a valid chair");
                    StartCoroutine(WaitForThreeSeconds());
                    btnInstantMove.interactable = true;
                    instantMoveTutorial.SetActive(false);
                    return;
                }
            }
        }

        if (!chosenChair.occupied)
        {
            if (passenger.GetComponent<Passenger>().colorID == 1)
            {
                if (chosenChair.colorID == 0 || chosenChair.colorID == 1)
                {
                    passenger.goal = chosenChair;
                    passenger.MoveToChair(0);
                }
                else
                {
                    Debug.LogError("Chair color doesn't match passenger's color");
                    StartCoroutine(WaitForThreeSeconds());
                    btnInstantMove.interactable = true;
                    instantMoveTutorial.SetActive(false);
                    return;
                }
            }
            else if (chosenChair.colorID == passenger.GetComponent<Passenger>().colorID)
            {
                passenger.goal = chosenChair;
                passenger.MoveToChair(0);
            }
            else
            {
                Debug.LogError("Chair color doesn't match passenger's color");
                StartCoroutine(WaitForThreeSeconds());
                btnInstantMove.interactable = true;
                instantMoveTutorial.SetActive(false);
                return;
            }
        }
        else
        {
            Debug.LogError("This chair is occupied");
            StartCoroutine(WaitForThreeSeconds());
            btnInstantMove.interactable = true;
            instantMoveTutorial.SetActive(false);
            return;
        }
    }

    void AddColumn()
    {
        gameLevelController.level.GetComponent<LevelControllerNew>().AddOneColumnToTheLeft();
    }
    #endregion

    IEnumerator WaitForThreeSeconds()
    {
        yield return new WaitForSeconds(3);
    }

    public override void OnEscapeWhenStackBoxEmpty()
    {

    }
}
