using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class transformer : MonoBehaviour
{

    public MachineConfig configuration;

    // Start is called before the first frame update
    public ItemData insertedObject;
    [SerializeField] private Image progressBar;
    public bool isInserted = false;
    public bool isTransformed = false;
    public float timeInMachine = 0.0f;
    public bool canInsertItem = true;

    
    private Recepie activeRecepie;


    public bool insertIntoMachine(GameObject item)
    {
        if (!canInsertItem) return false;
        ItemData itemDataTemp = item.GetComponent<ItemData>();
        if (itemDataTemp)
        {
            
            if (configuration.acceptedItemIds.Contains(itemDataTemp.itemType))
            {
                ItemData itemData = gameObject.AddComponent<ItemData>() as ItemData;
                itemData.itemType = itemDataTemp.itemType;
                itemData.itemName = itemDataTemp.itemName;
                
                Debug.Log("Accepted id");
                List<int> itemList = new List<int>();
                itemList.Add(itemData.itemType);
                Recepie recepie = configuration.getRecepieForItems(itemList);
                if (recepie)
                {
                    Debug.Log("Found recepie");
                    insertedObject = itemData;
                    isInserted = true;
                    activeRecepie = recepie;
                    canInsertItem = false;
                    isTransformed = false;
                    return true;
                    

                }
            }
            else
                Debug.Log("rejected id");
        }
        return false;
    }

    public bool canRemoveItem()
    {
        return isInserted && isTransformed;
    }

    public int removeItem()
    {
        if (!canRemoveItem()) return -1;
        isInserted = false;
        isTransformed = false;
        timeInMachine = 0.0f;
        int idToReturn = (int) this.activeRecepie.outputId;
        Destroy(this.insertedObject);
        
        this.insertedObject = null;
        canInsertItem = true;
        updateTopTimer();
        activeRecepie = null;
        
        return idToReturn;
        
    }
    
  

    void updateTopTimer()
    {
        float percentage = (isTransformed)?1.0f: timeInMachine / activeRecepie.time;
        progressBar.fillAmount = percentage;
    }

    void updateItem()
    {
        timeInMachine += Time.deltaTime;
        if (timeInMachine >= activeRecepie.time)
        {
            isTransformed = true;
            timeInMachine = 0.0f;
        }
    }

    void Start()
    {
        if (!progressBar) return;
        progressBar.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInserted && !isTransformed)
        {
            updateItem();
            updateTopTimer();
        }

    }
}
