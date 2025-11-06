// Projectile.cs
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private LayerMask hitMask; // I can use it to set what layers the projectile can hit
    [SerializeField] private bool destroyOnHit = true;
    [SerializeField] private Vector2 direction = Vector2.right;

    private float _timer;

    public void Fire(Vector2 dir)
    {
        direction = dir.normalized;
        transform.right = direction; // change projectile orientation
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
        _timer += Time.deltaTime;
        if (_timer >= lifetime) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((hitMask.value & (1 << other.gameObject.layer)) == 0) return;

        if (other.TryGetComponent<IDamageable>(out var dmg))
        {
            // point and normal could be improved with collision data
            var point = (Vector2)transform.position;
            var normal = -direction;
            dmg.TakeDamage(damage, point, normal);
        }
        if (destroyOnHit) Destroy(gameObject);
    }
}