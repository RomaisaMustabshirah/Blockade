using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    int score;
    int highScore;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;
        scoreText.text = "Score: 0";
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;

        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = "High Score: " + highScore;

            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }
}