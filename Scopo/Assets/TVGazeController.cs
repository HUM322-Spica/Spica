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
        // Configuration initiale de base quand on lance le jeu :
        // Le joueur doit pouvoir se voir à l'écran au début.
        if (normalVideoObject != null) normalVideoObject.SetActive(false);
        if (cameraVideoObject != null) cameraVideoObject.SetActive(true);
    }

    // 1. Appelé par ton script de zone (ZoneCameraTrigger)
    public void SetPlayerInZone(bool inZone)
    {
        isPlayerInZone = inZone;

        // Si le joueur sort complètement de la zone (Zone_Tele), 
        // on réinitialise l'énigme pour qu'il puisse se re-voir s'il revient.
        if (!isPlayerInZone)
        {
            hasTriggeredSecret = false;
            if (normalVideoObject != null) normalVideoObject.SetActive(false);
            if (cameraVideoObject != null) cameraVideoObject.SetActive(true);
            StopStaticNoise();
        }
    }

    // 2. Appelé par la Caméra de surveillance (GazeInteraction d'origine)
    public void StartInteraction()
    {
        // L'action se déclenche SEULEMENT si le joueur est dans la zone 
        // ET que le secret n'a pas encore été activé.
        if (isPlayerInZone && !hasTriggeredSecret)
        {
            hasTriggeredSecret = true; // On verrouille l'action (One-shot tant qu'il reste dans la pièce)

            // LE BASCOULEMENT MAGIQUE :
            if (cameraVideoObject != null) cameraVideoObject.SetActive(false); // On coupe son propre reflet
            if (normalVideoObject != null) normalVideoObject.SetActive(true);   // On affiche la vidéo normale

            PlayStaticNoise(); // On joue le BZZZT sonore
        }
    }

    // On n'a plus besoin de rien faire dans StopInteraction ! 
    // Le changement reste bloqué même si le joueur arrête de regarder la caméra.
    public void StopInteraction() 
    {
        // On laisse vide volontairement pour maintenir la vidéo normale affichée !
    }

    // 3. Logique Audio standard
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