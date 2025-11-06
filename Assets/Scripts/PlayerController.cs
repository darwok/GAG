using DG.Tweening.Core.Easing;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 8f;
    public bool blockInput;

    [Header("Comprobaci�n de suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    public float groundedRadius = 1;

    [Header("HP Management")]
    [SerializeField] private PlayerHP hp;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private Animator anim;

    private int jumpCounter;
    private bool facingRight = false;

    private const int MaxJumps = 2;

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

        anim?.SetFloat("MoveSpeed", Mathf.Abs(moveInput));

        // Saltar solo si est� en el suelo
        if (Input.GetButtonDown("Jump"))
        {
            jumpCounter++;

            if (jumpCounter >= MaxJumps) return;

            jumpCounter = Mathf.Clamp(jumpCounter, 0, MaxJumps - 1);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("Jump");
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

    private void GroundCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, groundedRadius, groundLayer);
        isGrounded = colliders.Length > 0;

        anim.SetBool("Airborne", !isGrounded);

        if (isGrounded)
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
                hp.RemoveHp(1);

                Vector2 direction = facingRight ? Vector2.left : Vector2.right;
                direction += Vector2.up;
                GetComponent<Rigidbody>().AddForce(direction * KNOCKBACK_FORCE);
                break;

            case "Hazard":
                hp.RemoveHp(hp.maxHp);
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