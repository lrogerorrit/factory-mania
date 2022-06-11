using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum conveyorItemRemoveReason
{
    pickUp,
    continueLine,
    deliver
};


public class conveyor : MonoBehaviour
{


    public float movingSpeed=0.1f;
    public float startPoint = -.45f;
    public float endPoint = .45f;

   

    public bool isOccupied = false;
    public bool acceptsObjects = true;
    public bool isDeliver = false;
    public bool canPickUp = false;
    public bool waitingForNext = false;

    private float length;

    [HideInInspector] public GameObject itemInConveyor;

    private GameObject child;
    private Renderer childRenderer;
    //private Renderer selfRenderer;

    private LevelHandler levelHandler;

    private Vector3 objSize;

   [SerializeField]private conveyor nextConveyorComponent;
   [SerializeField] private conveyor prevConveyorComponent;

    private OrderHandler orderHandler;
    
    
    // Start is called before the first frame update  
    void Start()
    {
        child = gameObject.transform.Find("base").gameObject;
        childRenderer = child.GetComponent<Renderer>();
        //selfRenderer = GetComponent<Renderer>();
        levelHandler = LevelHandler.instance;
        orderHandler = OrderHandler.instance;
        objSize = transform.localScale;
        length = objSize.x;
        if(isDeliver)
            childRenderer.material.color= Color.green;
    }
    

    public bool addItemToConveyor(GameObject item, bool fromLine = false)
    {
        if (itemInConveyor != null || isOccupied || (!fromLine && !acceptsObjects)) return false;
        this.isOccupied = true;
        itemInConveyor = item;
        item.transform.parent = gameObject.transform;
        item.transform.localPosition = new Vector3(startPoint, 0, 0);
        return true;

    }
   
   private bool deliverItem()
    {
        Debug.Log("hu");
        if (!orderHandler) return false;
        Debug.Log("hiu");
        ItemData itemData = itemInConveyor.GetComponent<ItemData>();
        if (itemData)
        {
            bool toReturn= orderHandler.handInItem(itemData.itemType);

            Destroy(itemInConveyor);
            return toReturn;
            
        }
        return false;
    }
    
    private bool pickUp(GameObject plObj)
    {
        if (!canPickUp || !itemInConveyor) return false;
        this.canPickUp = false;

        itemInConveyor.transform.parent = plObj.transform;
        itemInConveyor.transform.localPosition = new Vector3(0, 0, 0);
        itemInConveyor = null;
        isOccupied = false;
        return true;
    }
    
    private bool continueLine()
    {
        if (this.nextConveyorComponent)
        {
            
            
            itemInConveyor = null;
            isOccupied = false;
            canPickUp = false;

            return true;
            
        }
        return false;
    }
    
    public bool removeItemFromConveyor(conveyorItemRemoveReason reason, GameObject playerObj)
    {
        switch (reason)
        {
            case conveyorItemRemoveReason.pickUp:
                return pickUp(playerObj);
                break;
            case conveyorItemRemoveReason.continueLine:
                return continueLine();
                break;
            case conveyorItemRemoveReason.deliver:
               return deliverItem();
                break;

        }
        return false;
        
    }

    public bool removeItemFromConveyor(int reason, GameObject playerObj)
    {
        return removeItemFromConveyor((conveyorItemRemoveReason)reason,playerObj);
    }
    
    private void updateItemPosition()
    {
        canPickUp = false;
        if (itemInConveyor == null) return;
        if (itemInConveyor.transform.localPosition.x >= endPoint)
        {
            if (nextConveyorComponent)
            {
                if (nextConveyorComponent.isOccupied) {
                    waitingForNext = true;
                    return;
                }
                else
                {
                    waitingForNext = false;
                    if (itemInConveyor.transform.localPosition.x >= .455 )
                    {
                        
                        if (nextConveyorComponent.addItemToConveyor(this.itemInConveyor, true))
                            removeItemFromConveyor(conveyorItemRemoveReason.continueLine, null);
                        else
                            waitingForNext = true;
                        
                        return;
                            
                    }
                        
                }
            }
            else {
                if (isDeliver)
                {
                    deliverItem();
                    return;
                }
                canPickUp = true;
                return;
            };
        }
        itemInConveyor.transform.localPosition = new Vector3(itemInConveyor.transform.localPosition.x + movingSpeed* Time.deltaTime, 0, 0);
        if (itemInConveyor.transform.localPosition.x >= endPoint && !nextConveyorComponent)
        {
            if (!isDeliver)
            {
                canPickUp = true;
            }
            else
                if (itemInConveyor.transform.localPosition.x >= .5)
                    canPickUp = true;
            
        }

    }

        // Update is called once per frame
    void Update()
    {
        if (!itemInConveyor || (itemInConveyor && (!canPickUp&&!waitingForNext)))
        {
            childRenderer.material.mainTextureOffset += new Vector2(movingSpeed * Time.deltaTime, 0);
            if (childRenderer.material.mainTextureOffset.x > 100)
                childRenderer.material.mainTextureOffset = new Vector2(childRenderer.material.mainTextureOffset.x % 100, 0);
        }
        if (itemInConveyor && !canPickUp)
            updateItemPosition();
        
    }
}
