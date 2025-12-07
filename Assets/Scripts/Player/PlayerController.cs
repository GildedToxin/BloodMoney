using JetBrains.Annotations;
using NUnit.Framework.Constraints;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]

    public Animator RightHand;
    public Animator LeftHand;


    public float moveSpeed;
    public float sprintSpeed;
    public float crouchSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool canJump;

    private bool sprinting;
    private bool crouching;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode InventoryKey = KeyCode.G;

    [Header("Ground Check")]
    public float playerHight;
    public LayerMask Ground;
    public bool grounded;
    public float gravity;

    public Transform orientation;

    public float horizontalInput;
    public float verticalInput;

    private Vector3 movementDirection;

    Rigidbody rb;

    public float playerTestFloat;
    public HUDManager inventoryUIManager;

    public CartScript cartScript;

    //other variables
    public Camera cam;
    Interact interact;

    [Header("Step-Up Stair Variables")]
    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepHight = 0.3f; //how high can they step up
    [SerializeField] float stepSmoth = 0.1f; //smooth the transition between one step and another

    private IPlayerLookTarget currentLookAt;

    private void Awake()
    {
        //stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHight, stepRayUpper.transform.position.z); // makes it so it sets upper raycast to max step hight
    }

    private void Start()
    {
        GameManager.Instance.Player = this;
        interact = gameObject.GetComponent<Interact>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        canJump = true;
        sprinting = false;
        cam = Camera.main;
    }

    private void Update()
    {
        if (GameManager.Instance.isInMiniGame) 
        {             
            return;
        }
        RaycastLookDirection();
        //these arent used, but ill leave them here for now
        //cameraXRotation = cam.transform.rotation.x;
        //cameraZRotation = cam.transform.rotation.z;

        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, cam.transform.eulerAngles.y, transform.eulerAngles.z);



        //grounded check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHight * 0.5f + 0.2f, Ground);

        MyInput();
        SpeedControl();

        //bool for sprinting animation!!
        bool isSprinting = sprinting && grounded;
       // RightHand.SetBool("isSprinting", isSprinting);
        //LeftHand.SetBool("isSprinting", isSprinting);


        // player movement drag
        if (grounded)
        {
            // LinearDamping is the new term for Drag
            rb.linearDamping = groundDrag;
        } 
        else
        {
            rb.linearDamping = 0;
        }

/*
        if (Input.GetKey(KeyCode.J)){
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

            if (Physics.Raycast(ray, out RaycastHit hit, 10))
            {
                hit.collider.GetComponent<DecalProjector>().fadeFactor -= 0.01f;
            }
        }
*/
    }

    private void FixedUpdate()
    {


        if (GameManager.Instance.isInMiniGame)  
            return;

        MovePlayer();

        // if (!cartScript.isMoving)
        //   stepclimb();

        if (!grounded)
        {
            rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        }
    }
    public void Save(ref PlayerSaveData data)
    {
        // Save any player data here by setting variables in PlayerSaveData to current PlayerController Variables
        data.testFloat = playerTestFloat;
    }

    public void Load(PlayerSaveData data)
    {
        // Set variables in the player from the stored player save data here
        playerTestFloat = data.testFloat;
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //when player jumps
        if(Input.GetKey(jumpKey) && canJump)
        {
            canJump = false;

            Jump();
        }

        // when player sprints
        if (Input.GetKeyDown(sprintKey) && grounded && !crouching)
        {
            sprinting = true;
        }

        //when player stands from crouch
        if (Input.GetKeyUp(sprintKey) && grounded && !crouching)
        {
            sprinting = false;
        }

        //when player crouching
        if (Input.GetKeyDown(crouchKey) && grounded && interact.noInteraction)
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Invoke(nameof(ResetJump), jumpCooldown); // Calls reset jump with a delay of jumpCooldown so holding down the key keeps the player jumping
        }
    }
    private void ResetJump()
    {
        canJump = true;
    }

    public void RaycastLookDirection() // This is a bad name and should be changed to something better
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 50, Color.green);

        if (Physics.Raycast(ray, out RaycastHit hit, 10f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            IPlayerLookTarget lookable = hit.collider.GetComponent<IPlayerLookTarget>()
                          ?? hit.collider.GetComponentInParent<IPlayerLookTarget>();
            if (lookable != null)
            {
                // If looking at a new object, turn off the last one and turn on the new one
                if (currentLookAt != lookable) 
                {
                    currentLookAt?.OnLookExit();
                    currentLookAt = lookable;
                    currentLookAt.OnLookEnter();
                    //print(lookable.ToString());
                }
                return;
            }
        }

        // If nothing is hit, turn off the last object looked at
        if (currentLookAt != null)
        {
            currentLookAt.OnLookExit();
            currentLookAt = null;
        }
    }

    private void stepclimb()
    {
        RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.2f))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.3f))
            {
                rb.position -= new Vector3(0f, -stepSmoth, 0f);
            }
        }

        RaycastHit hitLower45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, 0.2f))
        {
            RaycastHit hitUpper45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, 0.3f))
            {
                rb.position -= new Vector3(0f, -stepSmoth, 0f);
            }
        }

        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, 0.2f))
        {
            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, 0.3f))
            {
                rb.position -= new Vector3(0f, -stepSmoth, 0f);
            }
        }
    }
}

[System.Serializable]
public struct PlayerSaveData
{
    // Create additional variables here to store player data
    public float testFloat;
}
