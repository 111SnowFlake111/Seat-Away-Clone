using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
                transform.DOLocalMove(waitTile.frontTile.transform.localPosition + new Vector3(0, 0.5f, 0), 0.75f).SetEase(Ease.Linear)
                    .OnStart(delegate
                {
                    GetComponent<Animator>().SetBool("Walk", true);

                    //if (transform.localPosition.x > waitTile.frontTile.transform.localPosition.x)
                    //{
                    //    transform.DORotate(new Vector3(0, -90, 0), .1f);
                    //}
                    //else if (transform.localPosition.x < waitTile.frontTile.transform.localPosition.x)
                    //{
                    //    transform.DORotate(new Vector3(0, 90, 0), .1f);
                    //}
                    //else if (transform.localPosition.z < waitTile.frontTile.transform.localPosition.z)
                    //{
                    //    transform.DORotate(Vector3.zero, .1f);
                    //}
                    //else if (transform.localPosition.z > waitTile.frontTile.transform.localPosition.z)
                    //{
                    //    transform.DORotate(new Vector3(0, 180, 0), .1f);
                    //}
                })
                    .OnComplete(delegate
                {
                    GetComponent<Animator>().SetBool("Walk", false);

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
                transform.DOLocalMove(waitTile.entrance.transform.localPosition + new Vector3(0, 0.5f, 0), 0.75f).SetEase(Ease.Linear)
                    .OnStart(delegate
                {
                    GetComponent<Animator>().SetBool("Walk", true);

                    transform.DORotate(new Vector3(0, -90, 0), .1f);

                    //if (transform.localPosition.x > waitTile.entrance.transform.localPosition.x)
                    //{
                    //    transform.DORotate(new Vector3(0, -90, 0), .1f);
                    //}
                    //else if (transform.localPosition.x < waitTile.entrance.transform.localPosition.x)
                    //{
                    //    transform.DORotate(new Vector3(0, 90, 0), .1f);
                    //}
                    //else if (transform.localPosition.z < waitTile.entrance.transform.localPosition.z)
                    //{
                    //    transform.DORotate(Vector3.zero, .1f);
                    //}
                    //else if (transform.localPosition.z > waitTile.entrance.transform.localPosition.z)
                    //{
                    //    transform.DORotate(new Vector3(0, 180, 0), .1f);
                    //}
                })
                    .OnComplete(delegate
                {
                    GetComponent<Animator>().SetBool("Walk", false);

                    Physics.Raycast(transform.position, Vector3.down, out hit, 1f, 1 << LayerMask.NameToLayer("Floor"));

                    waitTile = hit.collider.GetComponent<WaitTile>();

                    if (waitTile.GetComponent<MapTile>().autoTriggerPathFinder)
                    {
                        gameObject.GetComponent<PathFindingAStar>().FindPath();
                        gameObject.GetComponent<PathFindingAStar>().isAtEntrance = true;
                    }

                    gameObject.GetComponent<WaitInLine>().enabled = false;
                });
            }
        }
    }
}
