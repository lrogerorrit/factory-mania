using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combiner : MonoBehaviour
{
    public MachineConfig configuration;

    // Start is called before the first frame update
    public ItemData insertedObjectA;
    public ItemData insertedObjectB;
    [SerializeField] private Image progressBar;
    [SerializeField] private Image iconA;
    [SerializeField] private Image iconB;
    public int insertedObjects = 0;
    public bool isTransformed = false;
    public float timeInMachine = 0.0f;
    public bool canInsertItemA = true;
    public bool canInsertItemB = true;
    public bool canTransform = false;

    private Recepie activeRecepie;

   

    bool insertItem(GameObject item, bool canInsert, bool isA=true)
    {
        if (!canInsert) return false;
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
                if (isA)
                {
                    insertedObjectA = itemData;
                    if (insertedObjectB) itemList.Add(insertedObjectB.itemType);
                }
                else
                {
                    insertedObjectB = itemData;
                    if (insertedObjectA) itemList.Add(insertedObjectA.itemType);
                }
                Recepie recepie = configuration.getRecepieForItems(itemList);
                if (recepie)
                {
                    Debug.Log("Found recepie");
                    canTransform = true;
                    activeRecepie = recepie;
                }

                this.insertedObjects += 1;
                    return true;
            }
            else
                Debug.Log("rejected id");
        }
        return false;
    }

    public bool insertIntoMachine(GameObject item, bool sideA = true)
    {
        return insertItem(item, sideA ? canInsertItemA : canInsertItemB, sideA);
    }

    public bool canRemoveItem()
    {
        return insertedObjects==2 && isTransformed;
    }

    public int removeItem(bool isA=true) //TODO Make it so you can remove items at any time
    {
        int idToReturn;
        if (canTransform && !isTransformed) return -1;
        if (insertedObjects == 2 && isTransformed) {

            insertedObjects = 0;
            isTransformed = false;
            idToReturn = (int)this.activeRecepie.outputId;
            Destroy(this.insertedObjectA);
            Destroy(this.insertedObjectB);
            canInsertItemA = true;
            canInsertItemB = true;
            canTransform = false;

            updateTopTimer();
            activeRecepie = null;
        }
        else
        {
            this.insertedObjects -= 1;
            this.insertedObjects = Mathf.Min(Mathf.Max(this.insertedObjects, 0), 2);
            canTransform = false;

            if (isA)
            {
                idToReturn = (int)this.insertedObjectA.itemType;
                canInsertItemA = true;
                this.insertedObjectA = null;
            }
            else
            {
                idToReturn = (int)this.insertedObjectB.itemType;
                canInsertItemB = true;
                this.insertedObjectB = null;

            }
            activeRecepie = null;
        }
        return idToReturn;
    }

    void updateTopTimer()
    {
        float percentage = (isTransformed) ? 1.0f : timeInMachine / activeRecepie.time;
        progressBar.fillAmount = percentage;
    }

    void updateIcons()
    {
        if (this.insertedObjectA)
        {
            this.iconA.sprite = this.insertedObjectA.image;
            this.iconA.enabled = true;
        }
        else
            this.iconA.enabled = false;

        if (this.insertedObjectB)
        {
            this.iconB.sprite = this.insertedObjectB.image;
            this.iconB.enabled = true;
        }
        else
            this.iconB.enabled = false;
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
    
    // Start is called before the first frame update
    void Start()
    {
        if (!progressBar) return;
        progressBar.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (insertedObjects==2 && !isTransformed && canTransform)
        {
            updateItem();
            updateTopTimer();
        }
        updateIcons();
    }
}
