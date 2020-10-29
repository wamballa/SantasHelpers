using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{

  public bool unitLoaded = false;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  private void OnTriggerEnter2D(Collider2D other)
  {

    if (other.tag == "Cake")
    {
      Debug.Log("Load Waypoint");
    }
  }

  public void SetWaypointLoaded()
  {
    unitLoaded = true;
  }
  public bool GetWaypointLoadStatus()
  {
    return unitLoaded;
  }



}
