/*using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public KeyCode pauseKey = KeyCode.Escape;
    private bool gamePaused;
    public CanvasGroup pauseScreen;
    private Tween pauseTween;
    private float pauseTweenTime = 0.5f;
    public static TimeManager instance;
    private Coroutine freezeFrameRotine;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Game starts unpaused, just in case...
        gamePaused = false;
        SetTimescale(1f);

        if (pauseScreen != null)
        {
            pauseScreen.alpha = 0f;
            pauseScreen.interactable = false;
            pauseScreen.blocksRaycasts = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause(!gamePaused);
        }
    }

    public void SetTimescale(float value)
    {
        Time.timeScale = value;
    }

    public void TogglePause(bool paused)
    {
        if (pauseScreen == null)
        {
            Debug.LogWarning("TimeManager: pauseScreen no asignado.");
            return;
        }

        if (freezeFrameRotine != null)
        {
            StopCoroutine(freezeFrameRotine);
        }

        gamePaused = paused;
        pauseTween?.Kill();

        if (paused)
        {
            // Show pause menu
            pauseScreen.interactable = true;
            pauseScreen.blocksRaycasts = true;

            SetTimescale(0f);
            pauseTween = pauseScreen
                .DOFade(1f, pauseTweenTime)
                .SetUpdate(true);
        }
        else
        {
            // Hide pause menu
            pauseTween = pauseScreen
                .DOFade(0f, pauseTweenTime)
                .OnComplete(() =>
                {
                    pauseScreen.interactable = false;
                    pauseScreen.blocksRaycasts = false;
                    SetTimescale(1f);
                })
                .SetUpdate(true);
        }
    }

    public void FreezeFrame(float timeScale, float duration)
    {
        if (freezeFrameRotine != null)
        {
            StopCoroutine(freezeFrameRotine);
        }
        freezeFrameRotine = StartCoroutine(FreezeFrameRoutine(timeScale, duration));
    }

    IEnumerator FreezeFrameRoutine(float timeScale, float duration)
    {
        SetTimescale(timeScale);
        yield return new WaitForSecondsRealtime(duration);
        SetTimescale(1f);
    }
}*/

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public KeyCode pauseKey = KeyCode.Escape;

    public CanvasGroup pauseScreen;
    public static TimeManager instance;

    private bool gamePaused;
    private Tween pauseTween;
    private float pauseTweenTime = 0.5f;
    private Coroutine freezeFrameRotine;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        // DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Siempre que llegue a esta escena, que arranque despausado
        gamePaused = false;
        SetTimescale(1f);

        if (pauseScreen != null)
        {
            pauseScreen.alpha = 0f;
            pauseScreen.interactable = false;
            pauseScreen.blocksRaycasts = false;
            pauseScreen.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("TimeManager: pauseScreen no asignado en el inspector.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause(!gamePaused);
        }
    }

    public void SetTimescale(float value)
    {
        Time.timeScale = value;
    }

    public void TogglePause(bool paused)
    {
        if (pauseScreen == null)
        {
            Debug.LogWarning("TimeManager: pauseScreen no asignado, no puedo pausar.");
            return;
        }

        if (freezeFrameRotine != null)
        {
            StopCoroutine(freezeFrameRotine);
        }

        gamePaused = paused;
        pauseTween?.Kill();

        if (paused)
        {
            // Encender el menú de pausa
            pauseScreen.gameObject.SetActive(true);
            pauseScreen.interactable = true;
            pauseScreen.blocksRaycasts = true;

            SetTimescale(0f);
            pauseTween = pauseScreen
                .DOFade(1f, pauseTweenTime)
                .SetUpdate(true);
        }
        else
        {
            // Ocultar el menú de pausa
            pauseScreen.interactable = false;
            pauseScreen.blocksRaycasts = false;

            pauseTween = pauseScreen
                .DOFade(0f, pauseTweenTime)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    pauseScreen.gameObject.SetActive(false);
                    SetTimescale(1f);
                });
        }
    }

    // Para conectar fácilmente el botón Resume desde el inspector
    public void ResumeFromButton()
    {
        TogglePause(false);
    }

    public void FreezeFrame(float timeScale, float duration)
    {
        if (freezeFrameRotine != null)
        {
            StopCoroutine(freezeFrameRotine);
        }
        freezeFrameRotine = StartCoroutine(FreezeFrameRoutine(timeScale, duration));
    }

    IEnumerator FreezeFrameRoutine(float timeScale, float duration)
    {
        SetTimescale(timeScale);
        yield return new WaitForSecondsRealtime(duration);
        SetTimescale(1);
    }
}
