using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IDamageable
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 8f;

    [Header("Doble salto")]
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float groundedRadius = 0.6f;
    private int jumpCounter;

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

    [Header("Ataque con empuje")] // can be disabled, useful for testing
    [SerializeField] private bool attackAddsLunge = true;
    [SerializeField] private float attackLungeForce = 250f;

    [Header("VFX/SFX opcionales")]
    [SerializeField] private GameObject hitVfx;
    private Animator anim;

    [Header("Control")]
    [SerializeField] private bool blockInput = false;

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
        if (blockInput) return;

        moveInput = Input.GetAxisRaw("Horizontal");
        anim?.SetFloat("MoveSpeed", Mathf.Abs(moveInput));

        if (moveInput != 0)
        {
            facing = moveInput > 0 ? 1 : -1;
            var s = transform.localScale;
            s.x = Mathf.Abs(s.x) * facing;
            transform.localScale = s;
        }

        if (Input.GetButtonDown("Jump"))
        {
            TryJump();
        }

        //if (Input.GetButtonDown("Jump") && isGrounded && !IsDead)
        //{
        //    var v = rb.linearVelocity;
        //    v.y = jumpForce;
        //    rb.linearVelocity = v;
        //    anim.SetTrigger("Jump");
        //}

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
        //anim.SetBool("Airborne", !isGrounded);
    }

    private void TryJump()
    {
        if (isGrounded && jumpCounter > 0) jumpCounter = 0;

        if (jumpCounter < maxJumps)
        {
            anim?.SetTrigger("Jump");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCounter++;
        }
        // if already jumped max times, do nothing, had to test it a few times
    }

    private void Attack()
    {
        anim?.SetTrigger("Attack");
        if (!projectilePrefab || !firePoint) return;
        var proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
        proj.SetHitMask(enemyMask);
        proj.Fire(new Vector2(facing, 0));
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

    public void BlockInput(bool value)
    {
        blockInput = value;
    }

    public void Die()
    {
        anim?.SetTrigger("Death");
        BlockInput(true);
        //rb.linearVelocity = Vector2.zero;
        //rb.simulated = false;
    }

    void OnDrawGizmosSelected()
    {
        if (!groundCheck) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
