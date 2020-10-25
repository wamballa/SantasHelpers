using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Machine : MonoBehaviour
{
    public bool isCooking = false;
    private Animator anim;
    [Header("Cake stuff")]
    GameObject cake = null;
    GameObject cakeExplosion;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        if (anim == null) Debug.Log("ERROR: no animator found");
        cakeExplosion = GameAssets.instance.cakeExplosion;
        if (cakeExplosion == null) Debug.Log("ERROR: no cake explosion found");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimations();
    }
    void UpdateAnimations()
    {
        anim.SetBool("isCooking", isCooking);
    }
    public void PutCakeInOven(GameObject _cake)
    {
        if (!isCooking)
        {
            cake = _cake;
            isCooking = true;
        }
    }
    public void TakeCakeOutOfOven()
    {
        cake.GetComponent<Present>().TakeOutOfOven();
        cake = null;
        isCooking = false;
    }
    public void FinishCooking()
    {
        if (cake != null)
        {
            GameObject _cakeExplosion = Instantiate(cakeExplosion);
            Destroy(_cakeExplosion, 3f);
            Destroy(cake);
            //Debug.Log("MACHINE: Burnt Cake "+gameObject.name);
        }
        cake = null;
        isCooking = false;
    }
    public bool GetIsCooking()
    {
        return isCooking;
    }
    private void OnMouseDown()
    {
        //Debug.Log("MACHINE: OnMouseDown isCooking & cake "+isCooking+" "+cake);
        if (isCooking)
        {
            TakeCakeOutOfOven();
        }
    }
    private void OnGUI()
    {
        GUIStyle _style = new GUIStyle();
        _style.fontSize = 25;
        _style.normal.textColor = Color.white;
        GUI.Label(new Rect(0, 0, 200, 100), "Oven Status ", _style);
        int numberOfOven = GameAssets.instance.ovens.Length;
        for (int i = 0; i < numberOfOven; i++)
        {
            bool checkOven;
            checkOven = GameAssets.instance.ovens[i].GetComponent<Machine>().GetIsCooking();
            int posX = i + 1;
            GUI.Label(new Rect(0, posX * 25, 200, 100), "Oven " + i + " is cooking " + checkOven, _style);
        }
    }
}
