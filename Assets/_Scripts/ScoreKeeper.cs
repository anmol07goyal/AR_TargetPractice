using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreTxt;
    [SerializeField] private int targetScore;

    private void OnEnable()
    {
        GameManager.OnScoreChanged += GameManager_OnScoreChanged;
    }

    private void GameManager_OnScoreChanged(int score)
    {
        scoreTxt.text = "<size=20>Score</size>\n" + score;
        if (score >= targetScore)
            scoreTxt.text = "<size=20>WAVE\nCLEARED</size>";
    }

    private void OnDisable()
    {
        GameManager.OnScoreChanged -= GameManager_OnScoreChanged;
    }
}
