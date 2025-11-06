using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject playerGameObject;
    private PlayerController player;
    public UnityEvent onGameWinEvents;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player = playerGameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        EndGame();
    }
    private void EndGame()
    {
        StartCoroutine(LSR(2, 1));
    }

    public void GameVictory()
    {
        onGameWinEvents?.Invoke();
        StartCoroutine(LSR(2, 0));
    }

    IEnumerator LSR(float waitTime, int sceneToLoad)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneToLoad);
    }
}
