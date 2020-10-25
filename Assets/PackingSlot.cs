using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackingSlot : MonoBehaviour
{

    public bool isSlotFree = true;

    // Start is called before the first frame update
    void Start()
    {
        isSlotFree = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSlotToFilled()
    {
        if (isSlotFree) isSlotFree = false;
    }
    public bool GetIfSlotFree()
    {
        return isSlotFree;
    }

}
