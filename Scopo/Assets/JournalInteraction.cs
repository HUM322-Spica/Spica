using UnityEngine;

public class JournalInteraction : MonoBehaviour
{
    [Header("Réglages")]
    public float distanceRegard = 5f; // Distance à laquelle le joueur peut déclencher l'anim
    public string nomDuParametre = "IsLooking"; // Le nom exact dans ton Animator
    public string tagDuPNJ = "PNJ"; // Le tag que tu as mis sur ton bonhomme

    private Animator animatorActuel;

    void Update()
    {
        // On crée un rayon qui part du centre de la caméra
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // On dessine le rayon dans la fenêtre Scene pour t'aider (visible uniquement en mode Edit/Scene)
        Debug.DrawRay(ray.origin, ray.direction * distanceRegard, Color.red);

        if (Physics.Raycast(ray, out hit, distanceRegard))
        {
            // Est-ce qu'on touche un objet avec le bon Tag ?
            if (hit.collider.CompareTag(tagDuPNJ))
            {
                Animator anim = hit.collider.GetComponent<Animator>();

                if (anim != null)
                {
                    anim.SetBool(nomDuParametre, true);
                    animatorActuel = anim;
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
    }
}