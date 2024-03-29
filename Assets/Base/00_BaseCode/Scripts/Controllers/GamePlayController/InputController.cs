using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public GameObject chair;
    public GameObject floor;
    public GameObject floor2;

    [NonSerialized] Vector3 chairOgPos;

    Vector3 point;
    Vector3 mouseMovementPos;

    RaycastHit hit;
    RaycastHit floorHit;
    RaycastHit floorHit2;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 ogPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);            

            if(Physics.Raycast(ogPos, Vector3.forward, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Chair")))
            {
                if (hit.collider.gameObject.GetComponent<Chair>() != null)
                {
                    if (Physics.Raycast(ogPos, Vector3.forward, out floorHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor")))
                    {
                        if (floorHit.collider.gameObject.GetComponent<MapTile>().chair != null)
                        {
                            floorHit.collider.gameObject.GetComponent<MapTile>().chair = null;
                        }
                    }

                    chairOgPos = hit.collider.gameObject.transform.position;
                    point = hit.point;
                    chair = hit.collider.gameObject;
                }

                if (hit.collider.gameObject.GetComponent<TwinChair>() != null)
                {
                    if (Physics.Raycast(hit.collider.gameObject.GetComponent<TwinChair>().ChairList[0].transform.position, Vector3.forward, out floorHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor")))
                    {
                        if (floorHit.collider.gameObject.GetComponent<MapTile>().chair != null)
                        {
                            floorHit.collider.gameObject.GetComponent<MapTile>().chair = null;
                        }
                    }

                    if (Physics.Raycast(hit.collider.gameObject.GetComponent<TwinChair>().ChairList[1].transform.position, Vector3.forward, out floorHit2, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor")))
                    {
                        if (floorHit2.collider.gameObject.GetComponent<MapTile>().chair != null)
                        {
                            floorHit2.collider.gameObject.GetComponent<MapTile>().chair = null;
                        }
                    }

                    chairOgPos = hit.collider.gameObject.transform.position;
                    point = hit.point;
                    chair = hit.collider.gameObject;
                }
            }

            //if (Physics.Raycast(ogPos, Vector3.forward, out floorHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor")))
            //{
            //    if (floorHit.collider.gameObject.GetComponent<MapTile>().chair != null)
            //    {
            //        floorHit.collider.gameObject.GetComponent<MapTile>().chair = null;
            //    }
            //}
            
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 ogPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseMovementPos = ogPos;
            RaycastHit floor;
            RaycastHit floor2;

            if (chair != null)
            {
                if (chair.GetComponent<Chair>() != null)
                {
                    if (Physics.Raycast(ogPos, Vector3.forward, out floor, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor")))
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
                }

                if (chair.GetComponent<TwinChair>() != null)
                {
                    if (Physics.Raycast(chair.GetComponent<TwinChair>().ChairList[0].transform.position, Vector3.forward, out floor, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor")))
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

                    if (Physics.Raycast(chair.GetComponent<TwinChair>().ChairList[1].transform.position, Vector3.forward, out floor2, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor")))
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
                }

                point = new Vector3(mouseMovementPos.x, mouseMovementPos.y, point.z);
                chair.transform.position = point;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (chair != null)
            {
                if (chair.GetComponent<TwinChair>() != null)
                {
                    if (floor != null && floor2 != null)
                    {
                        chair.transform.parent = floor.transform;
                        chair.transform.localPosition = new Vector3(0.5f, 0, -0.5f);
                        floor.GetComponent<MapTile>().chair = chair.GetComponent<TwinChair>().ChairList[0].gameObject;
                        floor2.GetComponent<MapTile>().chair = chair.GetComponent<TwinChair>().ChairList[1].gameObject;
                    }
                    else
                    {
                        chair.transform.position = chairOgPos;
                        floorHit.collider.gameObject.GetComponent<MapTile>().chair = chair.GetComponent<TwinChair>().ChairList[0].gameObject;
                        floorHit2.collider.gameObject.GetComponent<MapTile>().chair = chair.GetComponent<TwinChair>().ChairList[1].gameObject;
                        chair = null;
                    }
                }
                else if (chair.GetComponent<Chair>() != null)
                {
                    if (floor != null)
                    {
                        chair.transform.parent = floor.transform;
                        chair.transform.localPosition = new Vector3(0, 0, -1);
                        floor.GetComponent<MapTile>().chair = chair;
                    }
                    else
                    {
                        chair.transform.position = chairOgPos;
                        floorHit.collider.gameObject.GetComponent<MapTile>().chair = chair;
                        chair = null;
                    }
                }

                chair = null;
                floor = null;
                floor2 = null;
            }
            
        } 
    }
}
