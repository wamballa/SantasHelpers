using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Present : MonoBehaviour
{
  //GameAssets gameAssets;
  public Transform[] pos;
  public GameObject[] ledge;

  // Movement 
  public bool isOnConveyorBelt;
  public float speed;
  public int direction;

  public Rigidbody2D rb;
  private int stage = 0;

  public bool isCooking = false;
  public bool isAlive = true;

  public Transform OutOfMachinePos;

  public string ovenName;
  public GameObject ovenPF;

  // Different Cake sprites
  public Sprite[] cakeSprite;
  public int cakeSpriteNum = 0;
  private SpriteRenderer spriteRenderer;

  // Cake explosion
  GameObject cakeExplosion;

  // Put on truck
  //GameObject packingManager;
  //Transform packingManager;
  //PackingManager packingManagerScript;
  public float gravity = 1f;

  // Move cake to start point
  Vector3 startPos1;
  Vector3 startPos2;
  bool isMoingToStartPos = true;
  bool isAtStartPos1 = false;
  bool isAtStartPos2 = false;

  public bool hasMouseDetectedMe = false;

  int currentWaypoint = 0;
    bool isOnWayPoint = false;


  bool cakeAlive;

  // Start is called before the first frame update
  void Start()
  {
    rb = transform.GetComponent<Rigidbody2D>();
    //gameAssets = GameObject.Find("GameAssets").GetComponent<GameAssets>();
    spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    cakeExplosion = GameAssets.instance.cakeExplosion;

    // startPos1 = GameObject.Find("StartPos1").transform.position;
    // if (startPos1 == null) Debug.Log("ERROR: cannot find start pos 1");
    // startPos2 = GameObject.Find("StartPos2").transform.position;
    // if (startPos2 == null) Debug.Log("ERROR: cannot find start pos 2");


    //GameObject go = GameObject.Find("PackedPositions");
    //Debug.Log("GO = " + go);
    //packingManagerScript = GameObject.Find("PackedPositions").GetComponent<PackingManager>();
    //if (packingManagerScript == null) Debug.Log("ERROR: packing manager not found");
  }

  // Update is called once per frame
  void Update()
  {

  }
  public void SetMouseDetectedCake()
  {
    hasMouseDetectedMe = true;
  }
  public bool GetMouseDetectedCake()
  {
    return hasMouseDetectedMe;
  }
  private void FixedUpdate()
  {
    if (isAlive)
    {
      if (isMoingToStartPos)
      {
        CheckStartPositions();
      }
      else
      {
        CheckForPlayerOnWayPoint();
        ApplyMovement();
        UpdateSprite();
      }
    }

  }
    void CheckForPlayerOnWayPoint()
    {
        int playerPos = GameAssets.instance.playerR.GetComponent<PlayerController>().rightPositionCurrent;

        if (isOnWayPoint)
        {
            if (playerPos == currentWaypoint)
            {

            }
        }
    }
  void CheckStartPositions()
  {
    // if (!isAtStartPos1 && !isAtStartPos2)
    if (currentWaypoint == 0)
    {
      // if (transform.position.y <= startPos1.y)
      if (transform.position.y <= GameAssets.instance.wayPoints[currentWaypoint].position.y)
      {
        Debug.Log("Cake at Waypoint "+currentWaypoint);
        currentWaypoint++;
        // isAtStartPos1 = true;
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
      }
    }
    // if (isAtStartPos1 && !isAtStartPos2)
    if (currentWaypoint == 1)
    {
      if (transform.position.x - GameAssets.instance.wayPoints[currentWaypoint].position.x <=0 )
      {
        Debug.Log("..Cake at Waypoint "+currentWaypoint);
        currentWaypoint++;
        // isAtStartPos2 = true;
        // rb.gravityScale = gravity;
        // isMoingToStartPos = false;
      }
      else
      {

        float step = 2f * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, GameAssets.instance.wayPoints[currentWaypoint].position, step);
      }
    }
    if (currentWaypoint == 2){
        // Debug.Log("Cake at Waypoint "+currentWaypoint);
            // Tell waypoint it's go a cake on it
            //GameAssets.instance.wayPoints[currentWaypoint].GetComponent<WayPointHandler>().IsLoaded = true;
            // Tell Waypoint which cake
            GameAssets.instance.wayPoints[currentWaypoint].GetComponent<WayPointHandler>().SetCakeToWayPoint(gameObject);
            // Move cake to the waypoint
            transform.position = GameAssets.instance.wayPoints[currentWaypoint].position;
    }
    if (currentWaypoint == 3)
        {
            if (transform.position.x <= GameAssets.instance.wayPoints[currentWaypoint].position.x){
                Debug.Log("DONE");
            }
            else
            {
                float step = 2f * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, GameAssets.instance.wayPoints[currentWaypoint].position, step);
            }
        }
  }
    public void MoveToNextWayPoint()
    {
        Debug.Log("Move to next waypoint");
        currentWaypoint++;
    }
  void UpdateSprite()
  {
    spriteRenderer.sprite = cakeSprite[cakeSpriteNum];
  }
  void ApplyMovement()
  {
    if (isOnConveyorBelt && !isCooking && isAlive)
    {
      rb.velocity = new Vector2((direction * speed), 0);
    }
  }
  // Machine Collision
  private void OnTriggerEnter2D(Collider2D collision)
  {
    // if (collision.tag == "EndOfBelt")
    // {
    //   CakeHitEndOfBelt();
    // }

    if (collision.tag == "Machine" && isAlive)
    {
      CheckIfOvenReady(collision);
    }
    if (collision.tag == "Player" && isAlive)
    {
      if (collision.GetComponent<PlayerR>().GetIsReadyToPack())
      {
        PutOnTruck();
      }
      else
      {
        Debug.Log("PRESENT: Collision with Player");
        MoveUp();
      }
    }
    if (collision.tag == "Mouse" && isAlive)
    {
      //CakeDead();
      DropCakeForMouseToEat();
    }
  }
  // void CakeHitEndOfBelt()
  // {
  //   // Debug.Log("Hit End Of Belt");
  //   isAlive = false;
  //   rb.velocity = Vector2.zero;
  //   rb.gravityScale = gravity;
  // }
  void CheckIfOvenReady(Collider2D ovenCollider)
  {
    if (!ovenCollider.GetComponent<Machine>().GetIsCooking() && isAlive)
    {
      PutInOven(ovenCollider);
    }
    else
    {
      Debug.Log("PRESENT: Oven Full");
      DropCakeForMouseToEat();
      //Destroy(gameObject);
    }
  }
  private void PutInOven(Collider2D collision)
  {
    isCooking = true;
    isOnConveyorBelt = false;
    ovenName = collision.name;
    ovenPF = collision.transform.gameObject;
    //Debug.Log("PRESENT: PutInOven ovenName + ovenPF.name " + ovenName + " " + ovenPF.name);

    transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
    transform.position = new Vector2(collision.transform.position.x, transform.position.y);
    rb.velocity = new Vector2(0, 0);

    // Put cake into oven
    collision.GetComponent<Machine>().PutCakeInOven(gameObject);
  }
  private void DropCakeForMouseToEat()
  {
    //Debug.Log("DropCakeForMouseToEat");
    isAlive = false;
    isOnConveyorBelt = false;
    isCooking = false;
    rb.velocity = new Vector2(0, 0);
    rb.gravityScale = gravity;
  }

  public void KillCake()
  {
    GameManager.instance.ReduceCakesRemaining();
    Destroy(gameObject);
  }

  public void PutOnTruck()
  {
    Debug.Log("PutOnTruck");

    transform.position = GameAssets.instance.UnitPosition[5].position;
    //PackingManager _packingManager = packingManager.GetComponent<PackingManager>();
    //Transform t = packingManagerScript.GetNextFreeSlot();
    PackingManager packingManagerScript = GameObject.Find("PackedPositions").GetComponent<PackingManager>();
    Transform nextFreeSlot = packingManagerScript.GetNextFreeSlot();
    //Debug.Log("Next Free Slot = " + nextFreeSlot);

    transform.position = new Vector2(nextFreeSlot.position.x, transform.position.y);

    //packingManagerScript
    //Debug.Log("Transform  ", packingManagerScript);
    //Transform slot = packingManagerScript.GetNextFreeSlot();



    //transform.position = new Vector2(nextFreeSlot.position.x, transform.position.y);


    rb.velocity = new Vector2(0, 0);
    rb.gravityScale = gravity;
    isCooking = false;
    isOnConveyorBelt = false;
    isAlive = false;

    GameManager.instance.AddCakesPacked();
  }

  public void TakeOutOfOven()
  {
    Debug.Log("PRESENT: TakeUnitOutOfMacine");
    isCooking = false;
    isOnConveyorBelt = true;
    transform.GetComponentInChildren<SpriteRenderer>().enabled = true;
    // Change sprite
    cakeSpriteNum++;
  }
  void MoveUp()
  {
    Debug.Log("PRESENT: MoveUp to stage = " + stage);
    isOnConveyorBelt = true;
    isMoingToStartPos = false;
    rb.gravityScale = 0f;

    //transform.position = gameAssets.UnitPosition[stage].position;
    transform.position = GameAssets.instance.UnitPosition[stage].position;
    direction = GameAssets.instance.UnitPosition[stage].GetComponent<SetCakeDirection>().direction;
    speed = GameAssets.instance.UnitPosition[stage].GetComponent<SetCakeDirection>().speed;

    //speed = gameAssets.ConveyorBelt[stage].GetComponent<Ledge>().speed;
    //speed = GameAssets.instance.ConveyorBelt[stage].GetComponent<Ledge>().speed;
    //direction = gameAssets.ConveyorBelt[stage].GetComponent<Ledge>().direction;
    //direction = GameAssets.instance.ConveyorBelt[stage].GetComponent<Ledge>().direction;

    stage++;
  }
  public void SetIsCooking(bool b)
  {
    isCooking = b;
  }
  public bool GetIsCooking()
  {
    return isCooking;
  }
}
