using UnityEngine;

[CreateAssetMenu(menuName = "GAG/SFX Bank", fileName = "SfxBank")]
public class SfxBank : ScriptableObject
{
    [Header("Jugador")]
    public AudioClip jump;
    public AudioClip attackShoot;
    public AudioClip hurt;
    public AudioClip death;

    [Header("Mundo")]
    public AudioClip obstacleBreak;
    public AudioClip pickup;
}
