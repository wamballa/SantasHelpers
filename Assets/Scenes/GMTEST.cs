using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GMTEST : MonoBehaviour
{
    public static GMTEST instance = null;
    public int Level { get; set; }
    private bool isInitialized = false;

    public TMP_Text levelText;


    private void Awake()
    {
        GetText().text = Level.ToString();

        Debug.Log("GameManager initialized with Level: " + Level);

        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            isInitialized = true;
        }
    }

    private void OnEnable()
    {
        Debug.Log("GameManager ON ENABLE ");
        GetText().text = Level.ToString();
    }

    void Start()
    {
        Debug.Log("GameManager Start, Level: " + Level);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            {
            print("Level number is " + Level);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Level++;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            print("RESTART===================================================");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }

        GetText().text = Level.ToString();

    }

    TMP_Text GetText()
    {
        GameObject go = GameObject.Find("LevelText");
        if (go != null)
        {
            levelText = go.GetComponent<TMP_Text>();
        }
        return levelText;
    }

}
