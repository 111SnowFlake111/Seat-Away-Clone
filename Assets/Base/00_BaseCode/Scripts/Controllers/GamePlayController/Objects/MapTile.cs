using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public GameObject chair;

    public List<MapTile> neighbourTiles;
    public MapTile leftTile;
    public MapTile rightTile;
    public MapTile frontTile;

    public bool autoTriggerPathFinder = false;
    
    void Start()
    {
        FindNeighbour(Vector3.forward);
        FindNeighbour(Vector3.back);
        FindNeighbour(Vector3.left);
        FindNeighbour(Vector3.right);
    }

    void FindNeighbour(Vector3 direction)
    {
        RaycastHit hit;

        if(Physics.Raycast(gameObject.transform.position, direction, out hit, 0.5f, 1 << LayerMask.NameToLayer("Floor")))
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

    [Button]
    void UpdateNeighbours()
    {
        neighbourTiles.Clear();

        FindNeighbour(Vector3.forward);
        FindNeighbour(Vector3.back);
        FindNeighbour(Vector3.left);
        FindNeighbour(Vector3.right);
    }
}
