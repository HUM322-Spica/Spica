using UnityEngine;

public class JournalInteraction : MonoBehaviour
{
    [Header("Interaction")]
    public float distanceRegard = 5f;
    public string nomDuParametre = "IsLooking";
    public string tagDuPNJ = "PNJ";

    private Animator animatorActuel;

    // Reference to the separate CameraShake script
    private CameraShake cameraShake;

    // Prevents shaking every frame
    private bool alreadyTriggered = false;

    void Start()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;

        Debug.DrawRay(ray.origin,
                      ray.direction * distanceRegard,
                      Color.red);

        if (Physics.Raycast(ray, out hit, distanceRegard))
        {
            if (hit.collider.CompareTag(tagDuPNJ))
            {
                Animator anim = hit.collider.GetComponent<Animator>();

                if (anim != null)
                {
                    anim.SetBool(nomDuParametre, true);
                    animatorActuel = anim;

                    cameraShake.Shake(2f, 0.2f);
                }
            }
            else
            {
                ArreterAnimation();
            }
        }
        else
        {
            ArreterAnimation();
        }
    }

    void ArreterAnimation()
    {
        if (animatorActuel != null)
        {
            animatorActuel.SetBool(nomDuParametre, false);
            animatorActuel = null;
        }

        alreadyTriggered = false;
    }
}