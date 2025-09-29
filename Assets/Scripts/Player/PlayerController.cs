using NUnit.Framework.Constraints;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintSpeed;
    public float crouchSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool canJump;

    private bool sprinting;
    private bool crouching;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.E;

    [Header("Ground Check")]
    public float playerHight;
    public LayerMask Ground;
    private bool grounded;

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 movementDirection;

    Rigidbody rb;

    //other variables
    public Camera cam;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        canJump = true;
        sprinting = false;
    }

    private void Update()
    {
        transform.rotation = cam.transform.rotation;

        //grounded check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHight * 0.5f + 0.2f, Ground);

        MyInput();
        SpeedControl();

        // player movement drag
        if(grounded)
        {
            // LinearDamping is the new term for Drag
            rb.linearDamping = groundDrag;
        } 
        else
        {
            rb.linearDamping = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //when player jumps
        if(Input.GetKey(jumpKey) && canJump && grounded && !crouching)
        {
            canJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown); // Calls reset jump with a delay of jumpCooldown so holding down the key keeps the player jumping
        }

        // when player sprints
        if (Input.GetKeyDown(sprintKey) && grounded && ! crouching)
        {
            sprinting = true;
        }
        if (Input.GetKeyUp(sprintKey) && grounded && !crouching)
        {
            sprinting = false;
        }

        //when player crouching
        if (Input.GetKeyDown(crouchKey) && grounded)
        {
            if (!crouching)
            {
                crouching = true;
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / 2, transform.localScale.z);
            }
            else if (crouching)
            {
                crouching = false;
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2, transform.localScale.z);
            }
        }    

    }

    private void MovePlayer()
    {
        //finds the movement direction
        movementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded && !sprinting && !crouching)
        {
            rb.AddForce(movementDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (grounded && sprinting && !crouching)
        {
            rb.AddForce(movementDirection.normalized * sprintSpeed * 10f, ForceMode.Force);

        }
        else if (grounded && !sprinting && crouching)
        {
            rb.AddForce(movementDirection.normalized * crouchSpeed * 10f, ForceMode.Force);
        }

        // in air
        else if (!grounded)
        {
            rb.AddForce(movementDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // limit velocity as needed
        if (flatVel.magnitude > moveSpeed && !sprinting && !crouching)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
        else if (flatVel.magnitude > sprintSpeed && sprinting && !crouching)
        {
            Vector3 limitedVel = flatVel.normalized * sprintSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
        else if (flatVel.magnitude > sprintSpeed && !sprinting && crouching)
        {
            Vector3 limitedVel = flatVel.normalized * crouchSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        //Reset Y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        canJump = true;
    }
}
