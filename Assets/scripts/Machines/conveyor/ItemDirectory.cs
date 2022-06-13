using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDirectory : MonoBehaviour
{
    static public ItemDirectory instance;
    public List<GameObject> itemDirectory;

    void Awake()
    {
        instance = this;
    }

    public GameObject getItemWithId(int i)
    {
        if (itemDirectory[i])
        {
            
            GameObject toReturn = Instantiate(itemDirectory[i], new Vector3(0,0,0), Quaternion.identity);
            return toReturn;
        }
        return null;
    }
    public Sprite getIdSprite(int i)
    {
        return itemDirectory[i].GetComponent<ItemData>().image;
    }
    
    public string getObjectName(int i)
    {
        return itemDirectory[i].GetComponent<ItemData>().itemName;
    }
}
