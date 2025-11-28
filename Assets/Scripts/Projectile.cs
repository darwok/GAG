using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private LayerMask hitMask; // objetivo a golpear (Enemy o Player)
    [SerializeField] private bool destroyOnHit = true;

    private float timer;
    private Vector2 direction = Vector2.right;

    public void Fire(Vector2 dir)
    {
        direction = dir.normalized;
        transform.right = direction;
    }

    public void SetHitMask(LayerMask mask) => hitMask = mask;

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
        timer += Time.deltaTime;
        if (timer >= lifetime) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((hitMask.value & (1 << other.gameObject.layer)) == 0) return;

        if (other.TryGetComponent<IDamageable>(out var dmg))
        {
            var point = (Vector2)transform.position;
            var normal = -direction;
            dmg.TakeDamage(damage, point, normal);
        }
        if (destroyOnHit) gameObject.SetActive(false);
    }
}
