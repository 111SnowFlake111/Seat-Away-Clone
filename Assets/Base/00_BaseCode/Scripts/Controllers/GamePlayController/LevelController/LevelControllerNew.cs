using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class LevelControllerNew : SerializedMonoBehaviour
{
    public int mapRow;
    public int mapColumn;
    public GameObject[,] map;

    public void SetMap()
    {
        map = new GameObject[mapColumn + 1, mapRow];       
    }

    public List<PathFinding> passengers;
    public List<WaitTile> sideway;

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

            foreach(PathFinding passenger in passengers)
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
            if(child.GetComponent<WaitTile>() != null)
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
            if (child.GetComponent<PathFinding>() != null)
            {
                passengers.Add(child.GetComponent<PathFinding>());
            }
        }
    }


    public List<GameObject> addedTilesOrder;

    [Button]
    public void UpdateMap()
    {
        int row = 0;
        int column = 0;

        addedTilesOrder.Clear();

        SetMap();
        foreach(Transform child in transform)
        {
            if (child.GetComponent<MapTile>() != null)
            {
                if(addedTilesOrder.Count <= 0)
                {
                    map[mapColumn, row] = child.gameObject;
                    addedTilesOrder.Add(child.gameObject);
                }
                else
                {
                    map[column, row] = child.gameObject;
                    addedTilesOrder.Add(child.gameObject);

                    if (column + 1 > mapColumn - 1)
                    {
                        column = 0;
                        row++;
                    }
                    else
                    {
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
}
