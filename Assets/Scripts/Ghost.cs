using UnityEngine;

public class Ghost : EnemyBase
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 1.6f;
    [SerializeField] private float bobAmplitude = 0.3f;
    [SerializeField] private float bobFrequency = 1.2f;
    [SerializeField] private float agroRange = 8f;

    private Transform player;
    private float t0;
    private Animator anim;

    protected override void Awake()
    {
        base.Awake();
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;
        t0 = Random.value * 10f;
    }

    protected override void Update()
    {
        base.Update();
        if (IsDead || !player) return;

        Vector2 pos = transform.position;
        float d = Vector2.Distance(pos, player.position);
        if (d > agroRange) return;

        Vector2 dir = (player.position - (Vector3)pos).normalized;
        float bob = Mathf.Sin((Time.time + t0) * bobFrequency) * bobAmplitude;
        Vector2 vel = dir * moveSpeed + Vector2.up * bob;
        transform.position = pos + vel * Time.deltaTime;

        var s = transform.localScale;
        s.x = Mathf.Abs(s.x) * (dir.x >= 0 ? 1 : -1);
        transform.localScale = s;
        anim?.SetTrigger("Float");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var dmg) &&
            other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            dmg.TakeDamage(contactDamage, transform.position, Vector2.zero);
        }
    }
}
