using UnityEngine;
using UnityEngine.InputSystem;

public class characterScript : MonoBehaviour {
    public Animator animator;

    void Update()
    {
        float moveX = 0f;
        float moveZ = 0f;

        // Horizontal
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            moveX = -1f;
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            moveX = 1f;

        // Vertical (forward/back)
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            moveZ = 1f;
        else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            moveZ = -1f;

        float speed = new Vector2(moveX, moveZ).magnitude;

        animator.SetBool("isWalking", speed > 0.1f);
    }
}
