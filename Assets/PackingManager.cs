using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackingManager : MonoBehaviour
{

    //public Transform[] packingSlots;
    PackingSlot[] packingSlots;
    public Transform[] slotTransforms;
    bool freeSlotFound;
    public Transform nextFreeSlot;
    int numberOfSlots;
    public bool areAllSlotsFilled;

    public bool isTruckFullyPacked = false;

    // Start is called before the first frame update
    void Start()
    {
        numberOfSlots = GameAssets.instance.packedPositions.Length;
    }

    // Update is called once per frame
    void Update()
    {
        //CheckIfAllSlotsFilled();

    }

    public Transform GetNextFreeSlot()
    {
        Debug.Log("GetNextFreeSlot ");
        freeSlotFound = false;
        int slotNum = 0;

        if (!GameManager.instance.isTruckPacked)
        {
            for (int i = 0; i < numberOfSlots; i++)
            {
                if (CheckIfSlotFree(i))
                {
                    //Debug.Log("Slot " + i + " is free ");
                    Transform freeSlotTransform = GameAssets.instance.packedPositions[i].transform;
                    SetSlotFilled(freeSlotTransform);
                    if (i == numberOfSlots-1) SetTruckIsFilled();
                    return freeSlotTransform;
                }
            }
        }
        return null;
    }
    public bool GetIsTruckFullyLoaded()
    {
        return isTruckFullyPacked;
    }
    void PrintSlotsStatus()
    {
        for (int i = 0; i < numberOfSlots; i++)
        {
            Debug.Log("Slot status " + i + " " + CheckIfSlotFree(i));
        }
    }
    void SetTruckIsFilled()
    {
        isTruckFullyPacked = true;
        GameManager.instance.isTruckPacked = true;
    }
    bool CheckIfSlotFree(int i)
    {
        bool _isSlotFree = GameAssets.instance
            .packedPositions[i].GetComponent<PackingSlot>()
            .GetIfSlotFree();
        //Debug.Log("Slot = " + i + " " + _isSlotFree);
        return _isSlotFree;

    }
    void SetSlotFilled (Transform transform)
    {
        transform.GetComponent<PackingSlot>().SetSlotToFilled();
    }
    bool CheckIfAllSlotsFilled()
    {

        for (int i = 0; i < numberOfSlots; i++)
        {
            if (!CheckIfSlotFree(i))
            {
                return false;
            }
        }
        return true;
    }

}
