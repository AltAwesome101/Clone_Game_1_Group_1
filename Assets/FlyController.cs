using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class FlyController : MonoBehaviour
{
    public float moveSpeed;
    public float maxFloatHeight = 10;
    public float minFloatHeight;

    public Camera freeLookCamera;

    [Header("Jetpack Boost")]
    public float boostMultiplier = 2f;   // speed multiplier while boosting
    public float boostDuration = 3f;     // how long the boost lasts
    public float boostCooldown = 5f;     // time to refill the meter after use

    private enum BoostState { Ready, Boosting, Cooldown }
    private BoostState boostState = BoostState.Ready;
    private float boostTimer;
    private float cooldownTimer;
    private float boostMeter = 1f;
    private bool hasJetpack = false;

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

        HandleBoost();

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

    // Called by JetpackPickup when the player collides with a jetpack
    public void EquipJetpack()
    {
        hasJetpack = true;
    }

    private void HandleBoost()
    {
        // only allow starting a boost if the jetpack is equipped and the meter is ready
        if (hasJetpack && boostState == BoostState.Ready && Keyboard.current.shiftKey.wasPressedThisFrame)
        {
            boostState = BoostState.Boosting;
            boostTimer = boostDuration;
        }

        switch (boostState)
        {
            case BoostState.Boosting:
                boostTimer -= Time.deltaTime;
                boostMeter = boostTimer / boostDuration;

                if (boostTimer <= 0f)
                {
                    boostState = BoostState.Cooldown;
                    cooldownTimer = boostCooldown;
                }
                break;

            case BoostState.Cooldown:
                cooldownTimer -= Time.deltaTime;
                boostMeter = 1f - (cooldownTimer / boostCooldown);

                if (cooldownTimer <= 0f)
                {
                    boostState = BoostState.Ready;
                    boostMeter = 1f;
                }
                break;

            case BoostState.Ready:
                boostMeter = 1f;
                break;
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateBoostMeter(boostMeter, boostState == BoostState.Ready);
        }
    }

    private void MoveCharacter()
    {
        Vector3 cameraForward = new Vector3(freeLookCamera.transform.forward.x, 0, freeLookCamera.transform.forward.z);
        transform.rotation = Quaternion.LookRotation(cameraForward);
        transform.Rotate(new Vector3(0, 0, 0), Space.Self);

        anim.SetBool("isFlying", true);

        // apply the boost multiplier only while actively boosting
        float currentSpeed = moveSpeed * (boostState == BoostState.Boosting ? boostMultiplier : 1f);

        Vector3 forward = freeLookCamera.transform.forward;
        Vector3 flyDirection = forward.normalized;
        currentHeight += flyDirection.y * currentSpeed * Time.deltaTime;
        currentHeight = Mathf.Clamp(currentHeight, minFloatHeight, maxFloatHeight);

        transform.position += flyDirection * currentSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
    }

    private void DisableMovement()
    {
        anim.SetBool("isFlying", false);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

    }
}