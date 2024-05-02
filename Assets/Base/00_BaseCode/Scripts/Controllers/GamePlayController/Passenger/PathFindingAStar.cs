using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Overlays;
using UnityEngine;

public class PathFindingAStar : SerializedMonoBehaviour
{
    public LevelControllerNew currentLevel;

    public List<MapTile> checkedTiles = new List<MapTile>();
    public List<MapTile> openTiles = new List<MapTile>();

    public List<MapTile> truePath = new List<MapTile>();

    public Chair goal;

    public MapTile currentMapTile;

    public bool isAtEntrance = false;
    RaycastHit currentFloor;
    [NonSerialized] public RaycastHit goalTileHit;

    [NonSerialized] public bool isSeated = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindPath();
        }
    }

    public void FindPath()
    {
        Physics.Raycast(transform.position, Vector3.down, out currentFloor, 1f, 1 << LayerMask.NameToLayer("Floor"));
        currentMapTile = currentFloor.collider.GetComponent<MapTile>();
        //Debug.LogError("current floor position recorded");

        for (int i = 0; i < currentLevel.avaiableChairs.Count; i++)
        {
            if (isSeated)
            {
                break;
            }
            //Debug.LogError(GetComponent<Passenger>().colorID);

            if(GetComponent<Passenger>().colorID <= 1)
            {
                if (currentLevel.avaiableChairs[i].GetComponent<Chair>().colorID == 0 || currentLevel.avaiableChairs[i].GetComponent<Chair>().colorID == 1)
                {
                    goal = currentLevel.avaiableChairs[i];
                    //Debug.LogError(goal.name + " is a potential chair for " + name);
                    Physics.Raycast(goal.transform.position, Vector3.down, out goalTileHit, 1f, 1 << LayerMask.NameToLayer("Floor"));
                }
                else
                {
                    continue;
                }
            }
            else
            {
                if (currentLevel.avaiableChairs[i].GetComponent<Chair>().colorID == GetComponent<Passenger>().colorID)
                {

                    goal = currentLevel.avaiableChairs[i];
                    //Debug.LogError(goal.name + " is a potential chair for " + name);
                    Physics.Raycast(goal.transform.position, Vector3.down, out goalTileHit, 1f, 1 << LayerMask.NameToLayer("Floor"));
                }
                else
                {
                    continue;
                }
            }
            
            
            

            if (!currentLevel.avaiableChairs[i].occupied)
            {
                //currentLevel.UpdateMap();
                checkedTiles.Clear();
                openTiles.Clear();

                

                foreach (GameObject tile in currentLevel.map)
                {
                    if (tile != null)
                    {
                        if (tile.GetComponent<MapTile>() != null)
                        {
                            if (tile.GetComponent<MapTile>().chair != null)
                            {
                                continue;
                            }
                            else
                            {
                                openTiles.Add(tile.GetComponent<MapTile>());
                            }
                        }
                    }
                }

                openTiles.Add(goalTileHit.collider.GetComponent<MapTile>());

                FilterPath(currentFloor.collider.GetComponent<MapTile>(), goalTileHit.collider.GetComponent<MapTile>());
            }
        }
    }

    //int GetGCost(MapTile startTile, MapTile currTile)
    //{
    //    //int dx = Mathf.Abs(Mathf.RoundToInt(startTile.transform.localPosition.x - currTile.transform.localPosition.x));
    //    //int dz = Mathf.Abs(Mathf.RoundToInt(startTile.transform.localPosition.z - currTile.transform.localPosition.z));

    //    int startRow = 0;
    //    int startColumn = 0;
    //    int endRow = 0;
    //    int endColumn = 0;

    //    for(int i = 0; i < currentLevel.map.GetLength(1); i++)
    //    {
    //        for(int j = 0; j < currentLevel.map.GetLength(0); j++)
    //        {
    //            if (currentLevel.map[i, j] != null)
    //            {
    //                if (currentLevel.map[i, j].GetComponent<MapTile>() == startTile)
    //                {
    //                    startRow = i;
    //                    startColumn = j;
    //                }

    //                if (currentLevel.map[i, j].GetComponent<MapTile>() == currTile)
    //                {
    //                    endRow = i;
    //                    endColumn = j;
    //                }
    //            }
    //        }
    //    }

    //    //Debug.LogError("Result from Get G Cost " + startRow);
    //    //Debug.LogError("Result from Get G Cost " + startColumn);
    //    //Debug.LogError("Result from Get G Cost " + endRow);
    //    //Debug.Log("Result from Get G Cost " + endColumn);
    //    return Mathf.Abs(startColumn - endColumn) + Mathf.Abs(startRow - endRow);
    //}

    //int GetHCost(MapTile currTile)
    //{
    //    //int dx = Mathf.Abs(Mathf.RoundToInt(currTile.transform.localPosition.x - goalTileHit.collider.transform.localPosition.x));
    //    //int dz = Mathf.Abs(Mathf.RoundToInt(currTile.transform.localPosition.z - goalTileHit.collider.transform.localPosition.z));
    //    //return dx + dz;

    //    int currRow = 0;
    //    int currColumn = 0;
    //    int endRow = 0;
    //    int endColumn = 0;

    //    for (int i = 0; i < currentLevel.map.GetLength(1); i++)
    //    {
    //        for (int j = 0; j < currentLevel.map.GetLength(0); j++)
    //        {
    //            if (currentLevel.map[i, j] != null)
    //            {
    //                if (currentLevel.map[i, j].GetComponent<MapTile>() == currTile)
    //                {
    //                    currRow = i;
    //                    currColumn = j;
    //                }

    //                if (currentLevel.map[i, j].GetComponent<MapTile>() == goalTileHit.collider.GetComponent<MapTile>())
    //                {
    //                    endRow = i;
    //                    endColumn = j;
    //                }
    //            }
    //        }
    //    }

    //    return Mathf.Abs(currColumn - endColumn) + Mathf.Abs(currRow - endRow);
    //}

    void FilterPath(MapTile currTile, MapTile goalTile)
    {
        truePath.Clear();

        //truePath.Add(currentFloor.collider.GetComponent<MapTile>());

        //queuedTiles.Enqueue(currentFloor.collider.GetComponent<MapTile>());
        //tileGCost[currentFloor.collider.GetComponent<MapTile>()] = 0;
        //tileHCost[currentFloor.collider.GetComponent<MapTile>()] = GetHCost(currentFloor.collider.GetComponent<MapTile>());
        //tileFCost[currentFloor.collider.GetComponent<MapTile>()] = tileGCost[currentFloor.collider.GetComponent<MapTile>()] + tileHCost[currentFloor.collider.GetComponent<MapTile>()];

        //while(queuedTiles.Count > 0)
        //{
        //    MapTile floor = queuedTiles.Dequeue();
        //    if (!checkedTiles.Contains(floor))
        //    {
        //        checkedTiles.Add(floor);

        //        tileHCost[floor] = GetHCost(floor);
        //        tileGCost[floor] = GetGCost(currentFloor.collider.GetComponent<MapTile>(), floor);
        //        tileFCost[floor] = tileGCost[floor] + tileHCost[floor];

        //        if(floor == goalTileHit.collider.GetComponent<MapTile>())
        //        {
        //            break;
        //        }

        //        foreach(MapTile neighbour in floor.neighbourTiles)
        //        {
        //            if (openTiles.Contains(neighbour))
        //            {
        //                if (!checkedTiles.Contains(neighbour))
        //                {
        //                    queuedTiles.Enqueue(neighbour);
        //                }
        //            }
        //        }
        //    }

        //}

        //foreach (MapTile tile in openTiles)
        //{
        //    if (!checkedTiles.Contains(tile))
        //    {
        //        checkedTiles.Add(tile);

        //        tileHCost[tile] = GetHCost(tile);
        //        tileGCost[tile] = GetGCost(currentFloor.collider.GetComponent<MapTile>(), tile);
        //        tileFCost[tile] = tileGCost[tile] + tileHCost[tile];
        //    }
        //}

        var toSearch = new List<MapTile>() { currTile };
        var processed = new List<MapTile>();

        while (toSearch.Any())
        {
            var current = toSearch[0];
            foreach (var t in toSearch)
            {
                if (t.F < current.F || t.F == current.F && t.H < current.H)
                {
                    current = t;
                }
            }

            processed.Add(current);
            toSearch.Remove(current);

            if (current == goalTile)
            {
                var currentPathTile = goalTile;
                var path = new List<MapTile>();
                var count = 100;
                while (currentPathTile != currTile)
                {
                    path.Add(currentPathTile);
                    currentPathTile = currentPathTile.linkedTile;
                    count--;
                    if (count < 0)
                    {
                        throw new Exception();
                    }
                }

                truePath = path;
                
            }

            foreach (var neighbor in current.neighbourTiles.Where(t => openTiles.Contains(t) && !processed.Contains(t)))
            {
                var inSearch = toSearch.Contains(neighbor);

                var costToNeighbor = current.G + current.GetDistance(neighbor);

                if (!inSearch || costToNeighbor < neighbor.G)
                {
                    neighbor.G = costToNeighbor;
                    neighbor.linkedTile = current;

                    if (!inSearch)
                    {
                        neighbor.H = neighbor.GetDistance(goalTile);
                        toSearch.Add(neighbor);
                    }
                }
            }
        }
        

        //for (int i = 0; i < currentLevel.map.GetLength(1); i++)
        //{
        //    for (int j = 0; j < currentLevel.map.GetLength(0); j++)
        //    {
        //        if (currentLevel.map[j, i] != null)
        //        {
        //            if(!checkedTiles.Contains(currentLevel.map[j, i].GetComponent<MapTile>()))
        //            {
        //                if (currentLevel.map[j, i].GetComponent<MapTile>().chair == null)
        //                {
        //                    checkedTiles.Add(currentLevel.map[j, i].GetComponent<MapTile>());

        //                    tileHCost[currentLevel.map[j, i].GetComponent<MapTile>()] = GetHCost(currentLevel.map[j, i].GetComponent<MapTile>());
        //                    tileGCost[currentLevel.map[j, i].GetComponent<MapTile>()] = GetGCost(currentFloor.collider.GetComponent<MapTile>(), currentLevel.map[j, i].GetComponent<MapTile>());
        //                    tileFCost[currentLevel.map[j, i].GetComponent<MapTile>()] = tileGCost[currentLevel.map[j, i].GetComponent<MapTile>()] + tileHCost[currentLevel.map[j, i].GetComponent<MapTile>()];
        //                }
        //            }
        //        }
        //    }
        //}

        //if (!checkedTiles.Contains(goalTileHit.collider.GetComponent<MapTile>()))
        //{
        //    checkedTiles.Add(goalTileHit.collider.GetComponent<MapTile>());

        //    tileHCost[goalTileHit.collider.GetComponent<MapTile>()] = GetHCost(goalTileHit.collider.GetComponent<MapTile>());
        //    tileGCost[goalTileHit.collider.GetComponent<MapTile>()] = GetGCost(currentFloor.collider.GetComponent<MapTile>(), goalTileHit.collider.GetComponent<MapTile>());
        //    tileFCost[goalTileHit.collider.GetComponent<MapTile>()] = tileGCost[goalTileHit.collider.GetComponent<MapTile>()] + tileHCost[goalTileHit.collider.GetComponent<MapTile>()];
        //}

        //checkedTiles.Remove(currentFloor.collider.GetComponent<MapTile>());
        //checkedTiles.Insert(0, currentFloor.collider.GetComponent<MapTile>());

        //for (int i = checkedTiles.IndexOf(goalTileHit.collider.GetComponent<MapTile>()); i >= 0; i--)
        //{
        //    if(truePath.Count <= 0)
        //    {
        //        truePath.Add(checkedTiles[i]);
        //    }
        //    else
        //    {
        //        if (truePath[truePath.Count - 1].neighbourTiles.Contains(checkedTiles[i]))
        //        {
        //            if (tileFCost[truePath[truePath.Count - 1]] > tileFCost[checkedTiles[i]])
        //            {
        //                truePath.Add(checkedTiles[i]);
        //            }
        //            else if (tileFCost[truePath[truePath.Count - 1]] == tileFCost[checkedTiles[i]])
        //            {
        //                if (tileGCost[truePath[truePath.Count - 1]] > tileGCost[checkedTiles[i]])
        //                {
        //                    truePath.Add(checkedTiles[i]);
        //                }
        //            }
        //        }
        //    }
        //}

        //truePath.Reverse();

        truePath.Reverse();

        if(truePath.Count > 0)
        {
            if (truePath[0].neighbourTiles.Contains(currTile))
            {
                truePath.Insert(0, currTile);
            }

            if (truePath[0] == currentFloor.collider.GetComponent<MapTile>())
            {
                if (truePath.Count < 2)
                {
                    //Debug.LogError("Path found (short ver)");
                    isSeated = true;
                    StartCoroutine(MoveToChair(0));
                }
                else
                {
                    //Debug.LogError("Path found (long ver)");
                    isSeated = true;
                    StartCoroutine(MoveToChair(1));
                }
            }
        }
        else
        {
            //Debug.LogError("Path not found for " + gameObject.name);
            return;
        }

        #region Experimental
        //bool finishedFindingPath = false;
        //while (true)
        //{
        //    for (int i = 0; i < truePath[truePath.Count - 1].neighbourTiles.Count; i++)
        //    {
        //        if (checkedTiles.Contains(truePath[truePath.Count - 1].neighbourTiles[i]))
        //        {
        //            if(!truePath.Contains(truePath[truePath.Count - 1].neighbourTiles[i]))
        //            {
        //                nextTile = truePath[truePath.Count - 1].neighbourTiles[i];
        //                break;
        //            }
        //            else
        //            {
        //                finishedFindingPath = true;
        //                nextTile = null;
        //            }
        //        }
        //        else
        //        {
        //            finishedFindingPath = true;
        //            nextTile = null;
        //        }
        //    }

        //    if (nextTile == null)
        //    {
        //        finishedFindingPath = true;
        //        break;
        //    }
        //    else
        //    {
        //        for (int i = 0; i < truePath[truePath.Count - 1].neighbourTiles.Count; i++)
        //        {
        //            if (truePath[truePath.Count - 1].neighbourTiles[i] == nextTile)
        //            {
        //                continue;
        //            }

        //            if (checkedTiles.Contains(truePath[truePath.Count - 1].neighbourTiles[i]))
        //            {
        //                if (truePath[truePath.Count - 1].neighbourTiles[i].chair != null)
        //                {
        //                    Debug.LogError("Chair found");
        //                    truePath.Add(truePath[truePath.Count - 1].neighbourTiles[i]);
        //                    finishedFindingPath = true;
        //                    break;
        //                }
        //                else if(truePath.Contains(truePath[truePath.Count - 1].neighbourTiles[i]))
        //                {
        //                    continue;
        //                }
        //                else
        //                {
        //                    if (tileFCost[nextTile] > tileFCost[truePath[truePath.Count - 1].neighbourTiles[i]])
        //                    {
        //                        nextTile = truePath[truePath.Count - 1].neighbourTiles[i];
        //                    }
        //                    else if (tileFCost[nextTile] == tileFCost[truePath[truePath.Count - 1].neighbourTiles[i]])
        //                    {
        //                        if (tileHCost[nextTile] > tileHCost[truePath[truePath.Count - 1].neighbourTiles[i]])
        //                        {
        //                            nextTile = truePath[truePath.Count - 1].neighbourTiles[i];
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    if (!truePath.Contains(nextTile))
        //    {
        //        truePath.Add(nextTile);
        //    }
        //    else
        //    {
        //        nextTile = null;
        //    }

        //    if (finishedFindingPath)
        //    {
        //        break;
        //    }
        //}

        //for(int i = 0; i < checkedTiles.Count; i++)
        //{
        //    if (truePath[i - 1].neighbourTiles.Contains(calculatedPath[i]))
        //    {
        //        if (calculatedPath[i].chair != null)
        //        {
        //            if (calculatedPath[i].chair == goal)
        //            {
        //                if (calculatedPath[i].leftTile == truePath[i - 1] || calculatedPath[i].frontTile == truePath[i - 1] || calculatedPath[i].rightTile == truePath[i - 1])
        //                {
        //                    truePath.Add(calculatedPath[i]);
        //                    break;
        //                }
        //            }
        //            else
        //            {
        //                continue;
        //            }
        //        }
        //        else
        //        {
        //            if (!truePath.Contains(calculatedPath[i]))
        //            {
        //                truePath.Add(calculatedPath[i]);
        //            }
        //        }
        //    }
        //}

        //if (truePath[truePath.Count - 1].chair != null)
        //{
        //    if(truePath[truePath.Count - 1].chair == goal)
        //    {
        //        if (truePath.Count < 2)
        //        {

        //        }
        //        else
        //        {
        //            MoveToChair();
        //        }
        //    }
        //}
        #endregion
    }

    public IEnumerator MoveToChair(int moveID)
    {
        switch (moveID)
        {
            case 0:
                transform.DOLocalMove(goalTileHit.collider.GetComponent<MapTile>().transform.localPosition + new Vector3(0, 0.5f, 0), 1f).OnUpdate(delegate
                {
                    currentLevel.passengerMoving = true;
                }).SetEase(Ease.Linear)
                    .OnStart(delegate
                    {
                        GetComponent<Animator>().SetBool("Walk", true);

                        if (transform.localPosition.x > goalTileHit.collider.GetComponent<MapTile>().transform.localPosition.x)
                        {
                            transform.DORotate(new Vector3(0, -90, 0), .1f);
                        }
                        else if (transform.localPosition.x < goalTileHit.collider.GetComponent<MapTile>().transform.localPosition.x)
                        {
                            transform.DORotate(new Vector3(0, 90, 0), .1f);
                        }

                        else if (transform.localPosition.z < goalTileHit.collider.GetComponent<MapTile>().transform.localPosition.z)
                        {
                            transform.DORotate(Vector3.zero, .1f);
                        }
                        else if (transform.localPosition.z > goalTileHit.collider.GetComponent<MapTile>().transform.localPosition.z)
                        {
                            transform.DORotate(new Vector3(0, 180, 0), .1f);
                        }

                        currentLevel.avaiableChairs.Remove(goal);
                        currentLevel.passengers.Remove(gameObject.GetComponent<PathFindingAStar>());
                        gameObject.GetComponent<WaitInLine>().waitTile.occupied = false;

                        goal.occupied = true;
                        if (goal.transform.parent.GetComponent<TwinChair>() != null)
                        {
                            goal.transform.parent.GetComponent<TwinChair>().moveAble = false;
                        }
                        else if (goal != null)
                        {
                            goal.moveAble = false;
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
                        transform.DORotate(Vector3.zero, .1f);
                        GetComponent<Animator>().SetBool("Sit", true);

                        if (goal.transform.parent.GetComponent<TwinChair>() != null)
                        {
                            goal.transform.parent.GetComponent<TwinChair>().moveAble = true;
                        }
                        else if (goal != null)
                        {
                            goal.moveAble = true;
                        }
                        gameObject.transform.parent = goal.transform;
                        gameObject.transform.localPosition = new Vector3(0, 0, 0);

                        currentLevel.passengerMoving = false;

                        currentLevel.CheckWinCondition();

                        gameObject.GetComponent<WaitInLine>().enabled = false;
                        gameObject.GetComponent<PathFindingAStar>().enabled = false;
                    }).WaitForCompletion();
                break;
            case 1:
                List<Vector3> path = new List<Vector3>();
                foreach(MapTile tile in truePath)
                {
                    path.Add(tile.transform.localPosition);
                }

                transform.DOLocalPath(path.ToArray(), 1f).SetEase(Ease.Linear).OnUpdate(delegate { currentLevel.passengerMoving = true; })
                    .OnWaypointChange(index =>
                    {
                        if (index + 1 != -1)
                        {
                            if (path[index].x > path[index + 1].x)
                            {
                                transform.DORotate(new Vector3(0, -90, 0), .1f);
                            }
                            else if (path[index].x < path[index + 1].x)
                            {
                                transform.DORotate(new Vector3(0, 90, 0), .1f);
                            }

                            else if (path[index].z < path[index + 1].z)
                            {
                                transform.DORotate(Vector3.zero, .1f);
                            }
                            else if (path[index].z > path[index + 1].z)
                            {
                                transform.DORotate(new Vector3(0, 180, 0), .1f);
                            }
                        }
                    })
                    .OnStart(delegate
                    {
                        currentLevel.avaiableChairs.Remove(goal);
                        currentLevel.passengers.Remove(gameObject.GetComponent<PathFindingAStar>());
                        gameObject.GetComponent<WaitInLine>().waitTile.occupied = false;

                        GetComponent<Animator>().SetBool("Walk", true);

                        goal.occupied = true;
                        if (goal.transform.parent.GetComponent<TwinChair>() != null)
                        {
                            goal.transform.parent.GetComponent<TwinChair>().moveAble = false;
                        }
                        else if (goal != null)
                        {
                            goal.moveAble = false;
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
                        transform.DORotate(Vector3.zero, .1f);

                        GetComponent<Animator>().SetBool("Sit", true);

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

                        currentLevel.passengerMoving = false;

                        currentLevel.CheckWinCondition();

                        gameObject.GetComponent<WaitInLine>().enabled = false;
                        gameObject.GetComponent<PathFindingAStar>().enabled = false;
                    }).WaitForCompletion();
                break;
        }

        yield return null;
    }
}
