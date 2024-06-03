using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    private Animator animator; // Referencia al componente Animator
    private Vector2 lastMoveDirection;

    //Para cuando se agregue el sonido de los pasos
    //public AudioSource footstepSound; // Referencia al AudioSource para los pasos
    //public AudioClip footstepClip; // Referencia al AudioClip de los pasos
    //private float footstepDelay = 0.35f; // Intervalo entre sonidos de pasos
    //private float nextFootstepTime = 0f; // Control del tiempo para el próximo sonido de paso

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Obtenemos el componente Animator
    }

    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveInput.Normalize();
        moveVelocity = moveInput * speed;

        // Actualizamos los parámetros del Animator
        animator.SetFloat("Horizontal", moveInput.x);
        animator.SetFloat("Vertical", moveInput.y);
        animator.SetFloat("Speed", moveVelocity.magnitude);

        if (moveInput != Vector2.zero)
        {
            moveInput.Normalize();
            animator.SetFloat("UltimoX", moveInput.x);
            animator.SetFloat("UltimoY", moveInput.y);

            // Reproduce el sonido de pasos si es el momento adecuado
            //if (Time.time >= nextFootstepTime)
            //{
            //    footstepSound.clip = footstepClip;
              //  footstepSound.Play();
                //nextFootstepTime = Time.time + footstepDelay;
            //}
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }
}
