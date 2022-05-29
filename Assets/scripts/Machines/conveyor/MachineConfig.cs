using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineConfig : MonoBehaviour
{
    // Start is called before the first frame update
    public List<int>acceptedItemIds;
    public List<Recepie> recepies;

    public bool isIdAccepted(int id)
    {
        foreach (int acceptedId in acceptedItemIds)
        {
            if (acceptedId == id)
            {
                return true;
            }
        }
        return false;
    }


    public Recepie getRecepieForItems(List<int> items)
    {
        foreach (Recepie recepie in recepies)
        {
            if (recepie.isRecepieForItems(items))
            {
                return recepie;
            }
        }
        return null;
    }


}
