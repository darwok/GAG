using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ZombieEnemy : EnemyBase
{
    [Header("Movimiento")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float checkDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;

    [Header("Agro")]
    [SerializeField] private Transform player;
    [SerializeField] private float agroRange = 6f;

    private Rigidbody2D rb;
    private int dir = -1;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    protected override void Update()
    {
        base.Update();
        if (IsDead) return;

        bool hasGround = Physics2D.Raycast(groundCheck.position, Vector2.down, checkDistance, groundMask);
        bool wallAhead = Physics2D.Raycast(wallCheck.position, Vector2.right * dir, checkDistance, groundMask);

        if (player)
        {
            float dx = player.position.x - transform.position.x;
            if (Mathf.Abs(dx) <= agroRange) dir = dx > 0 ? 1 : -1;
        }

        if (!hasGround || wallAhead) dir *= -1;

        var v = rb.linearVelocity;
        v.x = dir * walkSpeed;
        rb.linearVelocity = v;

        var s = transform.localScale;
        s.x = Mathf.Abs(s.x) * dir;
        transform.localScale = s;
        // TODO: anim "Walk"
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck)
            Debug.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * checkDistance, Color.green);
        if (wallCheck)
            Debug.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * dir * checkDistance, Color.red);
    }
}
