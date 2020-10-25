using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets instance = null;

    [Header("Ladders")]
    public Transform[] laddersRight;
    public Transform[] laddersLeft;

    [Header ("Mechanics")]
    public Transform[] RPos;
    public Transform[] LPos;
    public Transform[] UnitPosition;
    public GameObject[] ConveyorBelt;
    [Header("Cakes")]
    public GameObject cake;
    public GameObject cakeExplosion;
    [Header("Ovens")]
    public GameObject[] ovens;
    [Header("Packed Positions")]
    public Transform[] packedPositions;
    //public Transform nextFreeSlot;

    [Header("Debug Mode")]
    public bool debugMode;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
