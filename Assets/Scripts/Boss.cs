using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Boss : MonoBehaviour, IDamageable
{
    [Header("Patrulla")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float arriveDistance = 0.1f;

    [Header("Combate")]
    [SerializeField] private int maxHealth = 8;
    [SerializeField] private int contactDamage = 1;
    [SerializeField] private float shootCooldown = 2.0f;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private string playerTag = "Player";

    [Header("Disparo horizontal")]
    [SerializeField] private float shotVerticalScatter = 0.5f;

    [Header("Detección de jugador")]
    [SerializeField] private float agroDistance = 6f;
    [SerializeField] private bool requiereLineaDeVision = false;

    [Header("Física")]
    [SerializeField] private float gravityScale = 4f;
    [SerializeField] private bool freezeRotationZ = true;

    [Header("Animación (opcional)")]
    [SerializeField] private Animator anim;

    private Rigidbody2D rb;
    private Transform player;

    private Vector2 patrolA;
    private Vector2 patrolB;
    private Vector2 currentTargetPos;
    private bool goingToA = true;

    private float shootTimer;
    private int health;
    private bool isDead;

    public bool IsDead => isDead;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = freezeRotationZ;
        rb.gravityScale = gravityScale;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        health = maxHealth;
    }

    void Start()
    {
        if (!anim) anim = GetComponent<Animator>();

        var playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj) player = playerObj.transform;

        if (pointA && pointB)
        {
            patrolA = pointA.position;
            patrolB = pointB.position;
            currentTargetPos = patrolA;
            goingToA = true;
        }
        else
        {
            // Si no hay puntos, se queda quieto... I mean... where else to go?
            patrolA = patrolB = transform.position;
            currentTargetPos = transform.position;
        }
    }

    void Update()
    {
        if (isDead) return;

        // Walk A <-> B
        float distToTarget = Vector2.Distance(transform.position, currentTargetPos);
        if (distToTarget <= arriveDistance)
        {
            goingToA = !goingToA;
            currentTargetPos = goingToA ? patrolA : patrolB;
        }

        // Disparo con agro
        shootTimer -= Time.deltaTime;
        if (player && shootTimer <= 0f && PlayerEnAgro())
        {
            ShootAtPlayer();
            shootTimer = shootCooldown;
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        Vector2 dir = (currentTargetPos - (Vector2)transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * speed, rb.linearVelocity.y);

        anim?.SetFloat("MoveSpeed", Mathf.Abs(rb.linearVelocity.x));
    }

    private bool PlayerEnAgro()
    {
        if (!player) return false;

        float distancia = Vector2.Distance(transform.position, player.position);
        if (distancia > agroDistance) return false;

        if (requiereLineaDeVision)
        {
            Vector2 origin = firePoint ? (Vector2)firePoint.position : (Vector2)transform.position;
            Vector2 dir = ((Vector2)player.position - origin).normalized;
            RaycastHit2D hit = Physics2D.Raycast(origin, dir, distancia, ~0);
            if (!hit.collider || hit.collider.transform != player) return false;
        }

        return true;
    }

    private void ShootAtPlayer()
    {
        if (!projectilePrefab || !firePoint || !player) return;

        float side = (player.position.x < transform.position.x) ? -1f : 1f;
        Vector2 dir = new Vector2(side, 0f);

        // Variar la altura del disparo
        float offsetY = Random.Range(-shotVerticalScatter, shotVerticalScatter);
        Vector3 spawnPos = firePoint.position + new Vector3(0f, offsetY, 0f);

        var proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        proj.gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
        proj.SetHitMask(playerMask);
        proj.Fire(dir);

        anim?.SetTrigger("Attack");
    }

    public void TakeDamage(int amount, Vector2 hitPoint, Vector2 hitNormal)
    {
        if (isDead) return;

        health -= Mathf.Max(1, amount);
        anim?.SetTrigger("Hit");

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        anim?.SetTrigger("Death");
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        EventBus.OnEnemyKilled?.Invoke(gameObject);
        Destroy(gameObject, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.collider.CompareTag(playerTag)) return;

        if (col.collider.TryGetComponent<IDamageable>(out var dmg))
        {
            var cp = col.GetContact(0);
            dmg.TakeDamage(contactDamage, cp.point, cp.normal);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (pointA && pointB)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawWireSphere(pointA.position, 0.1f);
            Gizmos.DrawWireSphere(pointB.position, 0.1f);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, agroDistance);
    }
}