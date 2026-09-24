using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, InputSystem_Actions.IRunnerControlActions
{
    [SerializeField] private float forwardSpeed = 8f;
    [SerializeField] private float lateralSpeed = 7f;

    [SerializeField] private float gravity = -20f;
    [SerializeField] private float jumpHeight = 2f;

    private InputSystem_Actions inputActions;
    private float horizontalInput;
    private bool isJumping;
    private InputSystem_Actions.RunnerControlActions runnerActions;
    private CharacterController controller;
    private float verticalVelocity;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        runnerActions = inputActions.RunnerControl;
        runnerActions.SetCallbacks(this);
        controller = GetComponent<CharacterController>();
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

    private void Update()
    {
        if (controller.isGrounded)
        {
            if (isJumping)
            {
                // v = sqrt(h * -2 * g) -> velocidad inicial necesaria para alcanzar jumpHeight
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            else if (verticalVelocity < 0f)
                verticalVelocity = -2f; // pequeño valor para mantenerlo pegado al suelo

            isJumping = false; // consumimos el salto para no repetirlo mientras se mantenga pulsado
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 movement = new Vector3(horizontalInput * lateralSpeed, verticalVelocity, forwardSpeed);
        controller.Move(movement * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        isJumping = context.performed;
    }
}