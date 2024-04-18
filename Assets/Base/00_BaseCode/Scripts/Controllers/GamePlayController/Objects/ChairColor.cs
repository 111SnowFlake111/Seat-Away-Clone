using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairColor : MonoBehaviour
{
    [Header("----------CURRENT LEVEL----------")]
    public LevelControllerNew currentLevel;

    [Header("----------OBJECTS NEED COLOR CHANGE----------")]
    public List<GameObject> seatPartsNeedColorChange;

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
        GetComponent<Renderer>().material = currentLevel.avaiableColors[selectedColor];
        foreach(GameObject seatParts in seatPartsNeedColorChange)
        {
            seatParts.GetComponent<Renderer>().material = currentLevel.avaiableColors[selectedColor];
        }

        //if(gameObject.GetComponent<TwinChair>() != null)
        //{
        //    foreach(Chair chair in gameObject.GetComponent<TwinChair>().ChairList)
        //    {
        //        chair.colorID = selectedColor;
        //    }
        //}
        //else if(gameObject.GetComponent<Chair>() != null)
        //{
        //    gameObject.GetComponent<Chair>().colorID = selectedColor;
        //}
    }
}
