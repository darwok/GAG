using UnityEngine;
using UnityEngine.Events;

public class HPManager : MonoBehaviour
{
    public int maxHp;
    protected int currHp;
    protected GameManager gameManager;
    public UnityEvent onDeathEvent;

    protected virtual void Start()
    {
        currHp = maxHp;
        gameManager = GameManager.instance;
    }

    public virtual void TakeDamage(int amount)
    {
        currHp -= amount;
        currHp = Mathf.Clamp(currHp, 0, maxHp);
    }

    public bool IsAlive()
    {
        return currHp > 0;
    }
}
