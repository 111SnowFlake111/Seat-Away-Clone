using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    [HideInInspector] public int colorID
    {
        get
        {
            return GetComponent<PassengerColor>().selectedColor;
        } 
    }


}
