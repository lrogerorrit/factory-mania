using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour
{

    public GameObject item;


    void generateItem(GameObject player)
    {
        Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
        Instantiate(item, pos, Quaternion.identity);
        //TODO: Add it to player controller
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
