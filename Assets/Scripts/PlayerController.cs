using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private int maxJumps = 2;
    private int jumpCounter;
    public bool blockInput;
    private float moveInput;
    private Rigidbody2D rb;

    [Header("Combate")]
    [SerializeField] private float attackCooldown = 0.35f;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask enemyMask;

    [Header("Comprobación de suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    public float groundedRadius = 1;
    private bool isGrounded;

    [Header("HP Management")]
    [SerializeField] private PlayerHP hp;
    private const float KNOCKBACK_FORCE = 150;

    private Animator anim;

    private bool facingRight = false;

    private const int MaxJumps = 2;

    private int facing = 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        GroundCheck();

        if (blockInput) return;

        if (Input.GetMouseButtonDown(0))
        {
            float force = facingRight ? 250 : -250;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(new Vector2(force, 0));
            Attack();
        }

        // Entrada horizontal
        moveInput = Input.GetAxisRaw("Horizontal");
        facing = moveInput > 0 ? 1 : -1;
        anim?.SetFloat("MoveSpeed", Mathf.Abs(moveInput));

        // Saltar solo si está en el suelo
        //if (Input.GetButtonDown("Jump"))
        //{
        //    jumpCounter++;

        //    if (jumpCounter >= MaxJumps) return;

        //    jumpCounter = Mathf.Clamp(jumpCounter, 0, MaxJumps - 1);
        //    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        //    anim.SetTrigger("Jump");
        //}
        if (Input.GetButtonDown("Jump"))
        {
            TryJump();
        }

        if (!facingRight && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight && moveInput < 0)
        {
            Flip();
        }
    }
    public void BlockInput(bool value)
    {
        blockInput = value;
    }

    private void TryJump()
    {
        if (isGrounded && jumpCounter > 0) jumpCounter = 0;

        if (jumpCounter < maxJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCounter++;
            anim?.SetTrigger("Jump");
        }
        // if already jumped max times, do nothing, had to test it a few times
    }

    private void GroundCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, groundedRadius, groundLayer);
        bool wasGrounded = isGrounded;
        isGrounded = colliders.Length > 0;

        anim.SetBool("Airborne", !isGrounded);

        if (isGrounded && !wasGrounded)
        {
            jumpCounter = 0;
        }

        if (isGrounded && jumpCounter != 0)
        {
            jumpCounter = 0;
        }
    }
    void FixedUpdate()
    {
        // Movimiento lateral
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        //// Verificar si est� tocando el suelo
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    public void Attack()
    {
        anim?.SetTrigger("Attack");
        if (!projectilePrefab || !firePoint) return;
        var proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
        // configura a quién golpea
        var mask = new LayerMask();
        mask.value = enemyMask.value;
        typeof(Projectile).GetField("hitMask", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(proj, mask);
        proj.Fire(new Vector2(facing, 0));
        // TODO: trigger de animación "Attack"
    }

    public void Death()
    {
        BlockInput(true);
        anim?.SetTrigger("Death");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (hp == null || !hp.IsAlive())
        {
            return;
        }

        switch (other.gameObject.tag)
        {
            case "Enemy":
                hp.TakeDamage(1);

                Vector2 direction = facingRight ? Vector2.left : Vector2.right;
                direction += Vector2.up;
                GetComponent<Rigidbody>().AddForce(direction * KNOCKBACK_FORCE);
                break;

            case "Hazard":
                hp.TakeDamage(hp.maxHp);
                GetComponent<Rigidbody>().AddForce(Vector3.up * KNOCKBACK_FORCE);
                break;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}