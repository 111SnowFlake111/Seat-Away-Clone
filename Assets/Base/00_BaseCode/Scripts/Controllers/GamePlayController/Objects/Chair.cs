using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public bool moveAble = true;
    public bool occupied = false;

    [HideInInspector]
    public int colorID
    {
        get
        {
            if(transform.parent.GetComponent<TwinChair>() != null)
            {
                return transform.parent.GetComponent<ChairColor>().selectedColor;
            }
            else
            {
                return GetComponent<ChairColor>().selectedColor;
            }
        }
    }
}
