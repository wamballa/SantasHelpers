using UnityEngine;

public class GameplaySessionState : MonoBehaviour
{
    [SerializeField] private GameplayConfig gameplayConfig;
    [SerializeField, Min(1)] private int fallbackTruckCapacity = 8;

    public int CakesPacked { get; private set; }
    public int CurrentLevel { get; private set; }
    public bool IsTruckPacked { get; set; }
    public bool CanPackTruck { get; private set; } = true;
    public int TruckCapacity => ResolveTruckCapacity();

    public int Lives
    {
        get => GlobalVariables.Instance != null ? GlobalVariables.Instance.Lives : 0;
        set
        {
            if (GlobalVariables.Instance != null)
            {
                GlobalVariables.Instance.Lives = value;
            }
        }
    }

    public int Score
    {
        get => GlobalVariables.Instance != null ? GlobalVariables.Instance.Score : 0;
        set
        {
            if (GlobalVariables.Instance != null)
            {
                GlobalVariables.Instance.Score = value;
            }
        }
    }

    public void ResetForMenu()
    {
        if (GlobalVariables.Instance != null)
        {
            GlobalVariables.Instance.ResetValues();
        }

        CakesPacked = 0;
        CurrentLevel = 0;
        IsTruckPacked = false;
        CanPackTruck = true;
    }

    public void StartLevel(int levelNumber)
    {
        CurrentLevel = Mathf.Max(1, levelNumber);
        CakesPacked = 0;
        IsTruckPacked = false;
        CanPackTruck = true;
    }

    public bool TryAddPackedCake()
    {
        if (!CanPackTruck || GlobalVariables.Instance == null)
        {
            return false;
        }

        CakesPacked++;
        Score++;

        if (CakesPacked >= TruckCapacity)
        {
            CanPackTruck = false;
            IsTruckPacked = true;
        }

        return true;
    }

    public void ConsumeLife()
    {
        if (GlobalVariables.Instance != null)
        {
            GlobalVariables.Instance.Lives--;
        }
    }

    public void ClearCurrentLevel()
    {
        CurrentLevel = 0;
    }

    private int ResolveTruckCapacity()
    {
        if (gameplayConfig != null && gameplayConfig.truckCapacity > 0)
        {
            return gameplayConfig.truckCapacity;
        }

        return fallbackTruckCapacity;
    }
}
