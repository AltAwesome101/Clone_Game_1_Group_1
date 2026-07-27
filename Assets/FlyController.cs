
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
public class FlyController : MonoBehaviour
{
    public float moveSpeed;
    public float maxFloatHeight = 10;
    public float minFloatHeight;

    public Camera freeLookCamera;
    private float currentHeight;
    private Animator anim;
    private float xRotation;
    //public InputActionReference moveAction;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHeight = transform.position.y;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        xRotation = freeLookCamera.transform.rotation.eulerAngles.x;

        if (Keyboard.current.wKey.isPressed)
        {
            MoveCharacter();
        }
        else
        {
            DisableMovement();
        }
        currentHeight = Mathf.Clamp(transform.position.y, currentHeight, maxFloatHeight);
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
    }

    private void MoveCharacter()
    {
        Vector3 cameraForward = new Vector3(freeLookCamera.transform.forward.x, 0, freeLookCamera.transform.forward.z);
        transform.rotation = Quaternion.LookRotation(cameraForward);
        transform.Rotate(new Vector3(0, 0, 0), Space.Self);

        anim.SetBool("isFlying", true);

        Vector3 forward = freeLookCamera.transform.forward;
        Vector3 flyDirection = forward.normalized;
        currentHeight += flyDirection.y * moveSpeed * Time.deltaTime;
        currentHeight = Mathf.Clamp(currentHeight, minFloatHeight, maxFloatHeight);

        transform.position += flyDirection * moveSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
    }

    private void DisableMovement()
    {
        anim.SetBool("isFlying", false);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

    }
}
