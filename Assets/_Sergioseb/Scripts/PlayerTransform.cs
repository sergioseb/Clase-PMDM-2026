using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTransform : MonoBehaviour, InputSystem_Actions.IRunnerControlActions
{
    [SerializeField] private float forwardSpeed = 8f;
    [SerializeField] private float lateralSpeed = 7f;
    [SerializeField] private float dashForce = 8f;
    [SerializeField] private float dashCooldown = 0.75f;
    private InputSystem_Actions inputActions;
    private float moveInput;
    private float dashCooldownTimer;
    private InputSystem_Actions.RunnerControlActions runnerActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        runnerActions = inputActions.RunnerControl;
        runnerActions.SetCallbacks(this);
    }

    private void OnEnable()
    {
        runnerActions.Enable();
    }

    private void OnDisable()
    {
        runnerActions.Disable();
    }

    private void OnDestroy()
    {
        runnerActions.RemoveCallbacks(this);
        inputActions.Dispose();
    }

    private void FixedUpdate()
    {
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.fixedDeltaTime;
        }

        Vector3 movement = new Vector3(moveInput * lateralSpeed, 0f, forwardSpeed) * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.performed || dashCooldownTimer > 0f)
        {
            return;
        }

        dashCooldownTimer = dashCooldown;
        transform.Translate(Vector3.forward * dashForce * Time.fixedDeltaTime, Space.World);
    }
}