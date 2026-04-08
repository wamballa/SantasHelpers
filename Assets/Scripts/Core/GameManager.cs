using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const string MenuSceneName = "Menu";
    private const string GameplayScenePrefix = "GameLevel";

    private enum GameState
    {
        Idle,
        Ongoing,
        GameOver,
        Paused,
        Resuming
    }

    [SerializeField] private GameState currentState = GameState.Idle;
    private GameState previousState = GameState.Idle;
    [SerializeField] private GameplaySessionState sessionState;
    [SerializeField] private GameplayHudPresenter hudPresenter;

    public bool DebugMode;
    public int CakesPacked
    {
        get => sessionState != null ? sessionState.CakesPacked : 0;
    }
    public bool isTruckPacked
    {
        get => sessionState != null && sessionState.IsTruckPacked;
        set
        {
            if (sessionState != null)
            {
                sessionState.IsTruckPacked = value;
            }
        }
    }
    private bool isGameOver;

    public event EventHandler OnLevelComplete;
    public event EventHandler OnLevelStart;
    public event EventHandler OnTruckPacked;
    public event EventHandler OnCakeDropped;
    public event EventHandler OnPauseGamePlay;
    public event EventHandler OnResumeGamePlay;
    public event EventHandler OnGameOver;

    public int CurrentLevel => sessionState != null ? sessionState.CurrentLevel : 0;
    public int CakeDropPauseTimer { get; set; } = 3;

    public bool debugMode = false;
    public int TruckCapacity => sessionState != null ? sessionState.TruckCapacity : 0;

    private void Awake()
    {
        ResolveDependencies();
    }

    private void Start()
    {
        Log($"> {nameof(GameManager)} > Start");
        ApplySceneState(SceneManager.GetActiveScene().name);
    }

    private void ResetGameManager()
    {
        Log("> GameManager > ResetGameManager: Resetting game variables");
        sessionState?.ResetForMenu();
        currentState = GameState.Idle;
        isGameOver = false;
    }

    private void InitializeLevel(int levelNumber)
    {
        Log("> GameManager > InitializeLevel: Setting up level variables");
        currentState = GameState.Ongoing;
        sessionState?.StartLevel(levelNumber);
        isGameOver = false;

        OnLevelStart?.Invoke(this, EventArgs.Empty);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"> GameManager > OnSceneLoaded: {scene.name}");
        hudPresenter?.InvalidateCache();
        ApplySceneState(scene.name);
    }

    private void ApplySceneState(string sceneName)
    {
        if (sceneName == MenuSceneName)
        {
            ResetGameManager();
        }
        else if (IsGameplayScene(sceneName))
        {
            InitializeLevel(ParseGameplayLevel(sceneName));
        }
        else
        {
            currentState = GameState.Idle;
        }
    }

    private bool IsGameplayScene(string sceneName)
    {
        return sceneName.StartsWith(GameplayScenePrefix, StringComparison.Ordinal);
    }

    private int ParseGameplayLevel(string sceneName)
    {
        if (!IsGameplayScene(sceneName))
        {
            return 0;
        }

        string suffix = sceneName.Substring(GameplayScenePrefix.Length).Trim();
        if (string.IsNullOrEmpty(suffix))
        {
            return 1;
        }

        return int.TryParse(suffix, out int parsedLevel) ? parsedLevel : 1;
    }

    private void Update()
    {
        UpdateUI();
        CheckIfLivesLeft();

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            HandlePause();
        }
    }

    private void HandlePause()
    {
        if (currentState == GameState.Paused)
        {
            Log("Game Resumed");
            OnResumeGamePlay?.Invoke(this, EventArgs.Empty);
            currentState = previousState;
            return;
        }

        Log("Game Paused");
        previousState = currentState;
        currentState = GameState.Paused;
        OnPauseGamePlay?.Invoke(this, EventArgs.Empty);
    }

    private void CheckIfLivesLeft()
    {
        if (DebugMode) return;
        if (!IsGameplayScene(SceneManager.GetActiveScene().name)) return;
        if (sessionState == null || GlobalVariables.Instance == null) return;

        if (sessionState.Lives <= 0 && !isGameOver)
        {
            Log(GetSceneName() + "CheckIfLivesLeft ==============================");
            isGameOver = true;
            currentState = GameState.GameOver;
            GameUtilities.SaveHighScore(CakesPacked);
            OnGameOver?.Invoke(this, EventArgs.Empty);
            StartCoroutine(HandleGameOver());
        }
    }

    private IEnumerator HandleGameOver()
    {
        yield return new WaitForSeconds(3);
        Debug.Log(GetSceneName() + nameof(GameManager) + $": HandleGameOver. Level = {CurrentLevel}.");
        sessionState?.ClearCurrentLevel();
        SceneManager.LoadScene("GameOver");
    }

    public void HandleCakeDroppedEvent()
    {
        OnPauseGamePlay?.Invoke(this, EventArgs.Empty);
    }

    public void HandleMouseEatsCakeEvent()
    {
        OnResumeGamePlay?.Invoke(this, EventArgs.Empty);
        sessionState?.ConsumeLife();
    }

    public void RestartLevel()
    {
        Debug.Log("> GameManager > RestartLevel: Resetting level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    private void UpdateUI()
    {
        if (!IsGameplayScene(SceneManager.GetActiveScene().name) || sessionState == null) return;
        hudPresenter?.Refresh(sessionState);
    }

    public void AddPackedCakes()
    {
        if (sessionState == null || !sessionState.TryAddPackedCake())
        {
            return;
        }

        if (sessionState.IsTruckPacked)
        {
            Log("> GameManager > AddPackedCakes: Truck is now packed @@@@@@@@@");
            OnTruckPacked?.Invoke(this, EventArgs.Empty);
            OnLevelComplete?.Invoke(this, EventArgs.Empty);

            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Cake");
            foreach (var gameObject in gameObjects)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ResolveDependencies()
    {
        if (sessionState == null)
        {
            sessionState = GetComponent<GameplaySessionState>();
            if (sessionState == null)
            {
                sessionState = FindFirstObjectByType<GameplaySessionState>();
            }
        }

        if (hudPresenter == null)
        {
            hudPresenter = GetComponent<GameplayHudPresenter>();
            if (hudPresenter == null)
            {
                hudPresenter = FindFirstObjectByType<GameplayHudPresenter>();
            }
        }
    }

    private string GetSceneName()
    {
        return SceneManager.GetActiveScene().name + ": ";
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
