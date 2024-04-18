using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingBFS : SerializedMonoBehaviour
{
    public LevelControllerNew currentLevel;
    public GameObject[,] map;
    public List<Chair> chairs;
    public GameObject goal;

    public List<Vector3> path;
    public List<MapTile> finalPath = new List<MapTile>();

    public bool isAtEntrance = false;

    Queue<MapTile> BFSQueueList = new Queue<MapTile>();
    public Dictionary<MapTile, bool> visitStatus = new Dictionary<MapTile, bool>();
    public List<MapTile> traceBackPath = new List<MapTile>();

    public RaycastHit currentFloor;
    RaycastHit floor;

    bool isSeated = false;
    int counterForPathPicking;

    private void Start()
    {
        currentLevel.UpdateMap();
        currentLevel.GetChair();

        visitStatus.Clear();
        BFSQueueList.Clear();
        traceBackPath.Clear();
        path.Clear();
        finalPath.Clear();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentLevel.UpdateMap();
            currentLevel.GetChair();

            visitStatus.Clear();
            BFSQueueList.Clear();
            traceBackPath.Clear();
            path.Clear();
            finalPath.Clear();
            StartCoroutine(FindWay());
        }
    }

    
    [Button]
    public void GetMap()
    {
        counterForPathPicking = 0;

        for (int i = 0; i < currentLevel.avaiableChairs.Count; i++)
        {
            if (isSeated)
            {
                break;
            }

            currentLevel.UpdateMap();
            currentLevel.GetChair();

            visitStatus.Clear();
            BFSQueueList.Clear();
            traceBackPath.Clear();
            path.Clear();
            finalPath.Clear();

            if (currentLevel.avaiableChairs[i] != null)
            {
                if (!currentLevel.avaiableChairs[i].occupied)
                {
                    if (currentLevel.avaiableChairs[i].GetComponent<Chair>().colorID == GetComponent<Passenger>().colorID)
                    {
                        goal = currentLevel.avaiableChairs[i].gameObject;
                        StartCoroutine(FindWay());
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }
        }
    }

    //Breadth First Search Algorithm

    IEnumerator FindWay()
    {
        
        map = currentLevel.map;
        foreach (GameObject tile in map)
        {
            if (tile != null)
            {
                if (tile.GetComponent<MapTile>() != null)
                {
                    visitStatus[tile.GetComponent<MapTile>()] = false;
                }
            }
        }

        if (!Physics.Raycast(transform.position, Vector3.down, out currentFloor, 1f, 1 << LayerMask.NameToLayer("Floor")))
        {
            Debug.LogError("Not on the floor");
            yield return null;
        }
        else if (Physics.Raycast(transform.position, Vector3.down, out currentFloor, 1f, 1 << LayerMask.NameToLayer("Floor")))
        {
            if (currentFloor.collider.GetComponent<MapTile>() != null)
            {
                BFSQueueList.Enqueue(currentFloor.collider.GetComponent<MapTile>());
                traceBackPath.Add(currentFloor.collider.GetComponent<MapTile>());

                visitStatus[currentFloor.collider.GetComponent<MapTile>()] = true;
            }
            else
            {
                Debug.LogError("Not on the floor");
                yield return null;
            }
        }      

        //visitStatus[map[currentPos_col, currentPos_row].GetComponent<MapTile>()] = true;

        while(BFSQueueList.Count > 0)
        {
            MapTile tileMap = BFSQueueList.Dequeue();
            List<MapTile> tileMapNeighbours = tileMap.neighbourTiles;

            foreach(MapTile tile in tileMapNeighbours)
            {
                if (visitStatus[tile] == false)
                {
                    BFSQueueList.Enqueue(tile);
                    visitStatus[tile] = true;

                    if(tile.chair != null)
                    {
                        if(tile.chair == goal)
                        {
                            traceBackPath.Add(tile);
                        }                       
                    }
                    else
                    {
                        traceBackPath.Add(tile);
                    }
                }
            }
        }

        StartCoroutine(ContructWay());
        yield return null;
    }

    IEnumerator ContructWay()
    {
        Physics.Raycast(goal.transform.position, Vector3.down, out floor, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor"));
        counterForPathPicking = 0;

        //Lý do của việc gán lại thế này là để cho game kiểm tra hết tất cả các đường trống do List gốc có thể xếp các đường kế nhau không hợp lý
        //traceBackPath.Remove(floor.collider.GetComponent<MapTile>());
        //traceBackPath.Add(floor.collider.GetComponent<MapTile>());


        if (floor.collider.GetComponent<MapTile>() == currentFloor.collider.GetComponent<MapTile>())
        {
            yield return null;
        }

        if (floor.collider.GetComponent<MapTile>() != null)
        {
            if (currentFloor.collider.GetComponent<MapTile>().neighbourTiles.Contains(floor.collider.GetComponent<MapTile>()))
            {
                if (floor.collider.GetComponent<MapTile>().rightTile == currentFloor.collider.GetComponent<MapTile>() || floor.collider.GetComponent<MapTile>().leftTile == currentFloor.collider.GetComponent<MapTile>() || floor.collider.GetComponent<MapTile>().frontTile == currentFloor.collider.GetComponent<MapTile>())
                {
                    StartCoroutine(MoveToChair(0));
                    isSeated = true;
                    yield return null;
                }
                else
                {
                    Debug.LogError("Path not found for " + gameObject.name + "(1 step away check)");
                    isSeated = false;
                    yield return null;
                }
            }

            while (counterForPathPicking < 6)
            {
                StartCoroutine(FindPath(counterForPathPicking));
                if(finalPath.Count > 1)
                {
                    if (finalPath[0].transform.localPosition == currentFloor.collider.transform.localPosition)
                    {
                        foreach (MapTile mapTile in finalPath)
                        {
                            path.Add(mapTile.transform.localPosition + new Vector3(0, 0.5f, 0));
                        }

                        StartCoroutine(MoveToChair(1));
                        isSeated = true;
                        break;
                    }
                    else
                    {
                        Debug.LogError("Path not found for " + gameObject.name + "(long path check)");
                        isSeated = false;
                        counterForPathPicking++;
                    }
                }
                else
                {
                    Debug.LogError("Path not found for " + gameObject.name + "(long path check)");
                    isSeated = false;
                    counterForPathPicking++;
                }
            }

            yield return null;
# region OldPathFinding
            //for (int i = traceBackPath.IndexOf(floor.collider.gameObject.GetComponent<MapTile>()); i >= 0; i--)
            //{
            //    if(finalPath.Count == 0)
            //    {
            //        finalPath.Add(traceBackPath[i]);
            //    }
            //    else if(finalPath.Count == 1)
            //    {
            //        if (finalPath[finalPath.Count - 1].GetComponent<MapTile>().rightTile != null)
            //        {
            //            if (finalPath[finalPath.Count - 1].GetComponent<MapTile>().rightTile == traceBackPath[i])
            //            {
            //                finalPath.Add(traceBackPath[i]);
            //            }
            //        }

            //        if(finalPath[finalPath.Count - 1].GetComponent<MapTile>().leftTile != null)
            //        {
            //            if (finalPath[finalPath.Count - 1].GetComponent<MapTile>().leftTile == traceBackPath[i])
            //            {
            //                finalPath.Add(traceBackPath[i]);
            //            }
            //        }

            //        if (finalPath[finalPath.Count - 1].GetComponent<MapTile>().frontTile != null)
            //        {
            //            if (finalPath[finalPath.Count - 1].GetComponent<MapTile>().frontTile == traceBackPath[i])
            //            {
            //                finalPath.Add(traceBackPath[i]);
            //            }
            //        }
            //    }
            //    else if (finalPath.Count >= 2)
            //    {
            //        //Loại bỏ phần này nếu agent được phép đi chéo
            //        if (finalPath[finalPath.Count - 1].neighbourTiles.Contains(traceBackPath[i]))
            //        {
            //            finalPath.Add(traceBackPath[i]);

            //            //if (traceBackPath[i].neighbourTiles.Contains(currentFloor.collider.gameObject.GetComponent<MapTile>()))
            //            //{
            //            //    finalPath.Add(currentFloor.collider.gameObject.GetComponent<MapTile>());
            //            //    break;
            //            //}
            //        }
            //        else
            //        {
            //            if (finalPath[finalPath.Count - 1] == floor.collider.gameObject.GetComponent<MapTile>().rightTile || finalPath[finalPath.Count - 1] == floor.collider.gameObject.GetComponent<MapTile>().leftTile || finalPath[finalPath.Count - 1] == floor.collider.gameObject.GetComponent<MapTile>().frontTile)
            //            {
            //                foreach(MapTile floor in finalPath[finalPath.Count - 1].neighbourTiles)
            //                {
            //                    if (traceBackPath.GetRange(0, traceBackPath.IndexOf(this.floor.collider.gameObject.GetComponent<MapTile>())).Contains(floor))
            //                    {
            //                        continue;
            //                    }
            //                }
            //                finalPath.Remove(finalPath[finalPath.Count - 1]);
            //                i++;
            //                continue;
            //            }
            //            else
            //            {
            //                continue;
            //            }
            //        }
            //    }
            //}

            //finalPath.Reverse();
# endregion
        }
        else
        {
            Debug.LogError("Not a valid goal");
            yield return null;
        }
        #region OldPathChecker
        //if (finalPath[0].transform.localPosition == currentFloor.collider.transform.localPosition)
        //{
        //    foreach (MapTile mapTile in finalPath)
        //    {
        //        path.Add(mapTile.transform.localPosition + new Vector3(0, 0.5f, 0));
        //    }

        //    StartCoroutine(MoveToChair(1));
        //    isSeated = true;
        //    yield return null;
        //}
        //else
        //{
        //    Debug.LogError("Path not found for " + gameObject.name + "(long path check)");
        //    isSeated= false;
        //    yield return null;
        //}
        #endregion
    }

    IEnumerator FindPath(int ID)
    {
        finalPath.Clear();

        switch (ID)
        {
            case 0:
                if (floor.collider.gameObject.GetComponent<MapTile>().rightTile != null)
                {
                    if (traceBackPath.Contains(floor.collider.gameObject.GetComponent<MapTile>().rightTile))
                    {
                        finalPath.Add(floor.collider.GetComponent<MapTile>());
                        finalPath.Add(floor.collider.GetComponent<MapTile>().rightTile);

                        for (int i = traceBackPath.IndexOf(floor.collider.GetComponent<MapTile>().rightTile); i >= 0; i--)
                        {
                            if (traceBackPath[i] == floor.collider.GetComponent<MapTile>() || traceBackPath[i] == floor.collider.GetComponent<MapTile>().rightTile)
                            {
                                continue;
                            }

                            if (finalPath[finalPath.Count - 1].neighbourTiles.Contains(traceBackPath[i]))
                            {
                                finalPath.Add(traceBackPath[i]);
                            }
                        }

                        finalPath.Reverse();

                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

            case 1:
                if (floor.collider.gameObject.GetComponent<MapTile>().frontTile != null)
                {
                    if (traceBackPath.Contains(floor.collider.gameObject.GetComponent<MapTile>().frontTile))
                    {
                        finalPath.Add(floor.collider.GetComponent<MapTile>());
                        finalPath.Add(floor.collider.GetComponent<MapTile>().frontTile);

                        for (int i = traceBackPath.IndexOf(floor.collider.GetComponent<MapTile>().frontTile); i >= 0; i--)
                        {
                            if (traceBackPath[i] == floor.collider.GetComponent<MapTile>() || traceBackPath[i] == floor.collider.GetComponent<MapTile>().frontTile)
                            {
                                continue;
                            }

                            if (finalPath[finalPath.Count - 1].neighbourTiles.Contains(traceBackPath[i]))
                            {
                                finalPath.Add(traceBackPath[i]);
                            }
                        }

                        finalPath.Reverse();

                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

            case 2:
                if (floor.collider.gameObject.GetComponent<MapTile>().leftTile != null)
                {
                    if (traceBackPath.Contains(floor.collider.gameObject.GetComponent<MapTile>().leftTile))
                    {
                        finalPath.Add(floor.collider.GetComponent<MapTile>());
                        finalPath.Add(floor.collider.GetComponent<MapTile>().leftTile);

                        for (int i = traceBackPath.IndexOf(floor.collider.GetComponent<MapTile>().leftTile); i >= 0; i--)
                        {
                            if (traceBackPath[i] == floor.collider.GetComponent<MapTile>() || traceBackPath[i] == floor.collider.GetComponent<MapTile>().leftTile)
                            {
                                continue;
                            }

                            if (finalPath[finalPath.Count - 1].neighbourTiles.Contains(traceBackPath[i]))
                            {
                                finalPath.Add(traceBackPath[i]);
                            }
                        }

                        finalPath.Reverse();

                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

                //Khu vực quét sâu hơn, cũng tương đương với việc nặng tài nguyên hơn
            case 3:
                if(floor.collider.gameObject.GetComponent<MapTile>().rightTile != null)
                {
                    if (traceBackPath.Contains(floor.collider.gameObject.GetComponent<MapTile>().rightTile))
                    {
                        finalPath.Add(floor.collider.GetComponent<MapTile>());
                        finalPath.Add(floor.collider.GetComponent<MapTile>().rightTile);

                        for (int i = traceBackPath.IndexOf(floor.collider.GetComponent<MapTile>().rightTile); i >= 0; i--)
                        {
                            if (traceBackPath.IndexOf(floor.collider.GetComponent<MapTile>().rightTile) != traceBackPath.Count - 1 && i == traceBackPath.IndexOf(floor.collider.GetComponent<MapTile>().rightTile))
                            {
                                for(int a = 0; a < 2; a++)
                                {
                                    for (int j = traceBackPath.IndexOf(floor.collider.GetComponent<MapTile>().rightTile); j < traceBackPath.Count; j++)
                                    {

                                        if (finalPath[finalPath.Count - 1].neighbourTiles.Contains(traceBackPath[j]))
                                        {
                                            if (!finalPath.Contains(traceBackPath[j]))
                                            {
                                                finalPath.Add(traceBackPath[j]);
                                            }
                                            
                                        }
                                    }

                                    for (int k = traceBackPath.Count - 1; k >= traceBackPath.IndexOf(floor.collider.GetComponent<MapTile>().rightTile); k--)
                                    {

                                        if (finalPath[finalPath.Count - 1].neighbourTiles.Contains(traceBackPath[k]))
                                        {
                                            if (!finalPath.Contains(traceBackPath[k]))
                                            {
                                                finalPath.Add(traceBackPath[k]);
                                            }
                                            
                                        }
                                    }
                                }
                                
                            }

                            if (finalPath[finalPath.Count - 1].neighbourTiles.Contains(traceBackPath[i]))
                            {
                                if (!finalPath.Contains(traceBackPath[i]))
                                {
                                    finalPath.Add(traceBackPath[i]);
                                }
                                
                            }
                        }

                        finalPath.Reverse();

                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
                
            case 4:
                if(floor.collider.gameObject.GetComponent<MapTile>().frontTile != null)
                {
                    if (traceBackPath.Contains(floor.collider.gameObject.GetComponent<MapTile>().frontTile))
                    {
                        finalPath.Add(floor.collider.GetComponent<MapTile>());
                        finalPath.Add(floor.collider.GetComponent<MapTile>().frontTile);

                        for (int i = traceBackPath.IndexOf(floor.collider.GetComponent<MapTile>().frontTile); i >= 0; i--)
                        {
                            if (traceBackPath.IndexOf(floor.collider.GetComponent<MapTile>().frontTile) != traceBackPath.Count - 1 && i == traceBackPath.IndexOf(floor.collider.GetComponent<MapTile>().frontTile))
                            {
                                for (int a = 0; a < 2; a++)
                                {
                                    for (int j = traceBackPath.IndexOf(floor.collider.GetComponent<MapTile>().frontTile); j < traceBackPath.Count; j++)
                                    {

                                        if (finalPath[finalPath.Count - 1].neighbourTiles.Contains(traceBackPath[j]))
                                        {
                                            if (!finalPath.Contains(traceBackPath[j]))
                                            {
                                                finalPath.Add(traceBackPath[j]);
                                            }
                                        }
                                    }

                                    for (int k = traceBackPath.Count - 1; k >= traceBackPath.IndexOf(floor.collider.GetComponent<MapTile>().frontTile); k--)
                                    {

                                        if (finalPath[finalPath.Count - 1].neighbourTiles.Contains(traceBackPath[k]))
                                        {
                                            if (!finalPath.Contains(traceBackPath[k]))
                                            {
                                                finalPath.Add(traceBackPath[k]);
                                            }
                                        }
                                    }
                                }
                            }


                            if (finalPath[finalPath.Count - 1].neighbourTiles.Contains(traceBackPath[i]))
                            {
                                if (!finalPath.Contains(traceBackPath[i]))
                                {
                                    finalPath.Add(traceBackPath[i]);
                                }
                            }
                        }

                        finalPath.Reverse();

                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
                
            case 5:
                if(floor.collider.gameObject.GetComponent<MapTile>().leftTile != null)
                {
                    if (traceBackPath.Contains(floor.collider.gameObject.GetComponent<MapTile>().leftTile))
                    {
                        finalPath.Add(floor.collider.GetComponent<MapTile>());
                        finalPath.Add(floor.collider.GetComponent<MapTile>().leftTile);

                        for (int i = traceBackPath.IndexOf(floor.collider.GetComponent<MapTile>().leftTile); i >= 0; i--)
                        {
                            if(traceBackPath.IndexOf(floor.collider.GetComponent<MapTile>().leftTile) != traceBackPath.Count - 1 && i == traceBackPath.IndexOf(floor.collider.GetComponent<MapTile>().leftTile))
                            {
                                for (int a = 0; a < 2; a++)
                                {
                                    for (int j = traceBackPath.IndexOf(floor.collider.GetComponent<MapTile>().leftTile); j < traceBackPath.Count; j++)
                                    {

                                        if (finalPath[finalPath.Count - 1].neighbourTiles.Contains(traceBackPath[j]))
                                        {
                                            if (!finalPath.Contains(traceBackPath[j]))
                                            {
                                                finalPath.Add(traceBackPath[j]);
                                            }
                                        }
                                    }

                                    for (int k = traceBackPath.Count - 1; k >= traceBackPath.IndexOf(floor.collider.GetComponent<MapTile>().leftTile); k--)
                                    {

                                        if (finalPath[finalPath.Count - 1].neighbourTiles.Contains(traceBackPath[k]))
                                        {
                                            if (!finalPath.Contains(traceBackPath[k]))
                                            {
                                                finalPath.Add(traceBackPath[k]);
                                            }
                                        }
                                    }
                                }
                            }

                            if (finalPath[finalPath.Count - 1].neighbourTiles.Contains(traceBackPath[i]))
                            {
                                if (!finalPath.Contains(traceBackPath[i]))
                                {
                                    finalPath.Add(traceBackPath[i]);
                                }
                            }
                        }

                        finalPath.Reverse();

                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
        }

        yield return null;
    }

    IEnumerator MoveToChair(int moveID)
    {
        switch (moveID)
        {
            case 0:
                transform.DOLocalMove(floor.collider.transform.localPosition + new Vector3(0, 0.5f, 0), 1f).SetEase(Ease.Linear)
                    .OnStart(delegate
                    {
                        if(transform.localPosition.x - floor.collider.transform.localPosition.x > 0)
                        {
                            GetComponent<PassengerColor>().bodyPartsNeedColorChange[0].transform.DORotate(new Vector3(0, -90, 0), .1f);
                        }
                        else if (transform.localPosition.x - floor.collider.transform.localPosition.x < 0)
                        {
                            GetComponent<PassengerColor>().bodyPartsNeedColorChange[0].transform.DORotate(new Vector3(0, 90, 0), .1f);
                        }

                        if (transform.localPosition.z - floor.collider.transform.localPosition.z > 0)
                        {
                            GetComponent<PassengerColor>().bodyPartsNeedColorChange[0].transform.DORotate(Vector3.zero, .1f);
                        }
                        else if (transform.localPosition.z - floor.collider.transform.localPosition.z < 0)
                        {
                            GetComponent<PassengerColor>().bodyPartsNeedColorChange[0].transform.DORotate(new Vector3(0, 180, 0), .1f);
                        }

                        currentLevel.avaiableChairs.Remove(goal.GetComponent<Chair>());
                        currentLevel.passengers.Remove(gameObject.GetComponent<PathFindingAStar>());
                        gameObject.GetComponent<WaitInLine>().waitTile.occupied = false;

                        goal.GetComponent<Chair>().occupied = true;
                        if (goal.transform.parent.GetComponent<TwinChair>() != null)
                        {
                            goal.transform.parent.GetComponent<TwinChair>().moveAble = false;
                        }
                        else if (goal.GetComponent<Chair>() != null)
                        {
                            goal.GetComponent<Chair>().moveAble = false;
                        }

                        foreach (PathFindingAStar passenger in currentLevel.passengers)
                        {
                            passenger.GetComponent<WaitInLine>().MoveToNextTile();
                        }

                        for (int i = 0; i < currentLevel.passengers.Count; i++)
                        {
                            if (currentLevel.passengers[i] != null)
                            {
                                if (currentLevel.passengers[i].GetComponent<PathFindingAStar>().enabled)
                                {
                                    if (currentLevel.passengers[i].GetComponent<PathFindingAStar>().isAtEntrance)
                                    {
                                        currentLevel.passengers[i].GetComponent<PathFindingAStar>().FindPath();
                                        break;
                                    }
                                }
                            }
                        }
                    })
                    .OnComplete(delegate
                    {
                        GetComponent<PassengerColor>().bodyPartsNeedColorChange[0].transform.DORotate(Vector3.zero, .1f);

                        if (goal.transform.parent.GetComponent<TwinChair>() != null)
                        {
                            goal.transform.parent.GetComponent<TwinChair>().moveAble = true;
                        }
                        else if (goal.GetComponent<Chair>() != null)
                        {
                            goal.GetComponent<Chair>().moveAble = true;
                        }
                        gameObject.transform.parent = goal.transform;
                        gameObject.transform.localPosition = new Vector3(0, 0, 0);

                        gameObject.GetComponent<WaitInLine>().enabled = false;
                        gameObject.GetComponent<PathFindingAStar>().enabled = false;
                }).WaitForCompletion(); ;
                break;
            case 1:
                transform.DOLocalPath(path.ToArray(), 1f).SetEase(Ease.Linear)
                    .OnWaypointChange(index =>
                    {
                        if(index + 1 != -1)
                        {
                            if (path[index].x - path[index + 1].x > 0)
                            {
                                GetComponent<PassengerColor>().bodyPartsNeedColorChange[0].transform.DORotate(new Vector3(0, -90, 0), .1f);
                            }
                            else if (path[index].x - path[index + 1].x < 0)
                            {
                                GetComponent<PassengerColor>().bodyPartsNeedColorChange[0].transform.DORotate(new Vector3(0, 90, 0), .1f);
                            }

                            if (path[index].z - path[index + 1].z > 0)
                            {
                                GetComponent<PassengerColor>().bodyPartsNeedColorChange[0].transform.DORotate(Vector3.zero, .1f);
                            }
                            else if (path[index].z - path[index + 1].z < 0)
                            {
                                GetComponent<PassengerColor>().bodyPartsNeedColorChange[0].transform.DORotate(new Vector3(0, 180, 0), .1f);
                            }
                        }
                    })
                    .OnStart(delegate
                    {
                        currentLevel.avaiableChairs.Remove(goal.GetComponent<Chair>());
                        currentLevel.passengers.Remove(gameObject.GetComponent<PathFindingAStar>());
                        gameObject.GetComponent<WaitInLine>().waitTile.occupied = false;
                    

                        goal.GetComponent<Chair>().occupied = true;
                        if (goal.transform.parent.GetComponent<TwinChair>() != null)
                        {
                            goal.transform.parent.GetComponent<TwinChair>().moveAble = false;
                        }
                        else if (goal.GetComponent<Chair>() != null)
                        {
                            goal.GetComponent<Chair>().moveAble = false;
                        }

                        foreach (PathFindingAStar passenger in currentLevel.passengers)
                        {
                            passenger.GetComponent<WaitInLine>().MoveToNextTile();
                        }

                        for (int i = 0; i < currentLevel.passengers.Count; i++)
                        {
                            if (currentLevel.passengers[i] != null)
                            {
                                if (currentLevel.passengers[i].GetComponent<PathFindingAStar>().enabled)
                                {
                                    if (currentLevel.passengers[i].GetComponent<PathFindingAStar>().isAtEntrance)
                                    {
                                        currentLevel.passengers[i].GetComponent<PathFindingAStar>().FindPath();
                                        break;
                                    }
                                }
                            }
                        }
                }).OnComplete(delegate
                {
                    GetComponent<PassengerColor>().bodyPartsNeedColorChange[0].transform.DORotate(Vector3.zero, .1f);

                    if (goal.transform.parent.GetComponent<TwinChair>() != null)
                    {
                        goal.transform.parent.GetComponent<TwinChair>().moveAble = true;
                    }
                    else if (goal.GetComponent<Chair>() != null)
                    {
                        goal.GetComponent<Chair>().moveAble = true;
                    }
                    gameObject.transform.parent = goal.transform;
                    gameObject.transform.localPosition = new Vector3(0, 0, 0);

                    gameObject.GetComponent<WaitInLine>().enabled = false;
                    gameObject.GetComponent<PathFindingAStar>().enabled = false;
                }).WaitForCompletion();
                break;
        }

        yield return null;
    }
}
