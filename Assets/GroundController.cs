using UnityEngine;
using UnityEngine.InputSystem;

public class GroundController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -20f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundMask;

    public Camera freeLookCamera;
    public FlyController flyController; // assign the same GameObject's FlyController here
    public KeyCode flyToggleKey = KeyCode.F;

    private Animator anim;
    private float verticalVelocity;
    private bool isGrounded;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Toggle to fly mode
        if (Keyboard.current[Key.F].wasPressedThisFrame) // swap Key.F if you want flyToggleKey to be used dynamically
        {
            ToggleFlyMode();
            return;
        }

        GroundCheck();
        HandleMovement();
        ApplyGravity();
    }

    private void GroundCheck()
    {
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance + 0.1f, groundMask);
    }

    private void HandleMovement()
    {
        float h = 0f, v = 0f;
        if (Keyboard.current.wKey.isPressed) v += 1f;
        if (Keyboard.current.sKey.isPressed) v -= 1f;
        if (Keyboard.current.dKey.isPressed) h += 1f;
        if (Keyboard.current.aKey.isPressed) h -= 1f;

        bool isMoving = h != 0f || v != 0f;
        anim.SetBool("isWalking", isMoving);

        if (!isMoving) return;

        // Camera-relative movement, flattened to the ground plane
        Vector3 camForward = freeLookCamera.transform.forward;
        Vector3 camRight = freeLookCamera.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * v + camRight * h).normalized;

        transform.rotation = Quaternion.LookRotation(moveDir);
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void ApplyGravity()
    {
        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -1f; // small downward value to keep grounded
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        transform.position += Vector3.up * verticalVelocity * Time.deltaTime;
    }

    private void ToggleFlyMode()
    {
        anim.SetBool("isWalking", false);
        enabled = false;
        flyController.enabled = true;
    }
}