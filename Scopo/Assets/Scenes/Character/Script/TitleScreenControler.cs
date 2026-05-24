using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TitleScreenControler : MonoBehaviour
{
    [Header("TitleScreen Settings")]
    [SerializeField] private GameObject titleCanvas;
    [SerializeField] private Image titleImage;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Frame Animation Settings")]
    [SerializeField] private Image animationImage;
    [SerializeField] private Sprite[] animationFrames;
    [SerializeField] private float frameRate = 11f;
    [SerializeField] private float animationFadeDuration = 1f;

    IEnumerator Start()
    {
        titleCanvas.SetActive(true);

        SetAlpha(1f);

        // Hide animation image at start
        if (animationImage != null)
        {
            SetAnimationAlpha(0f);
            animationImage.gameObject.SetActive(false);
        }

        // Wait for player to press space
        yield return new WaitUntil(() =>
            Keyboard.current.spaceKey.wasPressedThisFrame
        );

        // fade out title
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            float alpha = 1f - (t / fadeDuration);
            SetAlpha(alpha);

            yield return null;
        }

        SetAlpha(0f);

        titleCanvas.SetActive(false);

        // Play frame animation
        if (animationImage != null && animationFrames != null && animationFrames.Length > 0)
        {
            animationImage.gameObject.SetActive(true);
            SetAnimationAlpha(1f);
            yield return StartCoroutine(PlayFrameAnimation());
            // Fade out the animation image
            t = 0f;
            while (t < animationFadeDuration)
            {
                t += Time.deltaTime;
                float alpha = 1f - (t / animationFadeDuration);
                SetAnimationAlpha(alpha);
                yield return null;
            }

            SetAnimationAlpha(0f);
            animationImage.gameObject.SetActive(false);
        }

    }

    IEnumerator PlayFrameAnimation()
    {
        float delay = 1f / frameRate;

        for (int i = 0; i < animationFrames.Length; i++)
        {
            animationImage.sprite = animationFrames[i];
            yield return new WaitForSeconds(delay);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Helper method to set the alpha of the title image
    void SetAlpha(float alpha)
    {
        Color c = titleImage.color;
        c.a = alpha;
        titleImage.color = c;
    }

    void SetAnimationAlpha(float alpha)
    {
        Color c = animationImage.color;
        c.a = alpha;
        animationImage.color = c;
    }
}
