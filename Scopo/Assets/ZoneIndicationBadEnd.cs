using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class ZoneEndingBad : MonoBehaviour
{
    [Header("UI & Transition")]
    [SerializeField] private GameObject textIndication;
    [SerializeField] private Image fader;
    [SerializeField] private Image endingDisplay;
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private float frameRate = 10f;

    [Header("Cinématiques")]
    [SerializeField] private Sprite[] animationFramesCommun;
    [SerializeField] private Sprite[] animationFramesBad;

    private bool playerInZone = false;
    private bool isEnding = false;

    private void Update()
    {
        if (playerInZone && !isEnding && Keyboard.current.xKey.wasPressedThisFrame)
        {
            StartCoroutine(PlayBadEndingSequence());
        }
    }

    private IEnumerator PlayBadEndingSequence()
    {
        isEnding = true;
        textIndication.SetActive(false);

        yield return StartCoroutine(Fade(0f, 1f));

        endingDisplay.color = Color.white;
        yield return StartCoroutine(PlayFrames(animationFramesCommun));

        yield return StartCoroutine(PlayFrames(animationFramesBad));

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            Color c = endingDisplay.color;
            c.a = 1f - t;
            endingDisplay.color = c;
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        Debug.Log("QUITTER LE JEU");
        Application.Quit();
    }

    private IEnumerator PlayFrames(Sprite[] frames)
    {
        float delay = 1f / frameRate;
        for (int i = 0; i < frames.Length; i++)
        {
            endingDisplay.sprite = frames[i];
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            Color c = fader.color;
            c.a = Mathf.Lerp(startAlpha, endAlpha, t / fadeDuration);
            fader.color = c;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isEnding)
            textIndication.SetActive(true);
            playerInZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            textIndication.SetActive(false);
            playerInZone = false;
    }
}