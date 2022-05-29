using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    [HideInInspector] public GameObject item;
    [HideInInspector] bool hasItem=false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool getHasItem()
    {
        return hasItem;
    }

    public void setItem(GameObject item)
    {
        if (getHasItem()) return;
        this.item = item;
        hasItem = true;
    }

    public void removeItem()
    {
        if (!getHasItem()) return;
        item = null;
        hasItem = false;
    }

    public GameObject getItem()
    {
        return item;
    }
}
