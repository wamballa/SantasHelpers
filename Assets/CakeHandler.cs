using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeHandler : MonoBehaviour
{

  //public Transform[] wayPoints;
  PlayerController playerRightScript;

    bool okToCheck = true;

    // Start is called before the first frame update
    void Start()
    {
        playerRightScript = GameObject.Find("PlayerR").GetComponent<PlayerController>();
        if (playerRightScript == null) Debug.Log("ERROR: cannot find player right script");
    }

    // Update is called once per frame
    void Update()
    {
        // Check Each Waypoint for a cake
        CheckIfPlayerOnCakeLevel();
        // Check if player at a loaded waypoint - if it is then move cake else drop cake
    }

    void CheckIfPlayerOnCakeLevel(){

        if (okToCheck)
        {

        }
        int playerCheck = playerRightScript.rightPositionCurrent + 2;

        if (GameAssets.instance
            .wayPoints[playerCheck].GetComponent<WayPointHandler>().GetIsLoaded())
        {
            //Debug.Log("ON SAME LEVEL");

            GameAssets.instance
            .wayPoints[playerCheck].GetComponent<WayPointHandler>().GetCakeOnWayPoint().GetComponent<Present>().MoveToNextWayPoint();

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cake")
        {
            Debug.Log("Cake at waypoint ");
        }
    }
}
