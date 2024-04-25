using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    public Animator animator;
    [HideInInspector] public int colorID
    {
        get
        {
            return GetComponent<PassengerColor>().selectedColor;
        } 
    }


}
