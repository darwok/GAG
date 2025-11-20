using UnityEngine;

public class CrossbowPowerup : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSfx;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var pc = other.GetComponent<Player>();
        if (!pc) return;

        pc.EnableCrossbow(true);
        SfxPlayer.I?.PlayPickup(transform.position);
        if (pickupSfx) AudioSource.PlayClipAtPoint(pickupSfx, transform.position);
        Destroy(gameObject);
    }
}