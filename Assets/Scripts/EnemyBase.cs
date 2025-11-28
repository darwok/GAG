using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    [SerializeField] protected int maxHealth = 2;
    [SerializeField] protected int contactDamage = 1;
    [SerializeField] protected float invulnTime = 0.2f;
    public Animator anim;
    public SoundList soundList;

    protected int health;
    protected float invulnTimer;

    public bool IsDead => health <= 0;

    protected virtual void Awake() { health = maxHealth; }

    protected virtual void Update()
    {
        if (invulnTimer > 0) invulnTimer -= Time.deltaTime;
    }

    public virtual void TakeDamage(int amount, Vector2 hitPoint, Vector2 hitNormal)
    {
        if (invulnTimer > 0 || IsDead) return;
        health -= Mathf.Max(1, amount);
        invulnTimer = invulnTime;
        // TODO: anim "Hurt"
        soundList.PlaySound("Hit");

        if (health <= 0) Die();
    }

    protected virtual void Die()
    {
        EventBus.OnEnemyKilled?.Invoke(gameObject);
        //anim.SetTrigger("Death");
        Destroy(gameObject, 0.1f);

    }

    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        TimeManager.instance?.FreezeFrame(0, 0.3f);
        if (col.collider.TryGetComponent<IDamageable>(out var dmg) &&
            col.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            dmg.TakeDamage(contactDamage, col.GetContact(0).point, col.GetContact(0).normal);
        }
    }
}
