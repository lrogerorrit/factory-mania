using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum recepieType
{
    TRANSFORM,
    COMBINE,
    TINT
}


public class Recepie : MonoBehaviour
{

    public recepieType type;
    public int item1;
    public int item2;
    public float time;
    public int outputId;


    public bool isRecepieForItems(List<int> items)
    {
        if ((type==recepieType.COMBINE) && items.Count == 2)
        {
            return ((items[0] == item1 && items[1] == item2) || (items[0] == item2 && items[1] == item1));
        }

        if ((type == recepieType.TRANSFORM) && items.Count == 1)
        {
            return (items[0] == item1);
        }

        return false;
    }
    
}

