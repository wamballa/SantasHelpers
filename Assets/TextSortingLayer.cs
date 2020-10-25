using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSortingLayer : MonoBehaviour
{

    public string sortingLayerName;
    public int orderInLayer;
    

    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = sortingLayerName;
        meshRenderer.sortingOrder = orderInLayer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
