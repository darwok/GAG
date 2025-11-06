using UnityEngine;
using UnityEngine.Events;

public class HPManager : MonoBehaviour
{
    public int maxHp;
    protected int currHp;
    protected GameManager gameManager;
    public UnityEvent onDeathEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currHp = maxHp;
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
