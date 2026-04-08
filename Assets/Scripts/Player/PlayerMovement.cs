using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerController playerController;
    private GameAssets gameAssets;
    private bool isInitialized;

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

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        if (!isInitialized)
        {
            Initialize();
        }

        SyncPlayersToCurrentPositions();
    }

    void Start()
    {
        if (!isInitialized)
        {
            Initialize();
        }

        SyncPlayersToCurrentPositions();
    }

    private void Initialize()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PLAYERMOVEMENT: Missing PlayerController reference.", this);
            enabled = false;
            return;
        }

        if (rightPlayerTransform == null || leftPlayerTransform == null)
        {
            Debug.LogError("PLAYERMOVEMENT: Assign Right Player Transform and Left Player Transform on PlayerMovement.", this);
            enabled = false;
            return;
        }

        gameAssets = ResolveGameAssets();
        if (gameAssets == null || gameAssets.RPos == null || gameAssets.LPos == null)
        {
            Debug.LogError("PLAYERMOVEMENT: Missing GameAssets position references.", this);
            enabled = false;
            return;
        }

        isInitialized = true;
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
        if (!isInitialized)
        {
            return;
        }

        // right
        if (playerController.RightPositionTarget != playerController.RightPositionCurrent)
        {
            // Move
            Log(">> " + playerController.RightPositionTarget);
            rightPlayerTransform.position = gameAssets.RPos[playerController.RightPositionTarget].position;
            playerController.RightPositionCurrent = playerController.RightPositionTarget;
        }
        if (playerController.LeftPositionTarget != playerController.LeftPositionCurrent)
        {
            // Move
            leftPlayerTransform.position = gameAssets.LPos[playerController.LeftPositionTarget].position;
            playerController.LeftPositionCurrent = playerController.LeftPositionTarget;
        }
    }
    [ContextMenu("Sync Players To Current Positions")]
    public void SyncPlayersToCurrentPositions()
    {
        if (!isInitialized)
        {
            return;
        }

        Log(">> RPOS = " + playerController.RightPositionCurrent);
        rightPlayerTransform.position = gameAssets.RPos[playerController.RightPositionCurrent].position;
        leftPlayerTransform.position = gameAssets.LPos[playerController.LeftPositionCurrent].position;
    }

    private GameAssets ResolveGameAssets()
    {
        if (gameAssets != null)
        {
            return gameAssets;
        }

        if (GameAssets.instance != null)
        {
            gameAssets = GameAssets.instance;
            return gameAssets;
        }

        gameAssets = FindFirstObjectByType<GameAssets>();
        return gameAssets;
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
