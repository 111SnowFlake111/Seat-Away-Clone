using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitTile : MonoBehaviour
{
    public WaitTile frontTile;
    public WaitTile entrance;

    public bool occupied = false;

    void Start()
    {
        FindFrontTile();
        FindEntrance();

        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.up, out hit, 1f))
        {
            if(hit.collider.GetComponent<PathFindingAStar>() != null)
            {
                occupied = true;
            }
        }
    }

    [Button]
    public void FindFrontTile()
    {
        RaycastHit hit;

        if (Physics.Raycast(gameObject.transform.position, Vector3.forward, out hit, 0.5f, 1 << LayerMask.NameToLayer("Floor")))
        {
            if (hit.collider.GetComponent<WaitTile>() != null)
            {
                frontTile = hit.collider.GetComponent<WaitTile>();
            }
        }
    }

    [Button]
    public void FindEntrance()
    {
        RaycastHit hit;

        if (Physics.Raycast(gameObject.transform.position, Vector3.left, out hit, 0.5f, 1 << LayerMask.NameToLayer("Floor")))
        {
            if (hit.collider.GetComponent<MapTile>() != null)
            {
                entrance = hit.collider.GetComponent<WaitTile>();
            }
        }
    }
}
