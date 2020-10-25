using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CakeSpawner : MonoBehaviour
{
    public bool _debugMode = false;
    bool canDropDebugCake = false;
    public GameObject debugGO;

    GameObject cake;
    bool canSpawnCake = true;
    [SerializeField] float timerDelay = 10f;
    float timer;

    public Transform truckDropPosition;

    bool isSpawnerOn;

    private void Awake()
    {

    }

    void Start()
    {
        // Subscribe to events
        GameManager.instance.OnLevelComplete += OnLevelComplete;
        GameManager.instance.OnLevelStart += OnLevelStart;

        //_debugMode = GameAssets.instance.debugMode;
        cake = GameAssets.instance.cake;
        _debugMode = GameAssets.instance.debugMode;

    }
    // Update is called once per frame
    void Update()
    {
        if (isSpawnerOn)
        {
            CheckIfCanSpawn();
            SpawnCake();
            CheckInput();
            CheckIfTruckPacked();
        }
    }
    private void FixedUpdate()
    {

    }
    private void LateUpdate()
    {
        if (canDropDebugCake)
        {
            canDropDebugCake = false;
            if (debugGO != null)
                debugGO.GetComponent<Present>().PutOnTruck();
        }
    }
    void CheckIfTruckPacked()
    {

    }
    void CheckIfCanSpawn()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            timer = timerDelay;
            canSpawnCake = true;
        }

    }
    void SpawnCake()
    {
        if (canSpawnCake && !_debugMode)
        {
            Instantiate(cake, transform.position, Quaternion.identity);
            canSpawnCake = false;
        }
    }
    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!GameManager.instance.isTruckPacked)
            {
                debugGO = Instantiate(cake);
                canDropDebugCake = true;
            }

        }
    }
    void OnLevelComplete(object sender, EventArgs e)
    {
        isSpawnerOn = false;
    }
    void OnLevelStart(object sender, EventArgs e)
    {
        isSpawnerOn = true;
    }
}
