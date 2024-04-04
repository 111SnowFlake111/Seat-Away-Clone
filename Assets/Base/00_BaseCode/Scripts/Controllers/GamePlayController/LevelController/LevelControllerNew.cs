using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class LevelControllerNew : SerializedMonoBehaviour
{
    public int mapRow;
    public int mapColumn;
    public GameObject[,] map;

    private void Awake()
    {
     
    }
    [Button] 
    public void SetMatrix()
    {
        map = new GameObject[mapRow, mapColumn];
    }
}
