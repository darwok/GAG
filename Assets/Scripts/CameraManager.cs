using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineCamera currentCamera;
    public CinemachineBasicMultiChannelPerlin noise;
    public List<CinemachineCamera> cameraList;
    private float defaultAmplitude = 2.5f;
    private float defaultFrequency = 2;
    private float defaultDuration = 0.3f;
    private Coroutine shakeRoutine;

    public static CameraManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CamShake()
    {
        if (noise == null)
            return;
        if (shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
        }

        shakeRoutine = StartCoroutine(ShakeR());
    }

    private void StopShake()
    {
        if (noise == null)
            return;
        noise.AmplitudeGain = 0;
        noise.FrequencyGain = 0;
    }

    IEnumerator ShakeR()
    {
        noise.AmplitudeGain = defaultAmplitude;
        noise.FrequencyGain = defaultFrequency;
        yield return new WaitForSeconds(defaultDuration);
        StopShake();
    }
}
