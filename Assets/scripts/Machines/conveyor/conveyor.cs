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

    private float length;

    [HideInInspector] public GameObject itemInConveyor;

    private GameObject child;
    private Renderer childRenderer;
    //private Renderer selfRenderer;

    private Vector3 objSize;

   public conveyor nextConveyorComponent;
   public conveyor prevConveyorComponent;
    

    
    
    // Start is called before the first frame update  
    void Start()
    {
        child = gameObject.transform.Find("base").gameObject;
        childRenderer = child.GetComponent<Renderer>();
        //selfRenderer = GetComponent<Renderer>();

        objSize = transform.localScale;
        length = objSize.x;
    }

    
    void addItemToConveyor(GameObject item)
    {
        if (itemInConveyor != null || isOccupied || !acceptsObjects)return;
        this.isOccupied = true;
        itemInConveyor = item;
        item.transform.parent = gameObject.transform;
        item.transform.localPosition = new Vector3(startPoint* length, 0, 0);

    }
   
    void deliverItem()
    {
        
    }
    
    void removeItemFromConveyor(conveyorItemRemoveReason reason)
    {
        
        switch (reason)
        {
            case conveyorItemRemoveReason.pickUp:
                break;
            case conveyorItemRemoveReason.continueLine:
                break;
            case conveyorItemRemoveReason.deliver:
                deliverItem();
                break;

        }
        
    }

    void removeItemFromConveyor(int reason)
    {
        removeItemFromConveyor((conveyorItemRemoveReason)reason);
    }
    
    void updateItemPosition()
    {
        canPickUp = false;
        if (itemInConveyor == null) return;
        if (itemInConveyor.transform.localPosition.x >= endPoint * length)
        {
            if (nextConveyorComponent)
            {
                if (nextConveyorComponent.isOccupied) return;
                else
                {
                    if (itemInConveyor.transform.localPosition.x >= .5 * length)
                    {
                        nextConveyorComponent.addItemToConveyor(this.itemInConveyor);
                        removeItemFromConveyor(conveyorItemRemoveReason.continueLine);
                    }
                        
                }
            }
            else {
                canPickUp = true;
                return;
            };
        }
        itemInConveyor.transform.localPosition = new Vector3(itemInConveyor.transform.localPosition.x + movingSpeed* Time.deltaTime, 0, 0);
        if (itemInConveyor.transform.localPosition.x > endPoint * length)
        {
            
        }

    }

        // Update is called once per frame
    void Update()
    {
        childRenderer.material.mainTextureOffset += new Vector2(movingSpeed*Time.deltaTime, 0);
        if (childRenderer.material.mainTextureOffset.x > 100)
            childRenderer.material.mainTextureOffset = new Vector2(childRenderer.material.mainTextureOffset.x % 100, 0);

        if (itemInConveyor)
            updateItemPosition();
        
    }
}
