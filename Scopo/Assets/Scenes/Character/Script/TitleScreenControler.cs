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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        titleCanvas.SetActive(true);

        SetAlpha(1f);

        // Wait for player to press space
        yield return new WaitUntil(() =>
            Keyboard.current.spaceKey.wasPressedThisFrame
        );


        // fade out
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
}
