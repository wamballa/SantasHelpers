using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointHandler : MonoBehaviour
{
    public GameObject cakeOnWayPoint;

  public bool isLoaded = false;
  public bool IsLoaded {
    get { return isLoaded;}
    set{ isLoaded = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (isLoaded == true) Debug.Log("TRUE");
    }

    public bool GetIsLoaded(){
      return isLoaded;
    }
    public void SetIsLoaded(){
      isLoaded = true;
    }
    public void SetCakeToWayPoint(GameObject c)
    {
        isLoaded = true;
        cakeOnWayPoint = c;
    }
    public GameObject GetCakeOnWayPoint()
    {
        return cakeOnWayPoint;
    }


}
