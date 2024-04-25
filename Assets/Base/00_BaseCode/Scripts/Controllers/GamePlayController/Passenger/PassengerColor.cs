using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerColor : MonoBehaviour
{
    [Header("----------CURRENT LEVEL----------")]
    public LevelControllerNew currentLevel;

    [Header("----------OBJECTS NEED COLOR CHANGE----------")]
    public List<GameObject> bodyPartsNeedColorChange;

    [Header("----------CHANGE COLOR----------")]
    [ValueDropdown("ListColors")]
    [OnValueChanged("ChangeMaterial")]
    public int selectedColor;

    IEnumerable ListColors()
    {
        if (currentLevel != null)
        {
            for (int i = 0; i < currentLevel.avaiableColors.Count; i++)
            {
                yield return i;
            }
        }
        else
        {
            yield return null;
        }
    }

    void ChangeMaterial()
    {
        //gameObject.GetComponent<Renderer>().material = currentLevel.avaiableColors[selectedColor];
        foreach (GameObject seatParts in bodyPartsNeedColorChange)
        {
            seatParts.GetComponent<Renderer>().material = currentLevel.avaiableColors[selectedColor];
        }

        //if(gameObject.GetComponent<Passenger>() != null)
        //{
        //    gameObject.GetComponent<Passenger>().colorID = selectedColor;
        //}
    }
}
