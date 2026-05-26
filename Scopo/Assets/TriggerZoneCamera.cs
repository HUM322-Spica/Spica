using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ZoneCameraTrigger : MonoBehaviour
{
    [Header("Détection")]
    [SerializeField] private GameObject cameraObject; // L'objet Surveillance Camera

    [Header("Audio Settings (Télé)")]
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private AudioClip tvStaticScratch; 
    [SerializeField] private float soundDuration = 1.0f;

    [Header("Événements de la Télé")]
    [SerializeField] private UnityEvent OnPlayerLookAtCameraInZone;
    [SerializeField] private UnityEvent OnPlayerStopLookingOrLeftZone;

    private bool isPlayerInZone = false;
    private bool isWatchingCamera = false;
    private Coroutine audioCoroutine;

    // Cette fonction tourne en boucle tant que le joueur est dans le cube invisible
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && cameraObject != null)
        {
            isPlayerInZone = true;

            // Astuce : On vérifie si l'objet de surbrillance (highlight) de la caméra est actif.
            // Si le système de Raycast global fait réagir la caméra, le script d'origine 
            // active l'objet 'highlight' ou change l'état interne.
            // Pour contourner de manière 100% fiable, on écoute si le joueur regarde l'objet.
        }
    }

    // --- LE CONTOURNEMENT DIRECTS VIA INSPECTOR ---
    // Ces fonctions ont maintenant un paramètre booléen exigé par l'évenement de la caméra !
    public void OnGazeChanged(bool isLooking)
    {
        if (isLooking && isPlayerInZone && !isWatchingCamera)
        {
            isWatchingCamera = true;
            OnPlayerLookAtCameraInZone?.Invoke();
            PlayStaticNoise();
        }
        else if (!isLooking && isWatchingCamera)
        {
            isWatchingCamera = false;
            OnPlayerStopLookingOrLeftZone?.Invoke();
            StopStaticNoise();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            if (isWatchingCamera)
            {
                isWatchingCamera = false;
                
                // CORRECTION ICI : On prévient l'UnityEvent de la télé de se réactiver !
                OnPlayerStopLookingOrLeftZone?.Invoke(); 
                
                StopStaticNoise();
            }
        }
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