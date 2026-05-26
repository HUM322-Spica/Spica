using UnityEngine;
using System.Collections;

public class TVGazeController : MonoBehaviour
{
    [Header("Écrans de la Télé")]
    [SerializeField] private GameObject normalVideoObject; // L'objet 'video' (La vidéo normale)
    [SerializeField] private GameObject cameraVideoObject; // L'objet 'video camera' (Le retour de la caméra)

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private AudioClip tvStaticScratch; 
    [SerializeField] private float soundDuration = 1.0f;

    private bool isPlayerInZone = false;
    private bool hasTriggeredSecret = false; 
    private Coroutine audioCoroutine;

    private void Start()
    {
        if (normalVideoObject != null) normalVideoObject.SetActive(false);
        if (cameraVideoObject != null) cameraVideoObject.SetActive(true);
    }

    public void SetPlayerInZone(bool inZone)
    {
        isPlayerInZone = inZone;

        if (!isPlayerInZone)
        {
            hasTriggeredSecret = false;
            if (normalVideoObject != null) normalVideoObject.SetActive(false);
            if (cameraVideoObject != null) cameraVideoObject.SetActive(true);
            StopStaticNoise();
        }
    }

    public void StartInteraction()
    {
        if (isPlayerInZone && !hasTriggeredSecret)
        {
            hasTriggeredSecret = true;

            // LE BASCOULEMENT MAGIQUE :
            if (cameraVideoObject != null) cameraVideoObject.SetActive(false);
            if (normalVideoObject != null) normalVideoObject.SetActive(true);

            PlayStaticNoise();
        }
    }

    public void StopInteraction() 
    {
    }

    private void PlayStaticNoise()
    {
        if (audioSource != null && tvStaticScratch != null)
        {
            StopStaticNoise();
            audioSource.clip = tvStaticScratch;
            audioSource.Play();
            audioCoroutine = StartCoroutine(StopAudioAfterDelay(soundDuration));
        }
    }

    private void StopStaticNoise()
    {
        if (audioCoroutine != null)
        {
            StopCoroutine(audioCoroutine);
            audioCoroutine = null;
        }
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private IEnumerator StopAudioAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}