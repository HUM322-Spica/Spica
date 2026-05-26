using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterController characterController;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] footstepSounds;

    [Header("Safety Settings")]
    [SerializeField] private float speedThreshold = 0.1f;

    private void Start()
    {
        if (characterController == null)
        {
            characterController = GetComponentInParent<CharacterController>();
        }
    }

    public void PlayFootstepSound()
    {
        if (footstepSounds != null && footstepSounds.Length > 0 && audioSource != null)
        {
            if (characterController != null)
            {
                float horizontalSpeed = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude;

               
                if (horizontalSpeed < speedThreshold)
                {
                    return; 
                }
            }
            int randomIndex = Random.Range(0, footstepSounds.Length);
            AudioClip clip = footstepSounds[randomIndex];

            audioSource.PlayOneShot(clip);
        }
    }
}