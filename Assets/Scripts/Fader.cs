using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public Image image;
    public float tweenTime;
    public UnityEvent onEndFadeEvent;
    public bool fadeOutOnStart;

    void Start()
    {
        if (fadeOutOnStart)
        {
            Fade(false);
        }
    }

    public void Fade(bool fadeIn)
    {
        float targetValue = fadeIn ? 1f : 0f;

        if (image.type == Image.Type.Filled)
        {
            image.DOFillAmount(targetValue, tweenTime).OnComplete(onEndFadeEvent.Invoke).SetDelay(2);
            return;
        }

        image.DOFade(targetValue, tweenTime).OnComplete(onEndFadeEvent.Invoke);
    }
}