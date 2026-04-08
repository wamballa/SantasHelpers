using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController playerController;

    public bool IsWalkingLeft { get; set; } = false;
    public bool IsWalkingRight { get; set; } = false;
    public bool IsClimbing { get; set; } = false;
    public bool IsIdle { get; set; } = true;
    public bool CanDoMove { get; set; } = false;
    public bool MoveDone { get; set; } = false;
    public bool P1 { get; set; } = false;
    public bool P2 { get; set; } = false;
    public bool P3 { get; set; } = false;
    public bool CanAcceptInput { get; set; } = true;
    public float ClimbingSpeed { get; set; } = 12f;
    public int RightPositionTarget { get; set; } = 0;
    public int LeftPositionTarget { get; set; } = 0;

    [SerializeField] private Transform rightPlayerTransform;
    [SerializeField] private Transform leftPlayerTransform;

    public bool debugMode = false;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            LogError("Missing PlayerController reference.");
            enabled = false;
            return;
        }

        if (rightPlayerTransform == null || leftPlayerTransform == null)
        {
            LogError("Assign Right Player Transform and Left Player Transform on PlayerMovement.");
            enabled = false;
            return;
        }

        if (GameAssets.instance == null || GameAssets.instance.RPos == null || GameAssets.instance.LPos == null)
        {
            LogError("Missing GameAssets position references.");
            enabled = false;
            return;
        }

        InitiatePlayers();
    }

    void Update()
    {
        // ApplyMovement();
        //CheckIfMoveDone();
        //CheckIfReadyToPack();
        // DoAnimations();
        HandlePlayerMove();
    }

    private void HandlePlayerMove()
    {
        // right
        if (playerController.RightPositionTarget != playerController.RightPositionCurrent)
        {
            // Move
            Log(">> " + playerController.RightPositionTarget);
            rightPlayerTransform.position = GameAssets.instance.RPos[playerController.RightPositionTarget].position;
            playerController.RightPositionCurrent = playerController.RightPositionTarget;
        }
        if (playerController.LeftPositionTarget != playerController.LeftPositionCurrent)
        {
            // Move
            leftPlayerTransform.position = GameAssets.instance.LPos[playerController.LeftPositionTarget].position;
            playerController.LeftPositionCurrent = playerController.LeftPositionTarget;
        }
    }
    void InitiatePlayers()
    {
        Log(">> RPOS = " + playerController.RightPositionCurrent);
        rightPlayerTransform.position = GameAssets.instance.RPos[playerController.RightPositionCurrent].position;
        leftPlayerTransform.position = GameAssets.instance.LPos[playerController.LeftPositionCurrent].position;
    }

    private void Log(string msg)
    {
        if (debugMode) Debug.Log("GAMEMANAGER: " + msg);
    }

    private void LogError(string msg)
    {
        if (debugMode) Debug.LogError("GAMEMANAGER: " + msg);
    }
}
