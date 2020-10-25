using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCakeDirection : MonoBehaviour
{

    public int direction;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        if (direction == null || speed == null) Debug.Log("ERROR: no direction or speed set for Conveyor Belt " + transform.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
