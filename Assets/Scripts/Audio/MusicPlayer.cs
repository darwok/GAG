using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer I { get; private set; }

    [SerializeField] private MusicBank bank;
    [SerializeField, Range(0f, 1f)] private float musicVolume = 0.8f;
    [SerializeField] private float crossfadeTime = 1.0f;

    private AudioSource a, b;
    private AudioSource current;

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        a = gameObject.AddComponent<AudioSource>();
        b = gameObject.AddComponent<AudioSource>();
        a.loop = b.loop = true;
        a.playOnAwake = b.playOnAwake = false;
        a.volume = b.volume = 0f;
        current = a;
    }

    public void SetBank(MusicBank m) => bank = m;

    public void PlayMenu() => Play(bank?.mainMenu);
    public void PlayGameplay() => Play(bank?.gameplay);
    public void PlayVictory() => Play(bank?.victory);
    public void PlayGameOver() => Play(bank?.gameOver);

    public void Play(AudioClip clip)
    {
        if (!clip) return;
        StopAllCoroutines();

        var next = current == a ? b : a;
        next.clip = clip;
        next.volume = 0f;
        next.Play();

        StartCoroutine(FadeSwap(current, next));
        current = next;
    }

    IEnumerator FadeSwap(AudioSource from, AudioSource to)
    {
        float t = 0f;
        while (t < crossfadeTime)
        {
            t += Time.unscaledDeltaTime;
            float k = crossfadeTime <= 0f ? 1f : Mathf.Clamp01(t / crossfadeTime);
            to.volume = musicVolume * k;
            from.volume = musicVolume * (1f - k);
            yield return null;
        }
        from.Stop();
        from.volume = 0f;
        to.volume = musicVolume;
    }
}