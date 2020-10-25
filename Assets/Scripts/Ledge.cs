using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{

    public int direction;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        if (direction == 0) Debug.Log("ERROR: no direction set for ledge " + transform.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
