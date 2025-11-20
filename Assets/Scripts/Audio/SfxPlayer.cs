using UnityEngine;

public class SfxPlayer : MonoBehaviour
{
    public static SfxPlayer I { get; private set; }
    [SerializeField] private SfxBank bank;
    [SerializeField] private float sfxVolume = 1f;

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayOneShot(AudioClip clip, Vector3 pos)
    {
        if (!clip) return;
        AudioSource.PlayClipAtPoint(clip, pos, sfxVolume);
    }

    // Helpers
    public void PlayJump(Vector3 pos) => PlayOneShot(bank?.jump, pos);
    public void PlayAttack(Vector3 pos) => PlayOneShot(bank?.attackShoot, pos);
    public void PlayHurt(Vector3 pos) => PlayOneShot(bank?.hurt, pos);
    public void PlayDeath(Vector3 pos) => PlayOneShot(bank?.death, pos);
    public void PlayPickup(Vector3 pos) => PlayOneShot(bank?.pickup, pos);
    public void PlayObstacleBreak(Vector3 p) => PlayOneShot(bank?.obstacleBreak, p);
    public void PlayVictory(Vector3 pos) => PlayOneShot(bank?.victory, pos);
}
