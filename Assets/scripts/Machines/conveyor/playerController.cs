using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerController : MonoBehaviour
{
    
    [SerializeField] private GameObject itemPosObj;
    [SerializeField] private ItemDirectory itemDirectory;
    [HideInInspector] public GameObject item;
    [HideInInspector] public bool hasItem=false;
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
        item.transform.parent = itemPosObj.transform;
        item.transform.localPosition = new Vector3(0,0,0);
        hasItem = true;
    }

    public void removeItem(bool withDestroy=true)
    {
        if (!getHasItem()) return;
        if (withDestroy)
        {
            Debug.Log("Hulk Smash, Hulk Destroy");
            Destroy(item);
        }
        item = null;
        hasItem = false;
    }

    public GameObject getItem()
    {
        return item;
    }

    private void conveyorTrigger(GameObject other, bool isEntrance)
    {
        conveyor conv = other.transform.parent.GetComponent<conveyor>();
        if (!conv) return;
        Debug.Log("hit conveyor");
        if (isEntrance) { 
            if (this.item)
            {
                Debug.Log("Trying to insert item");
                if (conv.addItemToConveyor(this.item))
                {
                    Debug.Log("Transfer successfull item");
                    this.removeItem(false);
                }
            }
        }
        else
        {
            Debug.Log("Trying to remove item");
           if( conv.removeItemFromConveyor(0, itemPosObj))
            {
                Debug.Log("Transfer successfull item");
                item = itemPosObj.transform.GetChild(0).gameObject;
            }
        }
        
    }
    
    private void boxTrigger(GameObject other)
    {
        Debug.Log("hit box");
        if (this.item) return;
        box boxComponent= other.GetComponent<box>();
        GameObject item = boxComponent.generateItem();
        this.setItem(item);
    }

    private void transformerTrigger(GameObject other)
    {
        Debug.Log("transformer hit");
        transformer transf = other.GetComponent<transformer>();
        if (item)
        {
            Debug.Log("Trying to insert item");
            if (transf.canInsertItem)
            {
                if (transf.insertIntoMachine(this.item))
                    this.removeItem();
                 
            }
        }
        else
        {
            Debug.Log("Trying to remove item");
            int itemId = transf.removeItem();
            if (itemId == -1)
            {
                Debug.Log("No item to remove");
                return;
            }
            GameObject item = itemDirectory.getItemWithId(itemId);
            this.setItem(item);

        }
    }

    

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "conveyorEntrance":
            case "conveyorExit":
                conveyorTrigger(other.gameObject, other.tag == "conveyorEntrance");
                break;
            case "box":
                boxTrigger(other.gameObject);
                break;
            case "transformer":
                transformerTrigger(other.gameObject);
                break;
        }
        
    }
}
