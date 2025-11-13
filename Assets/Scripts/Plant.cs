using UnityEngine;

public class Plant : EnemyBase
{
    [Header("Ataque")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 1.2f;
    [SerializeField] private float shootRange = 8f;
    [SerializeField] private LayerMask playerMask;

    private float timer;
    private Transform player;

    protected override void Awake()
    {
        base.Awake();
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj) player = playerObj.transform;
    }

    protected override void Update()
    {
        base.Update();
        if (IsDead || !player) return;

        int facing = player.position.x >= transform.position.x ? 1 : -1;
        var s = transform.localScale; s.x = Mathf.Abs(s.x) * facing; transform.localScale = s;

        timer -= Time.deltaTime;
        float dist = Vector2.Distance(player.position, transform.position);
        if (dist <= shootRange && timer <= 0f)
        {
            Shoot(facing);
            timer = shootCooldown;
        }
        anim?.SetTrigger("Attack");
    }

    private void Shoot(int facing)
    {
        if (!projectilePrefab || !firePoint) return;
        var proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
        proj.SetHitMask(playerMask);

        Vector2 dir = new Vector2(facing, 0.15f).normalized;
        proj.Fire(dir);
    }
}
