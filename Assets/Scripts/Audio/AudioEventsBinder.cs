using UnityEngine;

public class AudioEventsBinder : MonoBehaviour
{
    void OnEnable()
    {
        EventBus.OnPlayerJump += OnJump;
        EventBus.OnPlayerAttack += OnAttack;
        EventBus.OnPlayerDamaged += OnDamaged;
        EventBus.OnPlayerDied += OnDied;
        EventBus.OnEnemyKilled += OnEnemyKilled;
        EventBus.OnObstacleBroken += OnBreak;
    }

    void OnDisable()
    {
        EventBus.OnPlayerJump -= OnJump;
        EventBus.OnPlayerAttack -= OnAttack;
        EventBus.OnPlayerDamaged -= OnDamaged;
        EventBus.OnPlayerDied -= OnDied;
        EventBus.OnEnemyKilled -= OnEnemyKilled;
        EventBus.OnObstacleBroken -= OnBreak;
    }

    void OnJump() => AudioManager.I?.PlayJump(Vector3.zero);
    void OnAttack() => AudioManager.I?.PlayAttack(Vector3.zero);
    void OnDamaged(int a, Vector2 p) => AudioManager.I?.PlayHurt(p);
    void OnDied() => AudioManager.I?.PlayDeath(Vector3.zero);
    void OnEnemyKilled(GameObject g) => AudioManager.I?.PlayAttack(g ? g.transform.position : Vector3.zero);
    void OnBreak(Vector2 p) => AudioManager.I?.PlayObstacleBreak(p);
}