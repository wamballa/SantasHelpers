using System;
using System.Collections.Generic;
using UnityEngine;

public class CakeSpawner : MonoBehaviour
{
    private enum SpawnerState
    {
        Idle,
        Running,
        Stopped
    }

    [SerializeField] private bool showDebugOverlay;
    [SerializeField] private bool _debugMode;
    [SerializeField] private SpawnerState currentState;
    [SerializeField] private Transform feederTransform;
    [SerializeField] private List<float> currentLevelDelays = new();
    [SerializeField] private int currentDelayIndex;
    [SerializeField] private bool isSpawnerOn = true;

    private GameObject cakePrefab;
    private GameManager gameManager;
    private GameplayConfig gameplayConfig;
    private float spawnTimer;
    private bool isSubscribedToEvents;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        gameplayConfig = ResolveGameplayConfig();

        if (GameAssets.instance != null)
        {
            cakePrefab = GameAssets.instance.cake;
            _debugMode = GameAssets.instance.debugMode;
        }

        SubscribeToEvents();
        ResetSpawnCycle();
        currentState = isSpawnerOn ? SpawnerState.Running : SpawnerState.Idle;
    }

    private void Update()
    {
        if (!isSpawnerOn || cakePrefab == null)
        {
            return;
        }

        if (_debugMode)
        {
            HandleDebugSpawnInput();
        }

        if (currentLevelDelays == null || currentLevelDelays.Count == 0)
        {
            return;
        }

        spawnTimer -= Time.deltaTime;
        if (spawnTimer > 0f)
        {
            return;
        }

        SpawnCake();
        AdvanceSpawnCycle();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void OnLevelStart(object sender, EventArgs e)
    {
        isSpawnerOn = true;
        currentState = SpawnerState.Running;
        ResetSpawnCycle();
    }

    private void OnLevelComplete(object sender, EventArgs e)
    {
        isSpawnerOn = false;
        currentState = SpawnerState.Stopped;
    }

    private void OnGameOver(object sender, EventArgs e)
    {
        isSpawnerOn = false;
        currentState = SpawnerState.Stopped;
    }

    private void SpawnCake()
    {
        Vector3 spawnPosition = feederTransform != null ? feederTransform.position : transform.position;
        Instantiate(cakePrefab, spawnPosition, Quaternion.identity);
    }

    private void AdvanceSpawnCycle()
    {
        if (currentLevelDelays == null || currentLevelDelays.Count == 0)
        {
            spawnTimer = 0f;
            return;
        }

        currentDelayIndex = (currentDelayIndex + 1) % currentLevelDelays.Count;
        spawnTimer = Mathf.Max(0.1f, currentLevelDelays[currentDelayIndex]);
    }

    private void ResetSpawnCycle()
    {
        currentLevelDelays = ResolveCurrentLevelDelays();
        currentDelayIndex = 0;
        spawnTimer = currentLevelDelays.Count > 0 ? Mathf.Max(0.1f, currentLevelDelays[0]) : 0f;
    }

    private List<float> ResolveCurrentLevelDelays()
    {
        if (gameplayConfig != null && gameManager != null)
        {
            List<float> delays = gameplayConfig.GetSpawnDelaysForLevel(gameManager.CurrentLevel);
            if (delays != null && delays.Count > 0)
            {
                return new List<float>(delays);
            }
        }

        return new List<float> { 5f };
    }

    private GameplayConfig ResolveGameplayConfig()
    {
        GameplayConfig config = Resources.Load<GameplayConfig>("DefaultGameplayConfig");
        return config;
    }

    private void SubscribeToEvents()
    {
        if (gameManager == null || isSubscribedToEvents)
        {
            return;
        }

        gameManager.OnLevelStart += OnLevelStart;
        gameManager.OnLevelComplete += OnLevelComplete;
        gameManager.OnGameOver += OnGameOver;
        isSubscribedToEvents = true;
    }

    private void UnsubscribeFromEvents()
    {
        if (gameManager == null || !isSubscribedToEvents)
        {
            return;
        }

        gameManager.OnLevelStart -= OnLevelStart;
        gameManager.OnLevelComplete -= OnLevelComplete;
        gameManager.OnGameOver -= OnGameOver;
        isSubscribedToEvents = false;
    }

    private void HandleDebugSpawnInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnCake();
        }
    }

    private void OnGUI()
    {
        if (!showDebugOverlay || !(Application.isEditor || Debug.isDebugBuild))
        {
            return;
        }

        GUIStyle style = new()
        {
            fontSize = 22,
            normal = { textColor = Color.black }
        };

        GUI.Label(new Rect(10, 10, 400, 30), $"Spawner: {currentState} DelayIndex: {currentDelayIndex}", style);
    }
}
