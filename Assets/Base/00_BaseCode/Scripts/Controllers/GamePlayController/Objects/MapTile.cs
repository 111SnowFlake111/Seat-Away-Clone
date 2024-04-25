using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MapTile : MonoBehaviour
{
    public LevelControllerNew currentLevel;

    public GameObject chairSpawn;
    public GameObject twinChairSpawn;

    public GameObject chair;

    public List<MapTile> neighbourTiles;
    public MapTile leftTile;
    public MapTile rightTile;
    public MapTile frontTile;

    public float G;
    public float H;
    public float F => G + H;

    [NonSerialized] public MapTile linkedTile;

    public bool autoTriggerPathFinder = false;

    [NonSerialized] public GameObject chairInstance;
    public int colorID;
    
    void Start()
    {
        neighbourTiles.Clear();
        leftTile = null;
        rightTile = null;
        frontTile = null;

        FindNeighbour(Vector3.forward);
        FindNeighbour(Vector3.right);
        FindNeighbour(Vector3.back);
        FindNeighbour(Vector3.left);
        
    }

    void FindNeighbour(Vector3 direction)
    {
        RaycastHit hit;

        if(Physics.Raycast(gameObject.transform.position, direction, out hit, 10f, 1 << LayerMask.NameToLayer("Floor")))
        {
            if (hit.collider.GetComponent<MapTile>() != null)
            {
                if(direction == Vector3.forward)
                {
                    frontTile = hit.collider.GetComponent<MapTile>();
                }

                if(direction == Vector3.left)
                {
                    leftTile = hit.collider.GetComponent<MapTile>();
                }

                if(direction == Vector3.right)
                {
                    rightTile = hit.collider.GetComponent<MapTile>();
                }

                neighbourTiles.Add(hit.collider.GetComponent<MapTile>());
            }     
        }
    }

    public float GetDistance(MapTile other)
    {
        var dist = new Vector2Int(Mathf.Abs((int)transform.localPosition.x - (int)other.transform.localPosition.x), Mathf.Abs((int)transform.localPosition.y - (int)other.transform.localPosition.y));

        var lowest = Mathf.Min(dist.x, dist.y);
        var highest = Mathf.Max(dist.x, dist.y);

        var horizontalMovesRequired = highest - lowest;

        return lowest * 14 + horizontalMovesRequired * 10;
    }

    [Button]
    public void UpdateNeighbours()
    {
        neighbourTiles.Clear();
        leftTile = null;
        rightTile = null;
        frontTile = null;

        FindNeighbour(Vector3.forward);
        FindNeighbour(Vector3.back);
        FindNeighbour(Vector3.left);
        FindNeighbour(Vector3.right);
    }

    [Button]
    void SpawnChair()
    {
        if (chair != null)
        {
            Debug.LogError("There's already a chair here");
        }
        else
        {
            chairInstance = Instantiate(chairSpawn, transform.position, Quaternion.identity, this.transform);
            chairInstance.transform.localPosition = new Vector3(0, 0.5f, 0);
            chairInstance.GetComponent<ChairColor>().currentLevel = currentLevel;

            chair = chairInstance;


            Debug.LogError("Chair spawned");
        }
    }

    [Button]
    void SpawnTwinChair()
    {
        if (chair != null)
        {
            Debug.LogError("There's already a chair here");
        }
        else
        {
            if(rightTile != null)
            {
                chairInstance = Instantiate(twinChairSpawn, transform.position, Quaternion.identity, transform);
                chairInstance.transform.localPosition = new Vector3(0.5f, 0.5f, 0);
                chairInstance.GetComponent<ChairColor>().currentLevel = currentLevel;

                chair = chairInstance.GetComponent<TwinChair>().ChairList[0].gameObject;
                rightTile.GetComponent<MapTile>().chair = chairInstance.GetComponent<TwinChair>().ChairList[1].gameObject;

                Debug.LogError("Twinchair spawned");
            }
            else
            {
                Debug.LogError("Need an additional tile to the right to spawn a Twinchair here");
            }
        }
    }

    [Button]
    void DeleteChair()
    {
        if(chair != null)
        {
            if(chair.transform.parent.GetComponent<TwinChair>() != null)
            {
                chair = null;
                rightTile.GetComponent<MapTile>().chair = null;
                DestroyImmediate(chairInstance);

                Debug.LogError("Twinchair destroyed");
            }
            else
            {
                chair = null;
                DestroyImmediate(chairInstance);

                Debug.LogError("Chair destroyed");
            }

            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }
        else
        {
            Debug.LogError("There's no chair to destroy");
        }
    }
}
