using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Crow : EnemyBase
{
    [Header("Movimiento")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float diveSpeed = 5f;
    [SerializeField] private float patrolRange = 3f;
    [SerializeField] private float agroRangeX = 6f;
    [SerializeField] private float agroRangeY = 3f;

    private Rigidbody2D rb;
    private Vector2 origin;
    private int dir = 1;
    private Animator anim;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        origin = transform.position;
    }

    protected override void Update()
    {
        base.Update();
        if (IsDead) return;

        Transform player = FindPlayer();
        Vector2 pos = transform.position;

        bool inX = player && Mathf.Abs(player.position.x - pos.x) <= agroRangeX;
        bool inY = player && Mathf.Abs(player.position.y - pos.y) <= agroRangeY;

        if (inX && inY)
        {
            Vector2 dirTo = (player.position - transform.position).normalized;
            rb.linearVelocity = dirTo * diveSpeed;
            Flip(dirTo.x);
        }
        else
        {
            var v = new Vector2(dir * patrolSpeed, Mathf.Sin(Time.time * 2f) * 0.5f);
            rb.linearVelocity = v;

            if (Mathf.Abs(transform.position.x - origin.x) > patrolRange) dir *= -1;
            Flip(dir);
        }
        anim.SetTrigger("Fly");
    }

    private void Flip(float x)
    {
        var s = transform.localScale;
        s.x = Mathf.Abs(s.x) * (x >= 0 ? 1 : -1);
        transform.localScale = s;
    }

    private Transform FindPlayer()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        return p ? p.transform : null;
    }
}
