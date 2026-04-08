using TMPro;
using UnityEngine;

public class GameplayHudPresenter : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text packedCakesText;
    [SerializeField] private TMP_Text levelText;

    public void Refresh(GameplaySessionState sessionState)
    {
        if (sessionState == null)
        {
            return;
        }

        SetText(ResolveLivesText(), sessionState.Lives.ToString());
        SetText(ResolvePackedCakesText(), sessionState.CakesPacked.ToString());
        SetText(ResolveLevelText(), sessionState.CurrentLevel.ToString());
        SetText(ResolveScoreText(), sessionState.Score.ToString());
    }

    public void InvalidateCache()
    {
        scoreText = null;
        livesText = null;
        packedCakesText = null;
        levelText = null;
    }

    private TMP_Text ResolveScoreText()
    {
        if (scoreText == null)
        {
            scoreText = FindText("ScoreText");
        }

        return scoreText;
    }

    private TMP_Text ResolveLivesText()
    {
        if (livesText == null)
        {
            livesText = FindText("Lives");
        }

        return livesText;
    }

    private TMP_Text ResolvePackedCakesText()
    {
        if (packedCakesText == null)
        {
            packedCakesText = FindText("PackedCakeText");
        }

        return packedCakesText;
    }

    private TMP_Text ResolveLevelText()
    {
        if (levelText == null)
        {
            levelText = FindText("LevelNumberText");
        }

        return levelText;
    }

    private TMP_Text FindText(string objectName)
    {
        GameObject target = GameObject.Find(objectName);
        return target != null ? target.GetComponent<TMP_Text>() : null;
    }

    private void SetText(TMP_Text textComponent, string value)
    {
        if (textComponent != null)
        {
            textComponent.text = value;
        }
    }
}
