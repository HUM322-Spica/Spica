using UnityEngine;

public class ZoneCameraTrigger : MonoBehaviour
{
    [SerializeField] private TVGazeController tvController; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && tvController != null)
        {
            tvController.SetPlayerInZone(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && tvController != null)
        {
            tvController.SetPlayerInZone(false);
        }
    }
}