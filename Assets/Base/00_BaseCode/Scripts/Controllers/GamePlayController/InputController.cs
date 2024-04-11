using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public LevelControllerNew currentLevel;

    public GameObject chair;
    public GameObject floor;
    public GameObject floor2;

    [NonSerialized] Vector3 chairOgPos;

    Vector3 ogMousePos;
    Vector3 newMousePos;

    RaycastHit hit;
    RaycastHit floorHit;
    RaycastHit floorHit2;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ogMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);            

            if(Physics.Raycast(ogMousePos, Vector3.down, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Chair")))
            {
                if (hit.collider.gameObject.GetComponent<Chair>() != null)
                {
                    if (hit.collider.gameObject.GetComponent<Chair>().moveAble)
                    {
                        if (Physics.Raycast(ogMousePos, Vector3.down, out floorHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor")))
                        {
                            if (floorHit.collider.gameObject.GetComponent<MapTile>().chair != null)
                            {
                                floorHit.collider.gameObject.GetComponent<MapTile>().chair = null;
                            }
                        }

                        chairOgPos = hit.collider.gameObject.transform.position;
                        chair = hit.collider.gameObject;
                    }
                }

                if (hit.collider.gameObject.GetComponent<TwinChair>() != null)
                {
                    if (hit.collider.gameObject.GetComponent<TwinChair>().moveAble)
                    {
                        foreach (Chair chair in hit.collider.gameObject.GetComponent<TwinChair>().ChairList)
                        {
                            chair.GetComponent<BoxCollider>().enabled = false;
                        }

                        if (Physics.Raycast(hit.collider.gameObject.GetComponent<TwinChair>().ChairList[0].transform.position, Vector3.down, out floorHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor")))
                        {
                            if (floorHit.collider.gameObject.GetComponent<MapTile>().chair != null)
                            {
                                floorHit.collider.gameObject.GetComponent<MapTile>().chair = null;
                            }
                        }

                        if (Physics.Raycast(hit.collider.gameObject.GetComponent<TwinChair>().ChairList[1].transform.position, Vector3.down, out floorHit2, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor")))
                        {
                            if (floorHit2.collider.gameObject.GetComponent<MapTile>().chair != null)
                            {
                                floorHit2.collider.gameObject.GetComponent<MapTile>().chair = null;
                            }
                        }

                        chairOgPos = hit.collider.gameObject.transform.position;
                        chair = hit.collider.gameObject;
                    }
                }
            }           
        }

        if (Input.GetMouseButton(0))
        {
            newMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit floor;
            RaycastHit floor2;

            if (chair != null)
            {
                

                if (chair.GetComponent<Chair>() != null)
                {
                    chair.GetComponent<Rigidbody>().isKinematic = false;

                    if (Physics.Raycast(chair.transform.position, Vector3.down, out floor, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor")))
                    {
                        if (floor.collider.gameObject.GetComponent<MapTile>() != null && floor.collider.gameObject.GetComponent<MapTile>().chair == null)
                        {
                            this.floor = floor.collider.gameObject;
                        }
                        else
                        {
                            this.floor = null;
                        }
                    }
                    else
                    {
                        this.floor = null;
                    }

                    chair.GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude((newMousePos - chair.transform.position).normalized * 500f, 500f);
                }

                if (chair.GetComponent<TwinChair>() != null)
                {                   
                    chair.GetComponent<Rigidbody>().isKinematic = false;

                    if (Physics.Raycast(chair.GetComponent<TwinChair>().ChairList[0].transform.position, Vector3.down, out floor, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor")))
                    {
                        if (floor.collider.gameObject.GetComponent<MapTile>() != null && floor.collider.gameObject.GetComponent<MapTile>().chair == null)
                        {
                            this.floor = floor.collider.gameObject;
                        }
                        else
                        {
                            this.floor = null;
                        }
                    }
                    else
                    {
                        this.floor = null;
                    }

                    if (Physics.Raycast(chair.GetComponent<TwinChair>().ChairList[1].transform.position, Vector3.down, out floor2, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor")))
                    {
                        if (floor2.collider.gameObject.GetComponent<MapTile>() != null && floor2.collider.gameObject.GetComponent<MapTile>().chair == null)
                        {
                            this.floor2 = floor2.collider.gameObject;
                        }
                        else
                        {
                            this.floor2 = null;
                        }
                    }
                    else
                    {
                        this.floor2 = null;
                    }

                    chair.GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude((newMousePos - chair.transform.position).normalized * 500f, 500f);

                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (chair != null)
            {
                chair.GetComponent<Rigidbody>().velocity = Vector3.zero;

                if (chair.GetComponent<TwinChair>() != null)
                {
                    if (floor != null && floor2 != null)
                    {
                        chair.transform.parent = floor.transform;
                        chair.transform.localPosition = new Vector3(0.5f, 0.5f, 0);
                        floor.GetComponent<MapTile>().chair = chair.GetComponent<TwinChair>().ChairList[0].gameObject;
                        floor2.GetComponent<MapTile>().chair = chair.GetComponent<TwinChair>().ChairList[1].gameObject;
                    }
                    else
                    {
                        chair.transform.position = chairOgPos;
                        floorHit.collider.gameObject.GetComponent<MapTile>().chair = chair.GetComponent<TwinChair>().ChairList[0].gameObject;
                        floorHit2.collider.gameObject.GetComponent<MapTile>().chair = chair.GetComponent<TwinChair>().ChairList[1].gameObject;
                    }

                    foreach (Chair chair in chair.GetComponent<TwinChair>().ChairList)
                    {
                        chair.GetComponent<BoxCollider>().enabled = true;
                    }
                }
                else if (chair.GetComponent<Chair>() != null)
                {
                    if (floor != null)
                    {
                        chair.transform.parent = floor.transform;
                        chair.transform.localPosition = new Vector3(0, 0.5f, 0);
                        floor.GetComponent<MapTile>().chair = chair;
                    }
                    else
                    {                       
                        chair.transform.position = chairOgPos;
                        floorHit.collider.gameObject.GetComponent<MapTile>().chair = chair;
                    }                    
                }

                //foreach (PathFinding passenger in currentLevel.passengers)
                //{
                //    passenger.GetComponent<WaitInLine>().MoveToNextTile();
                //}

                for (int i = 0; i < currentLevel.passengers.Count; i++)
                {
                    if(currentLevel.passengers[i] != null)
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

                chair.GetComponent<Rigidbody>().isKinematic = true;
                chair = null;
                floor = null;
                floor2 = null;
            }
            
        } 
    }
}
