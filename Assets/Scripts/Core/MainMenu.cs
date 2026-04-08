using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private const string FirstLevelSceneName = "Level01";

    public void PlayGame()
    {
        if (GlobalVariables.Instance != null)
        {
            GlobalVariables.Instance.ResetValues();
        }
        SceneManager.LoadScene(FirstLevelSceneName);
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
