using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ZoneIndicationGoodEnd : MonoBehaviour
{
    [Header("UI & Transition")]
    [SerializeField] private Image fader;
    [SerializeField] private Image endingDisplay;
    [SerializeField] private float fadeDuration = 3.0f;
    [SerializeField] private float frameRate = 10f;
    [SerializeField] private Color finalScreenColor = Color.white;
    [Header("Cinématiques")]
    [SerializeField] private Sprite[] animationFramesCommun;
    [SerializeField] private Sprite[] animationFramesGood;

    private bool isEnding = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isEnding)
        {
            StartCoroutine(PlayGoodEndingSequence());
        }
    }

    private IEnumerator PlayGoodEndingSequence()
    {
        isEnding = true;

        
        fader.color = Color.black;
        yield return StartCoroutine(Fade(0f, 1f));

        endingDisplay.color = Color.white; 

        yield return StartCoroutine(PlayFrames(animationFramesCommun));

        yield return StartCoroutine(PlayFrames(animationFramesGood));

        fader.color = finalScreenColor; 
        
        float t = 0f;
        float finalFadeDuration = 1.5f;
        while (t < finalFadeDuration)
        {
            t += Time.deltaTime;
            float alpha = t / finalFadeDuration;

            Color cFader = fader.color;
            cFader.a = alpha;
            fader.color = cFader;

            yield return null;
        }

        Color finalC = fader.color;
        finalC.a = 1f;
        fader.color = finalC;
        endingDisplay.gameObject.SetActive(false);

        yield return new WaitForSeconds(3f);

        Debug.Log("QUITTER LE JEU (GOOD END - ECRAN BLANC)");
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
}