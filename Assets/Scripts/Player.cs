using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IDamageable
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 8f;

    [Header("Suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Combate")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float attackCooldown = 0.35f;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask enemyMask;

    [Header("VFX/SFX opcionales")]
    [SerializeField] private GameObject hitVfx;
    private Animator anim;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private float cooldown;
    private int health;
    private int facing = 1; // 1 der, -1 izq
    [SerializeField] private bool hasArmor = false;

    public bool IsDead => health <= 0;
    public int Health => health;
    public bool HasArmor => hasArmor;

    public System.Action<int, bool> UpdateHeartsUI;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        health = maxHealth;
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        anim?.SetFloat("MoveSpeed", Mathf.Abs(moveInput));

        if (moveInput != 0)
        {
            facing = moveInput > 0 ? 1 : -1;
            var s = transform.localScale;
            s.x = Mathf.Abs(s.x) * facing;
            transform.localScale = s;
        }

        if (Input.GetButtonDown("Jump") && isGrounded && !IsDead)
        {
            var v = rb.linearVelocity;
            v.y = jumpForce;
            rb.linearVelocity = v;
            anim.SetTrigger("Jump");
        }

        cooldown -= Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && cooldown <= 0f && !IsDead)
        {
            Attack();
            cooldown = attackCooldown;
        }
    }

    void FixedUpdate()
    {
        if (!IsDead)
        {
            var v = rb.linearVelocity;
            v.x = moveInput * speed;
            rb.linearVelocity = v;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        anim.SetBool("Airborne", !isGrounded);
    }

    private void Attack()
    {
        if (!projectilePrefab || !firePoint) return;
        var proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
        proj.SetHitMask(enemyMask);
        proj.Fire(new Vector2(facing, 0));
        anim?.SetTrigger("Attack");
    }

    public void SetArmor(bool value)
    {
        hasArmor = value;
        UpdateHeartsUI?.Invoke(health, hasArmor);
    }

    public void TakeDamage(int amount, Vector2 hitPoint, Vector2 hitNormal)
    {
        if (IsDead) return;

        if (hasArmor)
        {
            hasArmor = false;
            UpdateHeartsUI?.Invoke(health, hasArmor);
            EventBus.OnPlayerDamaged?.Invoke(0, hitPoint);
            return;
        }

        health -= Mathf.Max(1, amount);
        EventBus.OnPlayerDamaged?.Invoke(amount, hitPoint);
        if (hitVfx) Instantiate(hitVfx, hitPoint, Quaternion.identity);

        if (health <= 0) Die();
        else UpdateHeartsUI?.Invoke(health, hasArmor);
    }

    private void Die()
    {
        anim?.SetTrigger("Death");
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
    }

    void OnDrawGizmosSelected()
    {
        if (!groundCheck) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
