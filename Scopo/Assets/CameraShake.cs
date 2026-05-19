using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;

    private float shakeTimer;

    void Start()
    {
        virtualCamera = FindObjectOfType<CinemachineCamera>();

        noise = virtualCamera
            .GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            if (shakeTimer <= 0)
            {
                noise.AmplitudeGain = 0f;
            }
        }
    }

    public void Shake(float intensity, float duration)
    {
        noise.AmplitudeGain = intensity;
        shakeTimer = duration;
    }
}