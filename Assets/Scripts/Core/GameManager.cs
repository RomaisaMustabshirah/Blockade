using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameOverText;
    public GameObject restartButton;

    public bool gameOver = false;

    void Awake()
    {
        Instance = this;

        gameOverText.SetActive(false);
        restartButton.SetActive(false);
    }

   public void GameOver()
{
    if (gameOver)
        return;

    gameOver = true;
    AudioManager.Instance.PlayGameOver();
    gameOverText.SetActive(true);
    restartButton.SetActive(true);

    Time.timeScale = 0f;
}
    public void RestartGame()
{
    AudioManager.Instance.PlayButton();
    Time.timeScale = 1f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
}