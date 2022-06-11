using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour
{
    public int itemId;
    [Range(1.0f, 60.0f)] public float time=5.0f;
    [Range(0.0f, 60.0f)] public float minAppearanceTime = 30.0f;
    public int xp;

    public int getId()
    {
        return itemId;
    }
    public float getTime()
    {
        return time;
    }
    public int getXp()
    {
        return xp;
    }

    public void copyFrom(Order other)
    {
        this.itemId = other.itemId;
        this.time = other.time;
        this.xp = other.xp;
    }

    
}
