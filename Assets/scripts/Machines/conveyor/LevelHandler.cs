using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    public static LevelHandler instance;
    public int timeRemaining = 300;

    public OrderHandler orderHandler;


    void Awake()
    {

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        orderHandler.enableTimer(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void handInBadOrder()
    {
        Debug.Log("Handing in bad order");
    }

    public void handInGoodOrder(int xp)
    {
        Debug.Log("Handing in good order ("+xp+")");
        
    }
}
