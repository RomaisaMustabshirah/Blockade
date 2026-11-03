using UnityEngine;

public class GameOverSync : MonoBehaviour
{
    [Tooltip("Objects that appear at game over (RestartButton works well)")]
    public GameObject restartButton;

    [Tooltip("Objects that should follow it: GameOverBackground, MenuButton")]
    public GameObject[] followers;

    void Update()
    {
        bool show = restartButton != null && restartButton.activeInHierarchy;

        foreach (var go in followers)
        {
            if (go != null && go.activeSelf != show)
                go.SetActive(show);
        }
    }
}