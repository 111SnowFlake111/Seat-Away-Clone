using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public GameObject chair;
    public GameObject floor;

    [NonSerialized] Vector3 chairOgPos;

    Vector3 point;
    Vector3 mouseMovementPos;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 ogPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit hit;
            RaycastHit floorHit;

            if(Physics.Raycast(ogPos, Vector3.forward, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Chair")))
            {
                if (Physics.Raycast(ogPos, Vector3.forward, out floorHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor")))
                {
                    if (floorHit.collider.gameObject.GetComponent<MapTile>().chair != null)
                    {
                        floorHit.collider.gameObject.GetComponent<MapTile>().chair = null;
                    }
                }

                if (hit.collider.gameObject.GetComponent<Chair>() != null)
                {
                    chairOgPos = hit.collider.gameObject.transform.position;
                    point = hit.point;
                    chair = hit.collider.gameObject;
                }
            }

            if (Physics.Raycast(ogPos, Vector3.forward, out floorHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor")))
            {
                if (floorHit.collider.gameObject.GetComponent<MapTile>().chair != null)
                {
                    floorHit.collider.gameObject.GetComponent<MapTile>().chair = null;
                }
            }
            
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 ogPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseMovementPos = ogPos;
            RaycastHit hit;

            if (Physics.Raycast(ogPos, Vector3.forward, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor")))
            {
                if (hit.collider.gameObject.GetComponent<MapTile>() != null && hit.collider.gameObject.GetComponent<MapTile>().chair == null)
                {
                    floor = hit.collider.gameObject;
                }
                else
                {
                    floor = null;
                }
            }
            else
            {
                floor = null;
            }

            if (chair != null)
            {
                point = new Vector3(mouseMovementPos.x, mouseMovementPos.y, point.z);
                chair.transform.position = point;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (chair != null)
            {
                if (floor != null)
                {
                    chair.transform.parent = floor.transform;
                    chair.transform.localPosition = new Vector3(0, 0, -1);
                    floor.GetComponent<MapTile>().chair = chair;

                    chair = null;
                    floor = null;
                }
                else
                {
                    chair.transform.position = chairOgPos;
                    chair = null;
                }
            }
        }
    }
}
