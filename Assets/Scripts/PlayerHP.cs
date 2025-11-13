using UnityEngine;
using UnityEngine.Events;

public class PlayerHP : HPManager
{
    [SerializeField, Space(10)]
    private UnityEvent<int> onStartEvent, onModifyHpEvent;
    public Player player;

    [SerializeField, Space(10)]
    private UnityEvent onDeathEvent;

    protected override void Start()
    {
        base.Start();
        onStartEvent?.Invoke(maxHp);
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        onModifyHpEvent?.Invoke(currHp);
        CameraManager.instance?.CamShake();
        if (currHp <= 0)
        {
            player?.Die();
            gameManager?.GameOver();
            onDeathEvent?.Invoke();
        }
    }
}
