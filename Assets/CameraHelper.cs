using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHelper : MonoBehaviour
{
    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("camerea = " + Camera.main);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDrawGizmos()
    {
        float verticalHeightSeen = camera.orthographicSize * 2.0f;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3((verticalHeightSeen * camera.aspect), verticalHeightSeen, 0));
    }
    //void OnDrawGizmos()
    //{
    //    float verticalHeightSeen = Camera.main.orthographicSize * 2.0f;

    //    Gizmos.color = Color.cyan;
    //    Gizmos.DrawWireCube(transform.position, new Vector3((verticalHeightSeen * Camera.main.aspect), verticalHeightSeen, 0));
    //}
}
