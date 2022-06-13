using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class playerController : MonoBehaviour
{
    [Range(0.0f, 5.0f)] public float interactionTime = 1.0f;
    [SerializeField] private Image progressBar;
    [SerializeField] private Image progressBarBackground;
    [SerializeField] private GameObject itemPosObj;
    private ItemDirectory itemDirectory;
    private LevelHandler levelHandler;
    public GameObject item;
    [HideInInspector] public bool hasItem=false;
    
    
    public float touchingTime = 0.0f;
    public bool touching = false;
    public bool reachedTime = false;

    public Vector2 minPositions;
    public Vector2 maxPositions;
    public GameObject toFollow;

    public Vector3 offset = new Vector3(-3.8f, 3.35f, 1.66f);

    private bool progressBarVisible = false;
    
    

    private bool levelEnded = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!progressBar) return;
        levelHandler = LevelHandler.instance;
        itemDirectory = ItemDirectory.instance;
        setProgressBarVisibility(false);
        progressBar.fillAmount = 0;
    }

    
    void updatePosition()
    {
        Vector3 newPos = toFollow.transform.position;
        newPos.x = Mathf.Clamp(newPos.x, minPositions.x, maxPositions.x);
        newPos.z = Mathf.Clamp(newPos.z, minPositions.y, maxPositions.y);
        transform.position = newPos+offset;
        
    }

    void checkLevelEnded()
    {
        if (levelHandler.didLevelEnd())
        {
            levelEnded = true;
            setProgressBarVisibility(false);
            updateProgressBar(0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (levelEnded) return;
        checkLevelEnded();
        updatePosition();
        if (touching && !reachedTime)
        {
            this.touchingTime += Time.deltaTime;
            updateProgressBar();
        }
        
    }

    void updateProgressBar(float timeVal=-1.0f)
    {
        float timeValue = (timeVal == -1.0f) ? touchingTime : timeVal;
        this.progressBar.fillAmount = Mathf.Min(timeValue / interactionTime,1.0f);


    }
    
    void setProgressBarVisibility(bool state)
    {
        this.progressBarVisible = state;
        this.progressBar.gameObject.SetActive(state);
        this.progressBarBackground.gameObject.SetActive(state);


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
                hasItem = true;
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
                {
                    Debug.Log("bing bong the item should be gone");
                    this.removeItem(true);
                }
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

    private void combinerTrigger(GameObject other, bool isA)
    {
        Debug.Log("combiner hit");
        Combiner comb = other.transform.parent.GetComponent<Combiner>();
        if (item)
        {
            Debug.Log("Trying to insert item");
            if((isA&& comb.canInsertItemA)|| (!isA && comb.canInsertItemB))
            {
                if (comb.insertIntoMachine(this.item, isA))
                {
                    Debug.Log("bing bong the item should be gone");
                    this.removeItem(true);
                }
            }
        }
        else
        {
            Debug.Log("Trying to remove item");
            int itemId = comb.removeItem(isA);
            if (itemId == -1)
            {
                Debug.Log("No item to remove");
                return;
            }
            GameObject item = itemDirectory.getItemWithId(itemId);
            this.setItem(item);
        }
    }

    private void binTrigger()
    {
        Debug.Log("bin hit");
        if (item)
            this.removeItem(true);
    }

        private void OnTriggerEnter(Collider other)
    {
        if (levelEnded) return;
        this.touching = true;
        setProgressBarVisibility(true);
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (levelEnded) return;
        if (touchingTime >= interactionTime && !reachedTime)
        {
            reachedTime = true;
            setProgressBarVisibility(false);
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
                case "combinerA":
                case "combinerB":
                    combinerTrigger(other.gameObject, other.tag == "combinerA");
                    break;
                case "bin":
                    binTrigger();
                    break;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (levelEnded) return;
        this.touching = false;
        this.touchingTime = 0.0f;
        this.reachedTime = false;
        updateProgressBar();
        setProgressBarVisibility(false);
    }
}
