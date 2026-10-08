using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Movimiento usando Rigidbody.MovePosition en FixedUpdate.
// Recomendado cuando el Player necesita física (colisiones, fuerzas, rampas)
// gestionadas por el motor de físicas de Unity.
// Requiere un componente Rigidbody en el mismo GameObject
// (recomendable con "Is Kinematic" activado si no quieres que fuerzas externas lo muevan,
// o desactivado si quieres que reaccione a colisiones físicas).
[RequireComponent(typeof(Rigidbody))]
public class PlayerRigidbody : MonoBehaviour, InputSystem_Actions.IRunnerControlActions
{
    [SerializeField] private float forwardSpeed = 8f;
    [SerializeField] private float lateralSpeed = 7f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float dashForce = 12f;
    [SerializeField] private float dashCooldown = 0.75f;

    private InputSystem_Actions inputActions;
    private float horizontalInput;
    private bool isJumping;
    private bool isGrounded;
    private float dashCooldownTimer;
    private InputSystem_Actions.RunnerControlActions runnerActions;
    private Rigidbody rb;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        runnerActions = inputActions.RunnerControl;
        runnerActions.SetCallbacks(this);
        rb = GetComponent<Rigidbody>();
        InvokeRepeating(nameof(SpeedUp), 5f, 5f);
    }

    private void SpeedUp()
    {
        forwardSpeed += 0.5f;
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

        // Movimiento lateral y hacia adelante
        Vector3 movement = new Vector3(horizontalInput * lateralSpeed, 0f, forwardSpeed);
        Vector3 targetPosition = rb.position + movement * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);

        if (isJumping && isGrounded)
        {
            // Reseteamos la velocidad vertical antes de aplicar el impulso,
            // así el salto siempre tiene la misma fuerza independientemente
            // de si el jugador venía subiendo o bajando.
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            //rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }

        isJumping = false; // consumimos el salto para no repetirlo mientras se mantenga pulsado
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isJumping = true;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        rb.AddForce(new Vector3(horizontalInput, 0f, 0f) * dashForce, ForceMode.Impulse);
    }

    // Detectar si está en el suelo mediante colisiones con objetos etiquetados "Ground"
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            //Destroy(gameObject);
            Debug.Log("Colisión con obstáculo");
            GameManager.Instance.EndGame();
        }
    }
}