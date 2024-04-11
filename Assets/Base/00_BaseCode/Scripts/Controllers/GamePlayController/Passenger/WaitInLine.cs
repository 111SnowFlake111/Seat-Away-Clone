using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitInLine : MonoBehaviour
{
    public LevelControllerNew currentLevel;
    public WaitTile waitTile;


    void Start()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 1f, 1 << LayerMask.NameToLayer("Floor"));
        waitTile = hit.collider.GetComponent<WaitTile>();
        MoveToNextTile();
    }

    public void MoveToNextTile()
    {
        RaycastHit hit;

        if (waitTile.frontTile != null)
        {
            if (!waitTile.frontTile.occupied)
            {
                waitTile.frontTile.occupied = true;
                waitTile.occupied = false;
                transform.DOLocalMove(waitTile.frontTile.transform.localPosition + new Vector3(0, 0.5f, 0), 1f).OnComplete(delegate
                {
                    Physics.Raycast(transform.position, Vector3.down, out hit, 1f, 1 << LayerMask.NameToLayer("Floor"));

                    waitTile = hit.collider.GetComponent<WaitTile>();
                });
            }
            
        }
        else if(waitTile.entrance != null)
        {
            if(!waitTile.entrance.occupied)
            {
                waitTile.entrance.occupied = true;
                waitTile.occupied = false;
                transform.DOLocalMove(waitTile.entrance.transform.localPosition + new Vector3(0, 0.5f, 0), 1f).OnComplete(delegate
                {
                    Physics.Raycast(transform.position, Vector3.down, out hit, 1f, 1 << LayerMask.NameToLayer("Floor"));

                    waitTile = hit.collider.GetComponent<WaitTile>();

                    gameObject.GetComponent<PathFinding>().enabled = true;
                    if (waitTile.GetComponent<MapTile>().autoTriggerPathFinder)
                    {
                        gameObject.GetComponent<PathFinding>().GetMap();
                        gameObject.GetComponent<PathFinding>().isAtEntrance = true;
                    }

                    gameObject.GetComponent<WaitInLine>().enabled = false;
                });
            }
        }
    }
}
