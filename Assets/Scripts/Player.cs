using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IDamageable
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 8f;

    [Header("Doble salto")]
    [SerializeField] private int maxJumps = 2;
    //[SerializeField] private float groundedRadius = 0.6f;
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

    [Header("Ataque con empuje")]
    [SerializeField] private bool attackAddsLunge = true;
    [SerializeField] private float attackLungeForce = 250f;

    [Header("VFX/SFX")]
    private Animator anim;
    public SoundList soundList;

    [Header("Control")]
    [SerializeField] private bool blockInput = false;

    [Header("Crossbow")]
    [SerializeField] private Projectile crossbowProjectilePrefab;
    [SerializeField] private bool hasCrossbow = false;
    [SerializeField] private float crossbowAttackCooldown = 0.22f;
    [SerializeField] private Transform altFirePointLeft;
    [SerializeField] private Transform altFirePointRight;

    [Header("Knife")]
    [SerializeField] private Projectile knifeProjectilePrefab;
    [SerializeField] private bool hasKnife = false;
    //[SerializeField] private float knifeAttackCooldown = 0.18f;

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

    void Start()
    {
        // ARREGLO 1: cachear Animator, esto me ayudó a corregir las animaciones
        anim = GetComponent<Animator>();
        if (!anim) anim = GetComponentInChildren<Animator>();
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
            soundList?.PlaySound("Jump");
        }

        cooldown -= Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && cooldown <= 0f && !IsDead)
        {
            Attack();
            cooldown = attackCooldown;
            Vector2 direction = facing == 1 ? Vector2.right : Vector2.left;
            GetComponent<Rigidbody2D>().AddForce(direction * attackLungeForce);
            soundList?.PlaySoundRandomPitch("Attack");
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

        // ARREGLO 2: actualizar Airborne para el Animator, me hacía falta según la lógica inicial del script original
        anim?.SetBool("Airborne", !isGrounded);
    }

    public void EnableKnife(bool value)
    {
        hasKnife = value;
        hasCrossbow = false;
    }

    public void EnableCrossbow(bool value)
    {
        hasCrossbow = value;
    }

    private void TryJump()
    {
        if (isGrounded && jumpCounter > 0) jumpCounter = 0;

        if (jumpCounter < maxJumps)
        {
            anim?.SetTrigger("Jump");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCounter++;
            EventBus.OnPlayerJump?.Invoke();
        }
    }

    public void Attack()
    {
        anim?.SetTrigger("Attack");
        EventBus.OnPlayerAttack?.Invoke();

        if (!firePoint) return;

        if (hasCrossbow && crossbowProjectilePrefab)
        {
            float[] angles = { 0f, 30f, -30f };
            foreach (float angle in angles)
            {
                var proj = Instantiate(crossbowProjectilePrefab, firePoint.position, Quaternion.identity);
                proj.gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
                proj.SetHitMask(enemyMask);
                Vector2 dir = Quaternion.Euler(0, 0, angle) * new Vector2(facing, 0);
                proj.Fire(dir.normalized);
            }
            return;
        }

        if (hasKnife && knifeProjectilePrefab)
        {
            var proj = Instantiate(knifeProjectilePrefab, firePoint.position, Quaternion.identity);
            proj.gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
            proj.SetHitMask(enemyMask);
            proj.Fire(new Vector2(facing, 0));
            return;
        }

        if (projectilePrefab)
        {
            var proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            proj.gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
            proj.SetHitMask(enemyMask);
            proj.Fire(new Vector2(facing, 0));
        }
    }

    public void SetArmor(bool value)
    {
        hasArmor = value;
        UpdateHeartsUI?.Invoke(health, hasArmor);
    }

    public void TakeDamage(int amount, Vector2 hitPoint, Vector2 hitNormal)
    {
        if (IsDead) return;

        soundList?.PlaySound("Hurt");

        if (hasArmor)
        {
            hasArmor = false;
            UpdateHeartsUI?.Invoke(health, hasArmor);
            EventBus.OnPlayerDamaged?.Invoke(0, hitPoint);
            return;
        }

        health -= Mathf.Max(1, amount);
        EventBus.OnPlayerDamaged?.Invoke(amount, hitPoint);

        if (health <= 0) Die();
        else UpdateHeartsUI?.Invoke(health, hasArmor);
    }

    public void BlockInput(bool value) => blockInput = value;

    public void Die()
    {
        anim?.SetTrigger("Death");
        BlockInput(true);
        EventBus.OnPlayerDied?.Invoke();
        soundList?.PlaySound("Death");
    }

    void OnDrawGizmosSelected()
    {
        if (!groundCheck) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
