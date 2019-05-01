using UnityEngine;

public static class ScoreBucket
{
    public static int Score = 0;
    public static int HighScore = 0;

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
