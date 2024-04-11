using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class PathFinding : SerializedMonoBehaviour
{
    public LevelControllerNew currentLevel;
    public GameObject[,] map;
    public GameObject goal;

    public List<Vector3> path;
    public List<MapTile> finalPath = new List<MapTile>();

    int currentPos_row;
    int currentPos_col;

    public bool isAtEntrance = false;

    Queue<MapTile> BFSQueueList = new Queue<MapTile>();
    public Dictionary<MapTile, bool> visitStatus = new Dictionary<MapTile, bool>();
    public List<MapTile> traceBackPath = new List<MapTile>();

    public RaycastHit currentFloor;

    bool isSeated = false;

    void Start()
    {

        map = currentLevel.map;

        currentPos_row = 0;
        currentPos_col = map.GetLength(0) - 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            GoToEntrance();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetMap();
        }

        if (isSeated)
        {
            transform.localPosition = Vector3.zero;
        }
    }

    void GoToEntrance()
    {
        if (transform.position != map[currentPos_col, currentPos_row].transform.position)
        {
            transform.DOLocalMove(map[currentPos_col, currentPos_row].transform.localPosition + new Vector3(0, 0.5f, 0), 1f);
            isAtEntrance = true;
        }
        else
        {
            isAtEntrance = true;
        }
    }

    
    [Button]
    public void GetMap()
    {
        currentLevel.UpdateMap();

        visitStatus.Clear();
        BFSQueueList.Clear();
        traceBackPath.Clear();
        path.Clear();
        finalPath.Clear();

        FindWay();
    }

    //Breadth First Search Algorithm

    [Button]
    void FindWay()
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
            return;
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
                return;
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

        ContructWay();
    }

    [Button]
    void ContructWay()
    {
        RaycastHit floor;
        Physics.Raycast(goal.transform.position, Vector3.down, out floor, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor"));

        if(floor.collider.GetComponent<MapTile>() == currentFloor.collider.GetComponent<MapTile>())
        {
            return;
        }

        if (floor.collider.GetComponent<MapTile>() != null)
        {
            if (currentFloor.collider.GetComponent<MapTile>().neighbourTiles.Contains(floor.collider.GetComponent<MapTile>()))
            {
                if (floor.collider.GetComponent<MapTile>().rightTile == currentFloor.collider.GetComponent<MapTile>() || floor.collider.GetComponent<MapTile>().leftTile == currentFloor.collider.GetComponent<MapTile>() || floor.collider.GetComponent<MapTile>().frontTile == currentFloor.collider.GetComponent<MapTile>())
                {
                    transform.DOLocalMove(floor.collider.transform.localPosition + new Vector3(0, 0.5f, 0), 2f).SetEase(Ease.Linear).WaitForCompletion();
                    return;
                }
                else
                {
                    Debug.LogError("Path not found for " + gameObject.name);
                    return;
                }
            }

            for (int i = traceBackPath.IndexOf(floor.collider.gameObject.GetComponent<MapTile>()); i >= 0; i--)
            {
                if(finalPath.Count <= 0)
                {
                    finalPath.Add(traceBackPath[i]);
                }
                else
                {
                    //Loại bỏ phần này nếu agent được phép đi chéo
                    if (finalPath[finalPath.Count - 1].neighbourTiles.Contains(traceBackPath[i]))
                    {
                        finalPath.Add(traceBackPath[i]);
                        
                        if (traceBackPath[i].neighbourTiles.Contains(currentFloor.collider.gameObject.GetComponent<MapTile>()))
                        {
                            finalPath.Add(currentFloor.collider.gameObject.GetComponent<MapTile>());
                            break;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }             
            }

            finalPath.Reverse();
        }
        else
        {
            Debug.LogError("Not a valid goal");
            return;
        }

        if (finalPath[0].transform.localPosition == currentFloor.collider.transform.localPosition)
        {
            

            foreach (MapTile mapTile in finalPath)
            {
                path.Add(mapTile.transform.localPosition + new Vector3(0, 0.5f, 0));
            }
        }
        else
        {
            Debug.LogError("Path not found for " + gameObject.name);
            return;
        }

        if(path.Count > 1)
        {
            transform.DOLocalPath(path.ToArray(), 2f).SetEase(Ease.Linear).OnStart(delegate
            {
                currentLevel.passengers.Remove(gameObject.GetComponent<PathFinding>());
                gameObject.GetComponent<WaitInLine>().waitTile.occupied = false;

                foreach (PathFinding passenger in currentLevel.passengers)
                {
                    passenger.GetComponent<WaitInLine>().MoveToNextTile();
                }

                for (int i = 0; i < currentLevel.passengers.Count; i++)
                {
                    if (currentLevel.passengers[i] != null)
                    {
                        if (currentLevel.passengers[i].GetComponent<PathFinding>().enabled)
                        {
                            if (currentLevel.passengers[i].GetComponent<PathFinding>().isAtEntrance)
                            {
                                currentLevel.passengers[i].GetComponent<PathFinding>().GetMap();
                                break;
                            }
                        }
                    }
                }
            }).OnComplete(delegate
            {
                gameObject.transform.parent = goal.transform;
            });
        }
        
    }
}
