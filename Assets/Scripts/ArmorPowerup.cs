using UnityEngine;

public class ArmorPowerup : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSfx;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var pc = other.GetComponent<Player>();
        if (!pc) return;

        pc.SetArmor(true);
        if (pickupSfx) AudioSource.PlayClipAtPoint(pickupSfx, transform.position, 0.7f);
        Destroy(gameObject);
    }
}