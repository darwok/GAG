using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    void OnEnable()
    {
        EventBus.OnPlayerDied += OnPlayerDied;
        EventBus.OnLevelWon += OnWin;
    }
    void OnDisable()
    {
        EventBus.OnPlayerDied -= OnPlayerDied;
        EventBus.OnLevelWon -= OnWin;
    }

    void OnPlayerDied() { Time.timeScale = 1f; SceneLoader.LoadGameOver(); }
    void OnWin() { Time.timeScale = 1f; SceneLoader.LoadVictory(); }
}