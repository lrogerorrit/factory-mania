using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDirectory : MonoBehaviour
{

    public List<GameObject> itemDirectory;
    // Start is called before the first frame update

    public GameObject getItemWithId(int i)
    {
        if (itemDirectory[i])
        {
            Vector3 pos = new Vector3(0, 0, 0);
            GameObject toReturn = Instantiate(itemDirectory[i], pos, Quaternion.identity);
            return toReturn;
        }
        return null;
    }
    
}
