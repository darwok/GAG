using UnityEngine;

public class Breakable : MonoBehaviour, IDamageable
{
    [SerializeField] private int hp = 1;

    public bool IsDead => hp <= 0;

    public void TakeDamage(int amount, Vector2 hitPoint, Vector2 hitNormal)
    {
        if (IsDead) return;
        hp -= Mathf.Max(1, amount);
        if (hp <= 0)
        {
            EventBus.OnObstacleBroken?.Invoke(hitPoint);
            Destroy(gameObject);
        }
    }
}