using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class HpEnemy : HPManager
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public const string DEATH = "Death";
    public const int TRANSPARENT_LAYER = 1;
    public const float VANISH_TIME = 1;


    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (currHp <= 0)
        {
            onDeathEvent?.Invoke();
            RemoveEnemy();
        }
    }

    private void RemoveEnemy()
    {
        gameObject.layer = TRANSPARENT_LAYER;
        animator?.SetTrigger("Death");
        spriteRenderer.DOFade(0, VANISH_TIME).SetDelay(VANISH_TIME);
    }
}
