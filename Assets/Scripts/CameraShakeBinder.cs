using UnityEngine;

public class CameraShakeBinder : MonoBehaviour
{
    [SerializeField] private float damageShake = 1.5f;
    [SerializeField] private float killShake = 0.8f;

    void OnEnable()
    {
        EventBus.OnPlayerDamaged += OnDamaged;
        EventBus.OnEnemyKilled += OnEnemyKilled;
    }
    void OnDisable()
    {
        EventBus.OnPlayerDamaged -= OnDamaged;
        EventBus.OnEnemyKilled -= OnEnemyKilled;
    }
    void OnDamaged(int amount, Vector2 _) => CameraShake.I?.Shake(damageShake);
    void OnEnemyKilled(GameObject _) => CameraShake.I?.Shake(killShake);
}
