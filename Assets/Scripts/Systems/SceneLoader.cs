using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public const string SCENE_MAINMENU = "S_MainMenu";
    public const string SCENE_GAME = "S_Game";
    public const string SCENE_VICTORY = "S_Victory";
    public const string SCENE_GAMEOVER = "S_GameOver";

    public static void LoadMainMenu() => SceneManager.LoadScene(SCENE_MAINMENU);
    public static void LoadGame() => SceneManager.LoadScene(SCENE_GAME);
    public static void LoadVictory() => SceneManager.LoadScene(SCENE_VICTORY);
    public static void LoadGameOver() => SceneManager.LoadScene(SCENE_GAMEOVER);
}