using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct orderData
{
    public Order order;
    public OrderCard card;
    public float spawnTime;
    public float remainingTime;
    public bool willDelete;
}

public class OrderHandler : MonoBehaviour
{
    public static OrderHandler instance;
    public int maxOrders = 4;
    public int activeOrderNum = 0;
    public List<Order> possibleOrders;
    public bool isActive = false;
    private SoundHandler soundHandler;


    public List<orderData> activeOrders= new List<orderData>();
    private bool timerOn = false;
    private float timer = 0.0f;

    private float ti;

    private float timeSinceLastOrder = 0.0f;
    public float minTimeBetweenOrders = 5.0f;

    [SerializeField] private List<GameObject> cardPositions;
    private ItemDirectory itemDirectory;
    [SerializeField] private GameObject template;

    private LevelHandler levelHandler;

    void Awake()
    {
        instance = this;
        Random.seed = System.DateTime.Now.Millisecond;
    }

    private void makeNewOrder()
    {
        int index = Random.Range(0, possibleOrders.Count);
        Debug.Log(index);
        Order selectedOrder = possibleOrders[index];
        Debug.Log(selectedOrder);
        
        while (timer < selectedOrder.minAppearanceTime)
        {
            
            Debug.Log("Rerolling");
            index = Random.Range(0, possibleOrders.Count);
            Debug.Log(index);
            selectedOrder = possibleOrders[index];
        }
        
        orderData newOrder = new orderData();
        newOrder.order = gameObject.AddComponent<Order>() as Order;
        newOrder.card = gameObject.AddComponent<OrderCard>() as OrderCard;
        newOrder.order.copyFrom(selectedOrder);
        newOrder.spawnTime = Time.time;
        newOrder.willDelete = false;
        newOrder.remainingTime = newOrder.order.getTime();
        newOrder.card.setUp(newOrder.order, cardPositions[activeOrderNum],template);
        soundHandler.playAudio("newOrder");


        
        activeOrders.Add(newOrder);
        
        activeOrderNum += 1;
        
        
        
    }


    void chechOrdersToDelete(bool noSound=false)
    {
        int displacement = 0;

        for (int i = 0; i < activeOrders.Count; ++i)
        {
            orderData o = activeOrders[i - displacement];
            if (activeOrders[i - displacement].card.getShouldDelete())
            {
                /*
                if(!noSound)
                    soundHandler.playAudio("failOrder");*/
                updateCardPositions(i - displacement);
                activeOrderNum = Mathf.Max(0, activeOrderNum - 1);

                activeOrders[i - displacement].card.destroyCard();
                Destroy(activeOrders[i - displacement].order);
                Destroy(activeOrders[i - displacement].card);
                activeOrders.Remove(activeOrders[i - displacement]);

                displacement++;


            }

        }
    }

    void updateCardPositions(int removedPos)
    {
        for (int i = 0; i < activeOrders.Count; i++)
        {
            if (i > removedPos)
                activeOrders[i].card.setNewParent(cardPositions[i-1].transform);
        }
    }

    void Start()
    {
        ti = Time.time;
        itemDirectory = ItemDirectory.instance;
        soundHandler = SoundHandler.instance;
        levelHandler = LevelHandler.instance;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        timeSinceLastOrder += Time.deltaTime;
        if ((timeSinceLastOrder > minTimeBetweenOrders) && (activeOrderNum < maxOrders))
        {
            if (Random.Range(0, 100) > 70)
            {
                makeNewOrder();
                timeSinceLastOrder = 0.0f;
            }
        }

        float dt = Time.deltaTime;
        if (timerOn)
            timer += dt;
        //Debug.Log((Time.time - ti)+"-"+ (timer));
        ti = Time.time;

        for (int i=0;i< activeOrders.Count;++i)
        {

            //activeOrders[i].remainingTime += dt;
            float remTime = ((Time.time - activeOrders[i].spawnTime) / activeOrders[i].order.getTime());
           // Debug.Log(remTime);
            activeOrders[i].card.setSlider(Mathf.Max(0.0f,1.0f-remTime));
            if ((1.0f-remTime) <= 0.0f && ! activeOrders[i].card.getShouldDelete())
            {
                levelHandler.failOrder((int) Mathf.Floor(activeOrders[i].order.getXp()/2));
                activeOrders[i].card.setShouldDelete(true);
                
            }
        }

        chechOrdersToDelete();





    }

    public void enableTimer(bool state)
    {
        timerOn = state;
        if (timerOn)
            timer = 0.0f;
    }

    public bool handInItem(int id)
    {
        for (int i = 0; i < activeOrders.Count; ++i)
        {
            orderData o = activeOrders[i];
            if (o.order.getId() == id)
            {
                levelHandler.handInGoodOrder(o.order.getXp(),id);
                activeOrders[i].card.setShouldDelete(true);
                chechOrdersToDelete(true);
                return true;
            }
        }
        levelHandler.handInBadOrder();
        return false;
    }

    public void forceSpawnItem()
    {
        makeNewOrder();
    }

    public void setIsActive(bool state)
    {
        isActive = state;
    }

    public void deleteRemainingOrders()
    {
        for (int i = 0; i < activeOrders.Count; ++i)
        {
            activeOrders[i].card.setShouldDelete(true);
        }
        chechOrdersToDelete(true);
    }

}
