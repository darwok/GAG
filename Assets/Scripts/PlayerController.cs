using DG.Tweening.Core.Easing;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 8f;

    [Header("Comprobación de suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private Animator anim;

    public bool blockInput;
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
        if (blockInput) return;

        // Entrada horizontal
        moveInput = Input.GetAxisRaw("Horizontal");

        // Saltar solo si está en el suelo
        if (Input.GetButtonDown("Jump") && isGrounded)
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

    void FixedUpdate()
    {
        // Movimiento lateral
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        // Verificar si está tocando el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
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