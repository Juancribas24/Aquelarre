using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovInputSystem : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    private Animator animator;
    private Vector2 moveInput;

    // Referencia al InputAction
    private PlayerInput playerInputActions;

    public bool canMove = true;

    void Awake()
    {
        playerInputActions = new PlayerInput();
    }

    void OnEnable()
    {
        playerInputActions.Player.Move.performed += OnMovePerformed;
        playerInputActions.Player.Move.canceled += OnMoveCanceled;
        playerInputActions.Player.Enable();
    }

    void OnDisable()
    {
        playerInputActions.Player.Move.performed -= OnMovePerformed;
        playerInputActions.Player.Move.canceled -= OnMoveCanceled;
        playerInputActions.Player.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Actualizamos los parámetros del Animator
        animator.SetFloat("Horizontal", moveInput.x);
        animator.SetFloat("Vertical", moveInput.y);
        animator.SetFloat("Speed", moveVelocity.magnitude);

        if (moveInput != Vector2.zero)
        {
            moveInput.Normalize();
            animator.SetFloat("UltimoX", moveInput.x);
            animator.SetFloat("UltimoY", moveInput.y);
        }
    }

    void FixedUpdate()
    {
        moveVelocity = moveInput * speed;
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            moveInput = context.ReadValue<Vector2>();
        }
    }

    void OnMoveCanceled(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            moveInput = Vector2.zero;
        }
    }
}
