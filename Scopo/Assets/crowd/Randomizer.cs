using UnityEngine;

public class Randomizer : MonoBehaviour
{
    void Start()
    {
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            // This works for Option 2 (The Parameter method)
            anim.SetFloat("MyOffset", Random.value);
            
            // This also adds a tiny bit of speed variety so they don't sync back up
            anim.speed = Random.Range(0.8f, 1.2f);
        }
    }
}