using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Animator weaponAxe;
    [SerializeField] Animator weaponFireaxe;
    [SerializeField] Animator animator;
    [SerializeField] CharacterController controller;

    [SerializeField] static bool gameOver;

    [SerializeField] public float walkSpeed = 5.5f;
    [SerializeField] public float runSpeed = 10f;
    [SerializeField] float gravity = -9.80f;
    [SerializeField] float jumpHeight = 3f;

    [SerializeField] bool isWalking = false;
    [SerializeField] bool isRunning = false;
    [SerializeField] bool weaponOnMove = false;

    public bool isCrouching { get; protected set; }

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;

    [SerializeField] float normalHeight;
    [SerializeField] float crouchHeight;
    [SerializeField] float crouchSpeed;

    Vector3 velocity;
    bool isGrounded;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        Cursor.visible = false;
        isWalking = false;
        isCrouching = false;
        weaponOnMove = false;

        gameOver = false;
    }
    private void Update()
    {
        //Regulation time of falling
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * runSpeed * Time.deltaTime); // ÑÈÑÒÅÌÀ ÏÅÐÅÄÂÈÆÅÍÈß

        isCrouching = Input.GetKey(KeyCode.LeftControl) && isGrounded;
        isWalking = Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && isGrounded && weaponOnMove;
        isRunning = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && isGrounded && weaponOnMove;

        if (isCrouching)
        {
            controller.height = crouchHeight;
            controller.Move(move * crouchSpeed * Time.deltaTime);
        }
        else if (isWalking)
        {
            animator.SetBool("isMoving", true);
            weaponOnMove = true;
            weaponAxe.SetBool("weaponOnMove", true);
            weaponFireaxe.SetBool("weaponOnMove", true);
            animator.SetFloat("speed", 0.5f);
            controller.Move(move * walkSpeed * Time.deltaTime);
            controller.height = normalHeight;
        }
        else if (isRunning)
        {
            animator.SetBool("isMoving", true);
            weaponOnMove = true;
            weaponAxe.SetBool("weaponOnMove", true);
            weaponFireaxe.SetBool("weaponOnMove", true);
            animator.SetFloat("speed", 1);
            controller.Move(move * runSpeed * Time.deltaTime);
            controller.height = normalHeight;
        }
        else
        {
            weaponAxe.SetBool("weaponOnMove", false);
            weaponFireaxe.SetBool("weaponOnMove", false);
            animator.SetBool("isMoving", false);
            animator.SetFloat("speed", 0);
            controller.Move(move * runSpeed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
