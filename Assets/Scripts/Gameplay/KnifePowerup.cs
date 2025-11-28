using UnityEngine;

public class KnifePowerup : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSfx;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var player = other.GetComponent<Player>();
        if (!player) return;

        player.EnableKnife(true);
        SfxPlayer.I?.PlayPickup(transform.position);
        if (pickupSfx) AudioSource.PlayClipAtPoint(pickupSfx, transform.position, 0.7f);
        Destroy(gameObject);
    }
}