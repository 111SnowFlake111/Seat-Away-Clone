using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[System.Serializable]
public class LevelControllerNew : SerializedMonoBehaviour
{
    [Header("----------MAP CONFIG-----------")]
    public int mapRow;
    public int mapColumn;
    public GameObject[,] map;

    public List<Material> avaiableColors;
    public List<Chair> avaiableChairs;
    public List<PathFindingAStar> passengers;
    public List<WaitTile> sideway;

    [Space]
    [Header("-----TIME LIMIT(s)-----")]
    public int timeLimit;
    [NonSerialized] public bool passengerMoving;

    [Space]
    [Header("-----MISC-----")]
    public MapTile mapTile;
    public GameObject wallFront;
    public GameObject wallBack;
    public GameObject wallLeft;
    public GameObject wallRight;
    public Material floorColor_1;
    public Material floorColor_2;

    public void AddOneColumnToTheLeft()
    {
        mapColumn++;
        Vector3 positionOverwrite = new Vector3(2 - mapColumn - 1, 0, 2);
        for(int i = 0; i < mapColumn; i++)
        {
            var tileInstance = Instantiate(mapTile, transform);
            tileInstance.transform.localPosition = positionOverwrite;
            positionOverwrite -= new Vector3(0, 0, 1);
        }

        UpdateMap();

        foreach(GameObject tile in map)
        {
            if(tile != null)
            {
                if(tile.GetComponent<MapTile>() != null)
                {
                    tile.GetComponent<MapTile>().UpdateNeighbours();
                }
            }
        }

        AdjustSize();
    }

    //Mainly to allow Player to be able to see as the play field grow bigger
    public void AdjustSize()
    {
        Vector3 sizeEachColumn = new Vector3(0.15f, 0.15f, 0.15f);
        transform.localScale = sizeEachColumn * mapColumn;
    }

    public void CheckWinCondition()
    {
        foreach(PathFindingAStar passenger in passengers)
        {
            if (!passenger.isSeated)
            {
                return;
            }
        }

        if (!passengerMoving)
        {
            //Activate win pop up screen
        }
    }

    public void SetMap()
    {
        //Note To Self: Each row increase will take up to 1f z
        //Note To Self: Each column take up to 1f x
        //Note To Self: Every time a row is added (usually to the bottom row), left n right wall must be moved downward by 0.5f z and collider box must be increased by 0.5f in z size
        //Note To Self: Every time a column is added (usually to the left side), front n back wall must be moved to the left by 0.5 x and collider box must be increased by 1f in x size
        //These value is to move the wall

        map = new GameObject[mapColumn + 1, mapRow];

        wallRight.transform.localPosition = new Vector3(1.7f, -0.5f, 3.5f - 0.5f * mapRow);
        wallLeft.transform.localPosition = new Vector3(1.3f - mapColumn, -0.5f, 3.5f - 0.5f * mapRow);
        wallFront.transform.localPosition = new Vector3(1.5f - 0.5f * mapColumn, -0.5f, 2.7f);
        wallBack.transform.localPosition = new Vector3(1.5f - 0.5f * mapColumn, -0.5f, -0.5f * mapRow);

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
                        }
                        else
                        {
                            map[column, row].GetComponent<MeshRenderer>().material = floorColor_1;
                        }
                    }
                    else
                    {
                        if (column % 2 != 0)
                        {
                            map[column, row].GetComponent<MeshRenderer>().material = floorColor_2;
                        }
                        else
                        {
                            map[column, row].GetComponent<MeshRenderer>().material = floorColor_1;
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
                        }
                        else
                        {
                            map[column, row].GetComponent<MeshRenderer>().material = floorColor_1;
                        }
                    }
                    else
                    {
                        if (column % 2 != 0)
                        {
                            map[column, row].GetComponent<MeshRenderer>().material = floorColor_2;
                        }
                        else
                        {
                            map[column, row].GetComponent<MeshRenderer>().material = floorColor_1;
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

        MapTileNeighbourUpdate();
        AdjustSize();
    }

    [Button]
    void MapTileNeighbourUpdate()
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



