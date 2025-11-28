using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager I { get; private set; }

    [Header("SoundList (template)")]
    [SerializeField] private SoundList soundList;

    [Header("Nombres de música en SoundList")]
    [SerializeField] private string musicMenuName = "Menu";
    [SerializeField] private string musicGameplayName = "Gameplay";
    [SerializeField] private string musicVictoryName = "Victory";
    [SerializeField] private string musicGameOverName = "Gameover";

    [Header("Fade música")]
    [SerializeField] private float musicFadeTime = 1.0f;
    [SerializeField] private float musicFadeDelay = 0.0f;

    [Header("Nombres de SFX en SoundList")]
    [SerializeField] private string sfxJump = "Jump";
    [SerializeField] private string sfxAttack = "Attack";
    [SerializeField] private string sfxHurt = "Hurt";
    [SerializeField] private string sfxDeath = "Death";
    [SerializeField] private string sfxPickup = "SimpleShot";
    [SerializeField] private string sfxBreak = "Break";
    [SerializeField] private string sfxHit = "Hit";

    private string currentMusic;

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
        if (!soundList)
            soundList = GetComponent<SoundList>();
    }

    public void PlayMenuMusic() => PlayMusicByName(musicMenuName);
    public void PlayGameplayMusic() => PlayMusicByName(musicGameplayName);
    public void PlayVictoryMusic() => PlayMusicByName(musicVictoryName);
    public void PlayGameOverMusic() => PlayMusicByName(musicGameOverName);

    public void PlayMusicByName(string name)
    {
        if (!soundList || string.IsNullOrEmpty(name)) return;

        if (!string.IsNullOrEmpty(currentMusic) && currentMusic != name)
            soundList.StopSound(currentMusic);

        soundList.SoundFadeIn(name, musicFadeTime, musicFadeDelay);
        currentMusic = name;
    }

    // -------- SFX (por nombre) --------
    public void PlayJump(Vector3 _) => PlaySfxRandomPitch(sfxJump);
    public void PlayAttack(Vector3 _) => PlaySfxRandomPitch(sfxAttack);
    public void PlayHurt(Vector3 _) => PlaySfxRandomPitch(sfxHurt);
    public void PlayDeath(Vector3 _) => PlaySfxRandomPitch(sfxDeath);
    public void PlayPickup(Vector3 _) => PlaySfxRandomPitch(sfxPickup);
    public void PlayObstacleBreak(Vector3 _) => PlaySfxRandomPitch(sfxBreak);
    public void PlayHit(Vector3 _) => PlaySfxRandomPitch(sfxHit);

    private void PlaySfxRandomPitch(string name)
    {
        if (!soundList || string.IsNullOrEmpty(name)) return;
        soundList.PlaySoundRandomPitch(name);
    }
}