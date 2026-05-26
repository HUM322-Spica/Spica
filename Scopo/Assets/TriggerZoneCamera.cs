using UnityEngine;
using NarrationsJouables;

public class ZoneCameraTrigger : MonoBehaviour
{
    [SerializeField] private GazeInteraction cameraGazeScript; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraGazeScript.SetPlayerInZone(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraGazeScript.SetPlayerInZone(false);
        }
    }
}