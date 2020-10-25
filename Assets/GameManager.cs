using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [Header("Globals")]
     int startNumberOfCakes = 3;
     int cakesRemaining;
     public int cakesPacked;

    public bool isTruckPacked;

    [Header("UI")]
    public TMP_Text cakeRemainingText;
    public TMP_Text cakesPackedText;

    // Event
    public event EventHandler OnLevelComplete;
    public event EventHandler OnLevelStart;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        cakesRemaining = startNumberOfCakes;
        cakesPacked = 0;

        OnLevelStart?.Invoke(this, EventArgs.Empty);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        cakeRemainingText.text = cakesRemaining.ToString();
        cakesPackedText.text = cakesPacked.ToString();

    }
    public void AddCakesPacked()
    {
        cakesPacked++;
        if (cakesPacked == 3)
        {
            Debug.Log("@@@@@@@@@ Truck is now packed @@@@@@@@@");
            OnLevelComplete?.Invoke(this, EventArgs.Empty);
        }
    }
    public void ReduceCakesRemaining()
    {
        //Debug.Log("Reduxe cakes");
        cakesRemaining--;
    }

    void StopLevel()
    {

    }
}
