using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake I { get; private set; }
    [SerializeField] private CinemachineImpulseSource impulse;

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        if (!impulse) impulse = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float amplitude = 1f)
    {
        if (!impulse) return;
        var noise = Random.insideUnitCircle.normalized; // Vector2 used here
        Vector3 velocity = new Vector3(noise.x, noise.y, 0) * amplitude; // Explicitly creating Vector3
        impulse.GenerateImpulse(velocity); // Using GenerateImpulse(Vector3) instead of m_DefaultVelocity
    }
}
