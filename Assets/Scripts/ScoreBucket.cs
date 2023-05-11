using UnityEngine;

public static class ScoreBucket
{
    public static int Score { get; private set; }
    public static int HighScore { get; private set; }

    private static int _lastScore = 0;

    public static void ResetScore()
    {
        Score = 0;
        _lastScore = 0;
    }

    public static void AddScore(int score)
    {
        Score += score;
        UpdateHighScore();
    }

    public static void BuyRocket(int cost)
    {
        Score -= cost;
        if (Score < 0)
        {
            Score = 0;
        }
        UpdateHighScore();
    }

    public static void CheckPoint()
    {
        _lastScore = Score;
    }

    public static void RestoreCheckPoint()
    {
        Score = _lastScore;
    }

    private static void UpdateHighScore()
    {
        if (Score > HighScore)
        {
            HighScore = Score;
        }
    }

    public static void SaveHighScore()
    {
        PlayerPrefs.SetInt("highscore", HighScore);
        PlayerPrefs.Save();
    }

    public static void LoadHighScore()
    {
        HighScore = PlayerPrefs.GetInt("highscore");
    }
}
