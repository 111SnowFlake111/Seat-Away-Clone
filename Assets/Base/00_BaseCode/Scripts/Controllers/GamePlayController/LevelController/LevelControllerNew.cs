using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using DG.Tweening;
using UnityEngine.UI;
using Sirenix.Utilities;
using System.Linq;

[System.Serializable]
public class LevelControllerNew : SerializedMonoBehaviour
{
    [Header("----------LEVEL ID (IMPORTANT)-----------")]
    [Tooltip("Change with caution as this will affect level unlock and level progress")]
    public int levelId;

    [Space]
    [Header("-----------CAMERA CONTROL-----------")]
    [Tooltip("This is for camera zoom value, which will be loaded upon entering a level")]
    public float mainCameraZoomValue;

    [Space]
    [Header("----------MAP CONFIG-----------")]
    public int moneyReward;
    public int mapRow;
    public int mapColumn;
    public GameObject[,] map;

    public List<Material> avaiableColors;
    public List<Chair> avaiableChairs;
    public List<PathFindingAStar> passengers;
    public List<WaitTile> sideway;

    [Space]
    [Header("-----------TIME LIMIT(seconds)-----------")]
    [Tooltip("Time limit of a level, the value here indicate seconds")]
    public int timeLimit;
    //[NonSerialized] public bool passengerMoving;

    [Space]
    [Header("-----------MISC-----------")]
    public GameObject carModel;
    public GameObject carDoor;
    public MapTile mapTile;
    public GameObject wallFront;
    public GameObject wallBack;
    public GameObject wallLeft;
    public GameObject wallRight;
    public Material floorColor_1;
    public Material floorColor_2;

    [Space]
    [Header("-----------AUDIO-----------")]
    public AudioClip victory;
    public AudioClip defeat;

    [Space]
    [Header("-----------EFFECT-----------")]
    public ParticleSystem smoke;

    [NonSerialized] public int addColumnUsageLimit = 1;
    public List<PathFindingAStar> movingPassngers = new List<PathFindingAStar>();

    private void Start()
    {
        GamePlayController.Instance.gameScene.cam.orthographicSize = mainCameraZoomValue;
        GamePlayController.Instance.gameScene.background.transform.localPosition = new Vector3(-215f - 20f * CalculateMultipler(), 0, 2500f);

        int CalculateMultipler()
        {
            if (mapColumn > 3)
            {
                return mapColumn - 3;
            }
            else
            {
                return 0;
            }
        }
    }

    public void AddOneColumnToTheLeft()
    {
        Vector3[] pathForAnim;
        mapColumn++;

        GameObject[,] newMap = new GameObject[map.GetLength(0) + 1, map.GetLength(1)];

        for(int i = 0; i < map.GetLength(1); i++)
        {
            for(int j = 0; j < map.GetLength(0); j++)
            {
                if (map[j, i] != null)
                {
                    newMap[j + 1, i] = map[j, i];
                }
                
            }
        }


        //Vector3 positionOverwrite = new Vector3(2 - mapColumn - 1, 0, 2);

        //Duyệt chiều dọc
        for (int i = 0; i < newMap.GetLength(1); i++)
        {
            var tileInstance = Instantiate(mapTile, transform);
            if (map[0, i].GetComponent<MapTile>().colorID == 2)
            {
                tileInstance.GetComponent<MeshRenderer>().material = floorColor_1;
            }
            else
            {
                tileInstance.GetComponent<MeshRenderer>().material = floorColor_2;
            }

            pathForAnim = new Vector3[] { map[0, i].transform.localPosition, map[0, i].transform.localPosition - new Vector3(1, 0, 0) };
            tileInstance.GetComponent<MeshRenderer>().enabled = false;
            tileInstance.transform.localPosition = map[0, i].transform.localPosition - new Vector3(1, 0, 0);
            StartCoroutine(MoveTileAnim(tileInstance, pathForAnim));
            newMap[0, i] = tileInstance.gameObject;
            
            new WaitForSeconds(0.5f);
            //positionOverwrite -= new Vector3(0, 0, 1);
        }

        map = newMap;

        AdjustSize();
        AdjustWalls();

        StartCoroutine(DelayedUpdateNeighbour());
        addColumnUsageLimit--;

        carModel.transform.DOScale(new Vector3(carModel.transform.localScale.x + 0.5f, carModel.transform.localScale.y, carModel.transform.localScale.z), 0.5f)
            .OnStart(delegate
            {
                carModel.transform.DOLocalMoveX(carModel.transform.localPosition.x - 0.5f, 0.5f);
            });
        GamePlayController.Instance.gameScene.background.transform.DOLocalMoveX(GamePlayController.Instance.gameScene.background.transform.localPosition.x - 20f, 0.5f);
        GamePlayController.Instance.gameScene.InitState();
    }

    IEnumerator DelayedUpdateNeighbour()
    {
        yield return new WaitForSecondsRealtime(2);
        MapTileNeighbourUpdate();
    }
    IEnumerator MoveTileAnim(MapTile tile, Vector3[] destination)
    {
        tile.transform.DOLocalPath(destination, 0.5f)
            .OnWaypointChange(index =>
        {
            if(index == destination.Length - 1)
            {
                tile.GetComponent<MeshRenderer>().enabled = true;
            }
        });
        yield return null;
    }

    //Mainly to allow Player to be able to see as the play field grow bigger
    public void AdjustSize()
    {
        //Vector3 sizeEachColumn = new Vector3(2.4f, 2.4f, 2.4f);
        //transform.localScale = sizeEachColumn / mapColumn;

        //Note to self: Each column increased will require car model to increase x scale by ... and move local x position to the left by ...
        //Note to self: Each row increased will require car model to increase z scale by 0.25f and move local z position downward by 0.3f, 6 rows = 0.9f

        //if (mapColumn > 4 || mapRow > 8)
        //{
        //    GamePlayController.Instance.gameScene.cam.DOOrthoSize(5 + 0.5f * GetBiggestValueOfColumnAndRow(), 0.5f);
        //}

        //int GetBiggestValueOfColumnAndRow()
        //{
        //    if ((mapColumn - 4) > (mapRow - 8))
        //    {
        //        return mapColumn - 4;
        //    }
        //    else
        //    {
        //        return mapRow - 8;
        //    }
        //}

        GamePlayController.Instance.gameScene.cam.DOOrthoSize(mainCameraZoomValue + 0.5f, 0.5f);
    }

    public void CheckWinCondition()
    {
        if (!movingPassngers.Any())
        {
            if(passengers.Any())
            {
                if (timeLimit > 0)
                {
                    return;
                }
                else
                {
                    GamePlayController.Instance.playerContain.lose = true;
                    GamePlayController.Instance.playerContain.win = false;

                    GameController.Instance.musicManager.PlayOneShot(defeat);
                    EndGameBox.Setup().Show();
                    return;
                    //Activate lose
                }
            }
            else
            {
                GamePlayController.Instance.playerContain.lose = false;
                GamePlayController.Instance.playerContain.win = true;

                if (levelId >= UseProfile.CurrentLevel)
                {
                    UseProfile.CurrentLevel++;
                }

                carDoor.transform.DOLocalRotate(Vector3.zero, 0.75f)
                    .OnComplete(delegate
                    {

                        transform.DOLocalMoveZ(20f, 4f)
                        .OnStart(delegate
                        {
                            smoke.Play();
                            StartCoroutine(DelayedWin());
                        });
                    });

                //Activate win
            }
        }

        
    }

    IEnumerator DelayedWin()
    {
        yield return new WaitForSeconds(0.75f);
        GameController.Instance.musicManager.PlayOneShot(victory);
        EndGameBox.Setup().Show();
    }

    public void SetMap()
    {
        //Note To Self: Each row increase will take up to 1f z
        //Note To Self: Each column take up to 1f x
        //Note To Self: Every time a row is added (usually to the bottom row), left n right wall must be moved downward by 0.5f z and collider box must be increased by 0.5f in z size
        //Note To Self: Every time a column is added (usually to the left side), front n back wall must be moved to the left by 0.5 x and collider box must be increased by 1f in x size
        //These value is to move the wall

        //Note To Self: For current car model, each use of Increase Car Size boost, the model should increase by 0.5f for x scale and move to the left by 0.5f on x position (mean -0.5f)

        map = new GameObject[mapColumn + 1, mapRow];

        AdjustWalls();
    }

    void AdjustWalls()
    {
        wallRight.transform.localPosition = new Vector3(1.7f, -0.5f, 3.5f - 0.5f * mapRow);
        wallLeft.transform.localPosition = new Vector3(1.3f - mapColumn, -0.5f, 3.5f - 0.5f * mapRow);
        wallFront.transform.localPosition = new Vector3(1.5f - 0.5f * mapColumn, -0.5f, 2.7f);
        wallBack.transform.localPosition = new Vector3(1.5f - 0.5f * mapColumn, -0.5f, 2.5f - (float)mapRow);

        wallLeft.GetComponent<BoxCollider>().size = new Vector3(3, 0.2f, 0.5f * mapRow);
        wallRight.GetComponent<BoxCollider>().size = new Vector3(3, 0.2f, 0.5f * mapRow);
        wallFront.GetComponent<BoxCollider>().size = new Vector3(mapColumn, 0.2f, 2f);
        wallBack.GetComponent<BoxCollider>().size = new Vector3(mapColumn, 0.2f, 2f);
    }

    [Button]
    public void GetPassengers()
    {
        FindSideWays();
        FindPassengers();

        if (sideway.Count <= passengers.Count)
        {
            Debug.LogError("Not enough Sideway Tile for Passengers, please make some more");
        }
        else
        {
            int i = 1;

            foreach (PathFindingAStar passenger in passengers)
            {
                passenger.transform.localPosition = sideway[i].transform.localPosition + new Vector3(0, 0.5f, 0);
                i++;
            }

            Debug.LogError("All Passengers are ready");
        }
    }
    public void FindSideWays()
    {
        sideway.Clear();

        foreach (Transform child in transform)
        {
            if (child.GetComponent<WaitTile>() != null)
            {
                sideway.Add(child.GetComponent<WaitTile>());
            }
        }

        Vector3 sidewayPos = new Vector3(3, 0, 2);

        for (int i = 0; i < sideway.Count; i++)
        {
            if(i == 0)
            {
                sideway[i].transform.localPosition = new Vector3(2, 0, 2);
            }
            else if(i == 1)
            {
                sideway[i].transform.localPosition = sidewayPos;
            }
            else
            {
                sidewayPos += new Vector3(0, 0, -1);
                sideway[i].transform.localPosition = sidewayPos;
            }
        }
    }
    public void FindPassengers()
    {
        passengers.Clear();

        foreach (Transform child in transform)
        {
            if (child.GetComponent<PathFindingAStar>() != null)
            {
                passengers.Add(child.GetComponent<PathFindingAStar>());
            }
        }
    }

    [Button]
    public void UpdateMap()
    {
        int row = 0;
        int column = 0;

        SetMap();
        //Vector3 positionOverwrite = new Vector3(2 - mapColumn, 0, 2);
        foreach (Transform child in transform)
        {
            if (child.GetComponent<MapTile>() != null)
            {
                if (child.GetComponent<WaitTile>() != null)
                {
                    map[mapColumn, row] = child.gameObject;
                    //child.gameObject.transform.localPosition = new Vector3(2, 0, 2);
                }
                else
                {
                    map[column, row] = child.gameObject;
                    //child.gameObject.transform.localPosition = positionOverwrite;
                    if (row % 2 == 0)
                    {
                        if (column % 2 == 0)
                        {
                            map[column, row].GetComponent<MeshRenderer>().material = floorColor_2;
                            map[column, row].GetComponent<MapTile>().colorID = 2;
                        }
                        else
                        {
                            map[column, row].GetComponent<MeshRenderer>().material = floorColor_1;
                            map[column, row].GetComponent<MapTile>().colorID = 1;
                        }
                    }
                    else
                    {
                        if (column % 2 != 0)
                        {
                            map[column, row].GetComponent<MeshRenderer>().material = floorColor_2;
                            map[column, row].GetComponent<MapTile>().colorID = 2;
                        }
                        else
                        {
                            map[column, row].GetComponent<MeshRenderer>().material = floorColor_1;
                            map[column, row].GetComponent<MapTile>().colorID = 1;
                        }
                    }
                    

                    if (column + 1 > mapColumn - 1)
                    {
                        column = 0;
                        //positionOverwrite += new Vector3(-mapColumn + 1, 0, -1);
                        row++;
                    }
                    else
                    {
                        //positionOverwrite += new Vector3(1, 0, 0);
                        column++;
                    }

                    if (row >= mapRow)
                    {
                        break;
                    }
                }

            }
        }
    }

    [Button]
    public void UpdateMapAndReposition()
    {
        int row = 0;
        int column = 0;

        SetMap();
        Vector3 positionOverwrite = new Vector3(2 - mapColumn, 0, 2);
        foreach (Transform child in transform)
        {
            if (child.GetComponent<MapTile>() != null)
            {
                if (child.GetComponent<WaitTile>() != null)
                {
                    map[mapColumn, row] = child.gameObject;
                    child.gameObject.transform.localPosition = new Vector3(2, 0, 2);
                }
                else
                {
                    map[column, row] = child.gameObject;
                    child.gameObject.transform.localPosition = positionOverwrite;
                    if (row % 2 == 0)
                    {
                        if (column % 2 == 0)
                        {
                            map[column, row].GetComponent<MeshRenderer>().material = floorColor_2;
                            map[column, row].GetComponent<MapTile>().colorID = 2;
                        }
                        else
                        {
                            map[column, row].GetComponent<MeshRenderer>().material = floorColor_1;
                            map[column, row].GetComponent<MapTile>().colorID = 1;
                        }
                    }
                    else
                    {
                        if (column % 2 != 0)
                        {
                            map[column, row].GetComponent<MeshRenderer>().material = floorColor_2;
                            map[column, row].GetComponent<MapTile>().colorID = 2;
                        }
                        else
                        {
                            map[column, row].GetComponent<MeshRenderer>().material = floorColor_1;
                            map[column, row].GetComponent<MapTile>().colorID = 1;
                        }
                    }


                    if (column + 1 > mapColumn - 1)
                    {
                        column = 0;
                        positionOverwrite += new Vector3(-mapColumn + 1, 0, -1);
                        row++;
                    }
                    else
                    {
                        positionOverwrite += new Vector3(1, 0, 0);
                        column++;
                    }

                    if (row >= mapRow)
                    {
                        break;
                    }
                }

            }
        }

        carModel.transform.localPosition = new Vector3(0 - 0.5f * ColumnMultipler(), 0, 0.9f - 0.3f * RowMultipler());
        carModel.transform.localScale = new Vector3(2f + 0.5f * ColumnMultipler(), 2 , 1.75f + 0.25f * RowMultipler());

        int ColumnMultipler()
        {
            if (mapColumn > 3)
            {
                return mapColumn - 3;
            }
            else
            {
                return 0;
            }
        }

        int RowMultipler()
        {
            if (mapRow > 6)
            {
                return mapRow - 6;
            }
            else
            {
                return 0;
            }
        }

        smoke.gameObject.transform.localPosition = new Vector3(1.75f - 0.25f * mapColumn, -1, 2.5f - 1 * mapRow);
        MapTileNeighbourUpdate();
        //AdjustSize();
    }

    [Button]
    public void MapTileNeighbourUpdate()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<MapTile>() != null)
            {
                if (child.GetComponent<WaitTile>() != null)
                {
                    child.GetComponent<WaitTile>().FindFrontTile();
                    child.GetComponent<WaitTile>().FindEntrance();
                }

                child.GetComponent<MapTile>().UpdateNeighbours();
                child.GetComponent<MapTile>().currentLevel = transform.GetComponent<LevelControllerNew>();
            }
        }
    }

    [Button]
    public void GetChair()
    {
        avaiableChairs.Clear();

        foreach(GameObject mapTile in map)
        {
            if(mapTile != null)
            {
                if(mapTile.transform.childCount > 0)
                {
                    if(mapTile.transform.GetChild(0).GetComponent<TwinChair>() != null)
                    {
                        foreach(Chair chair in mapTile.transform.GetChild(0).GetComponent<TwinChair>().ChairList)
                        {
                            if (!chair.occupied)
                            {
                                avaiableChairs.Add(chair);
                            }
                            
                        }
                    }
                    else if(mapTile.transform.GetChild(0).GetComponent<Chair>() != null)
                    {
                        if (!mapTile.transform.GetChild(0).GetComponent<Chair>().occupied)
                        {
                            avaiableChairs.Add(mapTile.transform.GetChild(0).GetComponent<Chair>());
                        }  
                    }
                }
            }
        }
    }

}



