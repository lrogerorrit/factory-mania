using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class box : MonoBehaviour
{

    public int itemId;
    private ItemDirectory itemDirectory;
    [SerializeField] private Image icon;


    public GameObject generateItem()
    {
        return itemDirectory.getItemWithId(itemId);
        //TODO: Add it to player controller
    }

    void Start()
    {
        itemDirectory = ItemDirectory.instance;
        this.icon.sprite= itemDirectory.getIdSprite(itemId);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
