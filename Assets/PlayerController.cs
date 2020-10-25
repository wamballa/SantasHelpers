using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    int rightPositionTarget = 0;
    int rightPositionCurrent = 0;
    int leftPositionTarget = 0;
    int leftPositionCurrent = 0;
    bool isReadyToPack;
    Rigidbody2D rb;

    // 3 parts to a move
    bool canDoMove, moveDone, p1, p2, p3 = false;
    bool canAcceptInput = true;

    // Movement
    float climbingSpeed = 12f;

    //Animation
    Animator anim;
    bool isWalkingLeft, isWalkingRight, isClimbing = false;
    bool isIdle = true;


    // Start is called before the first frame update
    void Start()
    {

        InitiatePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        ApplyMovement();
        CheckIfMoveDone();
        CheckIfReadyToPack();
        DoAnimations();
    }
    private void FixedUpdate()
    {
        //DoAnimations();
    }
    void DoAnimations()
    {
        anim.SetBool("isRight", isWalkingRight);
        anim.SetBool("isClimbing", isClimbing);
        anim.SetBool("isLeft", isWalkingLeft);
        anim.SetBool("isIdle", isIdle);

    }
    void InitiatePlayer()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        anim = transform.GetComponent<Animator>();
        if (transform.name == "PlayerR")
        {
            transform.position = GameAssets.instance.RPos[0].position;
        }
        if (transform.name == "PlayerL")
        {
            transform.position = GameAssets.instance.LPos[0].position;
        }
    }
    void CheckIfMoveDone()
    {
        if (p1 && p2 && p3)
        {
            //Debug.Log("Movement done");
            moveDone = true;
            canAcceptInput = true;
            canDoMove = false;
        }
    }
    void CheckIfReadyToPack()
    {
        if (leftPositionTarget == 4)
            isReadyToPack = true;
        else isReadyToPack = false;
    }
    public bool GetIsReadyToPack()
    {
        return isReadyToPack;
    }
    void CheckInput()
    {
        if (transform.name == "PlayerR")
        {
            if (Input.GetKeyUp(KeyCode.P) && canAcceptInput)
            {

                if (rightPositionTarget < GameAssets.instance.RPos.Length-1)
                {

                    rightPositionTarget++;
                    SetPlayerReadyToClimb();
                    Debug.Log("PRESSED UP " + rightPositionTarget);
                }
            }
            if (Input.GetKeyUp(KeyCode.L) && canAcceptInput)
            {
                if (rightPositionTarget > 0)
                {
                    rightPositionTarget--;
                    SetPlayerReadyToClimb();
                }
            }
        }
        if (transform.name == "PlayerL")
        {
            if (Input.GetKeyUp(KeyCode.Q) && canAcceptInput)
            {
                if (leftPositionTarget < GameAssets.instance.LPos.Length-1)
                {
                    Debug.Log("PLAYER LEFT: move up");
                    leftPositionTarget++;
                    SetPlayerReadyToClimb();
                }
            }
            if (Input.GetKeyUp(KeyCode.A) && canAcceptInput)
            {
                if (leftPositionTarget > 0)
                {
                    leftPositionTarget--;
                    SetPlayerReadyToClimb();
                }
            }
        }

    }
    void SetPlayerReadyToClimb()
    {
        canDoMove = true;
        moveDone = false;
        p1 = false;
        p2 = false;
        p3 = false;
        canAcceptInput = false;
    }
    void ApplyMovement()
    {
        if (transform.name == "PlayerR")
        {
            if (!moveDone && canDoMove)
            {
                Debug.Log("RIGHT Current Target " + rightPositionCurrent + " " + rightPositionTarget);

                DoLadderAnimation(rightPositionCurrent, rightPositionTarget);
            }
        }
        if (transform.name == "PlayerL")
        {
            if (!moveDone && canDoMove)
            {
                Debug.Log("LEFT Current Target " + leftPositionCurrent + " " + leftPositionTarget);
                DoLadderAnimation(leftPositionCurrent, leftPositionTarget);
            }
        }
    }
    void DoLadderAnimation(int current, int target)
    {
        if (transform.name == "PlayerR")
        {
            // check animation bools
            bool isClimbingUp = target > current ? true : false;
            if (!isClimbingUp)
            {
                current--;
            }
            Vector2 ladderPosition = GameAssets.instance.laddersRight[current].position;
            Vector2 currentPosition = transform.position;
            Vector2 targetPosition = GameAssets.instance.RPos[target].position; //upladder
            // UP ladder
            if (current == 0 && isClimbingUp)
            {
                //move to ladder
                if (!p1 && !p2 && !p3)
                {
                    transform.position = new Vector2(ladderPosition.x, transform.position.y);
                    p1 = true;
                    p2 = true;

                }
                if (p1 && p2 && !p3)
                {
                    Debug.Log("Ready to move play up ladder  - target " + target);
                    float step = climbingSpeed * Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPosition.x, targetPosition.y), step);
                    if (transform.position.y == targetPosition.y)
                    {
                        p3 = true;
                        rightPositionCurrent = rightPositionTarget;
                        //Debug.Log("Player at final position ");
                        isWalkingRight = false;
                        isClimbing = false;
                        isWalkingLeft = false;
                        isIdle = true;
                    }
                }
            } else if (current == 1 && !isClimbingUp)
            {
                //move down ladder
                if (!p1 && !p2 && !p3)
                {
                    Debug.Log("Ready to move down ladder  - target " + target);
                    float step = climbingSpeed * Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPosition.x, targetPosition.y), step);
                    if (transform.position.y == targetPosition.y)
                    {
                        p1 = p2 = true;
                        rightPositionCurrent = rightPositionTarget;
                        //Debug.Log("Player at final position ");
                        isWalkingRight = false;
                        isClimbing = false;
                        isWalkingLeft = false;
                        isIdle = true;
                    }
                }
                if (p1 && p2 && !p3)
                {
                    transform.position = new Vector2(targetPosition.x, targetPosition.y);
                    p3 = true;
                    rightPositionCurrent = rightPositionTarget;
                    //Debug.Log("Player at final position ");
                    isWalkingRight = false;
                    isClimbing = false;
                    isWalkingLeft = false;
                    isIdle = true;
                }
            }
            else
            {
                p1 = p2 = true;
                if (p1 && p2 && !p3) // go to final point
                {
                    Debug.Log("Ready to move play up ladder  - target " + target);
                    float step = climbingSpeed * Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPosition.x, targetPosition.y), step);
                    if (transform.position.y == targetPosition.y)
                    {
                        p3 = true;
                        rightPositionCurrent = rightPositionTarget;
                        //Debug.Log("Player at final position ");
                        isWalkingRight = false;
                        isClimbing = false;
                        isWalkingLeft = false;
                        isIdle = true;
                    }
                }
            }



        }
        if (transform.name == "PlayerL")
        {
            bool isClimbingUp = target > current ? true : false;

            //Vector2 ladderPosition = GameAssets.instance.laddersLeft[current].position;
            //Vector2 currentPosition = transform.position;
            Vector2 targetPosition = GameAssets.instance.LPos[target].position; //upladder
            p1 = p2 = true;
            if (p1 && p2 && !p3) // goto final position
            {
                Debug.Log("Ready to move play up ladder  - target " + target);
                float step = climbingSpeed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPosition.x, targetPosition.y), step);
                if (transform.position.y == targetPosition.y)
                {
                    p3 = true;
                    leftPositionCurrent = leftPositionTarget;
                    //Debug.Log("Player at final position ");
                }
            }
        }
    }
    private void OnGUI()
    {
        if (transform.name == "PlayerR")
        {
            GUIStyle _style = new GUIStyle();
            _style.fontSize = 25;
            _style.normal.textColor = Color.white;
            float xPos = 700f;
            GUI.Label(new Rect(xPos, 0, 200, 100), "Anim Status ", _style);
            GUI.Label(new Rect(xPos, 25, 200, 100), "isIdle " + isIdle, _style);
            GUI.Label(new Rect(xPos, 50, 200, 100), "isLeft " + isWalkingLeft, _style);
            GUI.Label(new Rect(xPos, 75, 200, 100), "isRight " + isWalkingRight, _style);
            GUI.Label(new Rect(xPos, 100, 200, 100), "isClimb " + isClimbing, _style);
        }

    }
}
