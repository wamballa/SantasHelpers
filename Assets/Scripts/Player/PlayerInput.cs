using System;
using System.Collections.Generic;
using UnityEngine;
#if MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED
using Lofelt.NiceVibrations;
#endif

public class PlayerInput : MonoBehaviour
{
    private enum TouchControlScheme
    {
        AutoDetect,
        ScreenQuadrants,
        DualThumbZones
    }

    [Header("Touch Controls")]
    [SerializeField] private TouchControlScheme touchControlScheme = TouchControlScheme.AutoDetect;
    [SerializeField] private bool useSafeArea = true;
    [SerializeField, Range(0.3f, 0.8f)] private float thumbZoneHeight = 0.42f;
    [SerializeField, Range(0.2f, 0.8f)] private float thumbZoneVerticalSplit = 0.52f;
    [SerializeField, Range(0f, 0.25f)] private float centerGapWidth = 0.04f;
    [SerializeField] private bool vibrateOnAcceptedTouch = true;

    private PlayerController playerController;
    private GameManager gameManager;
    private bool isSubscribedToGameManager;
    private readonly Queue<PlayerController.PlayerInputCommand> pendingCommands = new();

    // Control
    public bool canMove = true;

    void Start()
    {
        Initiate();
    }

    // Update is called once per frame
    void Update()
    {
        HandleKeyboardInput();
        HandleTouchInput();

        if (Application.isEditor && Input.touchCount == 0)
        {
            HandleMouseInput();
        }
        FlushPendingCommands();

    }

    private void HandleKeyboardInput()
    {
        if (!canMove) { return; }

        if (Input.GetKeyUp(KeyCode.P))
        {
            QueueCommand(new PlayerController.PlayerInputCommand(PlayerController.PlayerInputAction.RightUp));
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            QueueCommand(new PlayerController.PlayerInputCommand(PlayerController.PlayerInputAction.RightDown));
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            QueueCommand(new PlayerController.PlayerInputCommand(PlayerController.PlayerInputAction.LeftUp));
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            QueueCommand(new PlayerController.PlayerInputCommand(PlayerController.PlayerInputAction.LeftDown));
        }

    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    TryQueueTouchCommand(touch.position);
                }
            }
        }
    }

    private bool TryQueueTouchCommand(Vector2 touchPosition)
    {
        Rect interactionRect = GetTouchInteractionRect();
        if (!interactionRect.Contains(touchPosition))
        {
            return false;
        }

        PlayerController.PlayerInputCommand? command = ResolveTouchCommand(touchPosition, interactionRect);
        if (!command.HasValue)
        {
            return false;
        }

        QueueCommand(command.Value);
        return true;
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryQueueTouchCommand(Input.mousePosition);
        }
    }

    private void QueueCommand(PlayerController.PlayerInputCommand command)
    {
        pendingCommands.Enqueue(command);
    }

    private void FlushPendingCommands()
    {
        if (!canMove || playerController == null) return;
        while (pendingCommands.Count > 0)
        {
            bool applied = playerController.TryApplyInputCommand(pendingCommands.Dequeue());
            if (applied)
            {
                TriggerTouchFeedback();
            }
        }
    }

    private PlayerController.PlayerInputCommand? ResolveTouchCommand(Vector2 touchPosition, Rect interactionRect)
    {
        TouchControlScheme resolvedScheme = ResolveTouchControlScheme();
        return resolvedScheme switch
        {
            TouchControlScheme.DualThumbZones => ResolveDualThumbZoneCommand(touchPosition, interactionRect),
            _ => ResolveQuadrantCommand(touchPosition, interactionRect),
        };
    }

    private TouchControlScheme ResolveTouchControlScheme()
    {
        if (touchControlScheme != TouchControlScheme.AutoDetect)
        {
            return touchControlScheme;
        }

        return Application.isMobilePlatform
            ? TouchControlScheme.DualThumbZones
            : TouchControlScheme.ScreenQuadrants;
    }

    private Rect GetTouchInteractionRect()
    {
        if (!useSafeArea)
        {
            return new Rect(0f, 0f, Screen.width, Screen.height);
        }

        return Screen.safeArea;
    }

    private PlayerController.PlayerInputCommand? ResolveQuadrantCommand(Vector2 touchPosition, Rect interactionRect)
    {
        float midX = interactionRect.xMin + interactionRect.width * 0.5f;
        float midY = interactionRect.yMin + interactionRect.height * 0.5f;

        bool isLeft = touchPosition.x < midX;
        bool isTop = touchPosition.y >= midY;

        return BuildCommand(isLeft, isTop);
    }

    private PlayerController.PlayerInputCommand? ResolveDualThumbZoneCommand(Vector2 touchPosition, Rect interactionRect)
    {
        float gapWidth = interactionRect.width * centerGapWidth;
        float halfWidth = interactionRect.width * 0.5f;
        float leftZoneWidth = Mathf.Max(0f, halfWidth - gapWidth * 0.5f);
        float rightZoneWidth = leftZoneWidth;
        float zoneHeight = interactionRect.height * thumbZoneHeight;
        float zoneBottom = interactionRect.yMin;

        Rect leftZone = new Rect(interactionRect.xMin, zoneBottom, leftZoneWidth, zoneHeight);
        Rect rightZone = new Rect(interactionRect.xMax - rightZoneWidth, zoneBottom, rightZoneWidth, zoneHeight);

        if (leftZone.Contains(touchPosition))
        {
            return BuildCommand(true, IsUpperThumbZoneTouch(touchPosition, leftZone));
        }

        if (rightZone.Contains(touchPosition))
        {
            return BuildCommand(false, IsUpperThumbZoneTouch(touchPosition, rightZone));
        }

        return null;
    }

    private bool IsUpperThumbZoneTouch(Vector2 touchPosition, Rect zone)
    {
        float splitHeight = zone.yMin + zone.height * thumbZoneVerticalSplit;
        return touchPosition.y >= splitHeight;
    }

    private PlayerController.PlayerInputCommand BuildCommand(bool isLeft, bool isUpper)
    {
        PlayerController.PlayerInputAction action;

        if (isLeft)
        {
            action = isUpper
                ? PlayerController.PlayerInputAction.LeftUp
                : PlayerController.PlayerInputAction.LeftDown;
        }
        else
        {
            action = isUpper
                ? PlayerController.PlayerInputAction.RightUp
                : PlayerController.PlayerInputAction.RightDown;
        }

        return new PlayerController.PlayerInputCommand(action);
    }

    private void TriggerTouchFeedback()
    {
        if (!vibrateOnAcceptedTouch || !Application.isMobilePlatform)
        {
            return;
        }

#if MOREMOUNTAINS_NICEVIBRATIONS_INSTALLED
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.Selection);
#else
        Handheld.Vibrate();
#endif
    }

    private void Initiate()
    {

        playerController = GetComponent<PlayerController>();
        if (playerController == null) Debug.LogError("ERROR: no player controller");
        gameManager = FindFirstObjectByType<GameManager>();
        SubscribeToGameManagerEvents();

    }

    private void SubscribeToGameManagerEvents()
    {
        if (gameManager == null || isSubscribedToGameManager) return;
        gameManager.OnLevelComplete += OnLevelComplete;
        gameManager.OnLevelStart += OnLevelStart;
        gameManager.OnPauseGamePlay += OnPauseGamePlay;
        gameManager.OnResumeGamePlay += OnResumeGamePlay;
        gameManager.OnTruckPacked += OnTruckPacked;
        gameManager.OnGameOver += OnGameOver;
        isSubscribedToGameManager = true;
    }

    private void OnDestroy()
    {
        UnsubscribeFromGameManagerEvents();
    }

    private void UnsubscribeFromGameManagerEvents()
    {
        if (gameManager == null || !isSubscribedToGameManager) return;
        gameManager.OnLevelComplete -= OnLevelComplete;
        gameManager.OnLevelStart -= OnLevelStart;
        gameManager.OnPauseGamePlay -= OnPauseGamePlay;
        gameManager.OnResumeGamePlay -= OnResumeGamePlay;
        gameManager.OnTruckPacked -= OnTruckPacked;
        gameManager.OnGameOver -= OnGameOver;
        isSubscribedToGameManager = false;
    }

    private void OnGameOver(object sender, EventArgs e)
    {
        canMove = false;
        pendingCommands.Clear();
    }

    private void OnResumeGamePlay(object sender, EventArgs e)
    {
        canMove = true;
    }

    private void OnPauseGamePlay(object sender, EventArgs e)
    {
        canMove = false;
        pendingCommands.Clear();
    }

    private void OnTruckPacked(object sender, EventArgs e)
    {
        // throw new NotImplementedException();
    }
    private void OnLevelStart(object sender, EventArgs e)
    {
        canMove = true;
    }

    private void OnLevelComplete(object sender, EventArgs e)
    {
        canMove = false;
        pendingCommands.Clear();
    }
}
