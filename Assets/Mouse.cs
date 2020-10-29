using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{

    //Transform _transform;
    Vector3 startPos;

    bool isGettingCake;
    bool isReturningToStart;

    Rigidbody2D rb;
    float speed = 15f;
    Transform _cakeDetected;

    public List<GameObject> droppedCakeList = new List<GameObject>();

    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        CheckForCakeToEat();
        ApplyMovement();
        CheckIfNearCake();
        CheckIfHome();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Cake" && !collision.GetComponent<Present>().GetMouseDetectedCake())
        {
            // Debug.Log("Mouse detects cake " + collision.name);
            collision.GetComponent<Present>().SetMouseDetectedCake();
            droppedCakeList.Add(collision.gameObject);
        }
    }
    void CheckIfNearCake()
    {
        if (isGettingCake && !isReturningToStart)
        {
            float deltaX = transform.position.x - target.transform.position.x;
            if (Mathf.Abs(deltaX) <= 0.5f)
            {
                EatCake();
            }
        }

    }
    private void EatCake()
    {
        if (isGettingCake && !isReturningToStart)
        {
            // Debug.Log("EatCake");
            isGettingCake = false;
            isReturningToStart = true;
            GameObject _explosion = Instantiate(GameAssets.instance.cakeExplosion, transform.position, Quaternion.identity);
            Destroy(_explosion, 3f);
            // Debug.Log("Target / dropList " + target + " / " + droppedCakeList);
            //Destroy(target);
            if (droppedCakeList.Count > 0)
            {
                droppedCakeList.RemoveAt(0);
            }
            else
            {
                Debug.Log("ERROR: cannot remove from list");
            }
            target.GetComponent<Present>().KillCake();
        }
    }
    void CheckForCakeToEat()
    {
        if (droppedCakeList.Count > 0 && !isGettingCake && !isReturningToStart)
        {
            //Debug.Log("CheckIfAnyCakeToEat1");
            target = droppedCakeList[0];
            isGettingCake = true;
            isReturningToStart = false;
        }
    }
    void ApplyMovement()
    {
        if (isGettingCake && !isReturningToStart)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
        }
        if (isReturningToStart && !isGettingCake)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, startPos, step);
        }
    }
    void CheckIfHome()
    {
        if (isReturningToStart)
        {
            if (transform.position == startPos)
            {
                isReturningToStart = false;
                isGettingCake = false;
                rb.velocity = new Vector2(0, 0);
            }
        }
    }
    private void OnGUI()
    {
        GUIStyle _style = new GUIStyle();
        _style.fontSize = 25;
        _style.normal.textColor = Color.white;
        float xPos = 300f;
        int cakeNum = 0;
        GUI.Label(new Rect(xPos, 0, 200, 100), "Mouse status ", _style);
        foreach (GameObject obj in droppedCakeList)
        //droppedCakeList.ForEach
        {
            GUI.Label(new Rect(xPos, 25+(cakeNum*25), 200, 100), "DropList " + droppedCakeList[cakeNum], _style);
            cakeNum++;
        }
    }
}
