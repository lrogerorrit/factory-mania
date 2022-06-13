using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderCard : MonoBehaviour
{

    private Text text;
    private Image slider;
    private Image icon;
    private GameObject card;
    private ItemDirectory itemDirectory;
    private bool willDelete;


    void Start()
    {
        
    }

    public void setUp(Order order,GameObject parentObj,GameObject template)
    {
        willDelete = false;
        if(!itemDirectory)
            itemDirectory = ItemDirectory.instance;
        this.card = Instantiate(template, parentObj.transform);
        setNewParent(parentObj.transform);
        this.text = this.card.GetComponentInChildren<Text>();
        this.slider = this.card.transform.Find("Canvas/card/slider").GetComponent<Image>();
        this.icon = this.card.transform.Find("Canvas/card/image").GetComponent<Image>();
        this.text.text = itemDirectory.getObjectName(order.getId());
        setSlider(1.0f);
        this.icon.sprite = itemDirectory.getIdSprite(order.getId());
    }
    
    public GameObject getCard()
    {
        return this.card;
    }

    public void setSlider(float value)
    {
        slider.fillAmount = value;
    }

    public void setNewParent(Transform parent)
    {
        this.card.transform.SetParent(parent,false);
        this.card.transform.parent = parent.transform;
        this.card.transform.localPosition = new Vector3(0, 0, 0);
        this.card.transform.localRotation = Quaternion.identity;
    }

    public bool getShouldDelete()
    {
        return willDelete;
    }
    public void setShouldDelete(bool state)
    {
        this.willDelete = state;
    }

    public void destroyCard()
    {
        Destroy(this.card.gameObject);
    }
}
