using UnityEngine;

public class SfxEventsListener : MonoBehaviour
{
    void OnEnable()
    {
        EventBus.OnPlayerJump += OnJump;
        EventBus.OnPlayerAttack += OnAttack;
        EventBus.OnPlayerDamaged += OnDamaged;
        EventBus.OnPlayerDied += OnDied;
        EventBus.OnEnemyKilled += OnEnemyKilled;
        EventBus.OnObstacleBroken += OnBreak;
        //EventBus.OnLevelWon += OnWin;
    }
    void OnDisable()
    {
        EventBus.OnPlayerJump -= OnJump;
        EventBus.OnPlayerAttack -= OnAttack;
        EventBus.OnPlayerDamaged -= OnDamaged;
        EventBus.OnPlayerDied -= OnDied;
        EventBus.OnEnemyKilled -= OnEnemyKilled;
        EventBus.OnObstacleBroken -= OnBreak;
        //EventBus.OnLevelWon -= OnWin;
    }

    void OnJump() => SfxPlayer.I?.PlayJump(Vector3.zero);
    void OnAttack() => SfxPlayer.I?.PlayAttack(Vector3.zero);
    void OnDamaged(int a, Vector2 p) => SfxPlayer.I?.PlayHurt(p);
    void OnDied() => SfxPlayer.I?.PlayDeath(Vector3.zero);
    void OnEnemyKilled(GameObject g) => SfxPlayer.I?.PlayAttack(g ? g.transform.position : Vector3.zero);
    void OnBreak(Vector2 p) => SfxPlayer.I?.PlayObstacleBreak(p);
    //void OnWin() => SfxPlayer.I?.PlayVictory(Vector3.zero);
}
