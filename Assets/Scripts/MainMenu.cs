using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private int gScene = 1;

    public void StartGame()
    {
        SceneManager.LoadScene(gScene);
    }
}
