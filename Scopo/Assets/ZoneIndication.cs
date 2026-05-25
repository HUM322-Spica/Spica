using UnityEngine;
using TMPro;

public class ZoneIndication : MonoBehaviour
{
    [SerializeField] private GameObject textIndication;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textIndication.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textIndication.SetActive(false);
        }
    }
}