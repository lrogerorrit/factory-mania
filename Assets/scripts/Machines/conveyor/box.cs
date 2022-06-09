using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour
{

    public int itemId;
    [SerializeField] private ItemDirectory itemDirectory;


    public GameObject generateItem()
    {
        return itemDirectory.getItemWithId(itemId);
        //TODO: Add it to player controller
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
