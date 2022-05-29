using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class transformer : MonoBehaviour
{

    public MachineConfig configuration;

    // Start is called before the first frame update
    [HideInInspector] public ItemData insertedObject;
    [HideInInspector] public bool isInserted = false;
    [HideInInspector] public bool isTransformed = false;
    [HideInInspector] public float timeInMachine = 0.0f;

    private Recepie activeRecepie;


    void insertIntoMachine(GameObject item)
    {
        ItemData itemData = item.GetComponent<ItemData>();

        if (configuration.acceptedItemIds.Contains(itemData.itemType))
        {
            List<int> itemList = new List<int>();
            itemList.Add(itemData.itemType);
            Recepie recepie = configuration.getRecepieForItems(itemList);
            if (recepie != null)
            {
                insertedObject = itemData;
                isInserted = true;
                activeRecepie = recepie;
                //TODO: Remove item from player controller

            }
        }
    }

    bool canRemoveItem()
    {
        return isInserted && isTransformed;
    }

    void removeItem()
    {
        if (!canRemoveItem()) return;
        isInserted = false;
        isTransformed = false;
        timeInMachine = 0.0f;
        //TODO:
    }

    void updateTopTimer()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
