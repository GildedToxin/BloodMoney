using JetBrains.Annotations;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
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

    public CartBehavior CartBehavior;

    //other variables
    public Camera cam;
    Interact interact;

    [Header("Step-Up Stair Variables")]
    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepHight = 0.3f; //how high can they step up
    [SerializeField] float stepSmoth = 1f; //smooth the transition between one step and another

    private IPlayerLookTarget currentLookAt;


    public float sensitivityX;
    public float sensitivityY;


    private float xRotation;
    private float yRotation;

    public AudioSource audioSource;
    public List<AudioClip> walking;

    private void Start()
    {
        GameManager.Instance.Player = this;
        interact = gameObject.GetComponent<Interact>();
        rb = GetComponent<Rigidbody>();
        //rb.freezeRotation = true;
        canJump = true;
        sprinting = false;
        //cam = Camera.main;

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        audioSource.clip = walking[Random.Range(0, walking.Count)];
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !GameManager.Instance.isPaused && !FindAnyObjectByType<DialogueManager>().conversationStarted)
        {
            print(UnityEngine.Cursor.visible);
            Time.timeScale = 0f;
            GameManager.Instance.pauseMenu.gameObject.SetActive(true);
            GameManager.Instance.isPaused = true;
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
            UnityEngine.Cursor.visible = true;
            return;
        }else if(Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.isPaused)
        {
            Time.timeScale = 1f;
            GameManager.Instance.pauseMenu.gameObject.SetActive(false);
            GameManager.Instance.isPaused = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            print(UnityEngine.Cursor.visible);
            print(UnityEngine.Cursor.lockState);
            return;
        }
        if (GameManager.Instance.isInMiniGame)
        {
            return;
        }

        RaycastLookDirection();


        //grounded check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHight * 0.5f + 0.2f, Ground);

        if (!GameManager.Instance.isInMiniGame && !GameManager.Instance.isPaused)
            MyInput();

        SpeedControl();

        //bool for sprinting animation!!
        bool isSprinting = sprinting && grounded;

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
    }

    private void FixedUpdate()
    {


        if (GameManager.Instance.isInMiniGame || GameManager.Instance.isPaused)
        {

            print("returning");
            return;
        }
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
        if(FindAnyObjectByType<FirstDayManager>() != null && FindAnyObjectByType<FirstDayManager>().isShowingScreen && GameManager.Instance.currentDay == 0)
            return;
        if(FindAnyObjectByType<DialogueManager>() != null && (FindAnyObjectByType<DialogueManager>().conversationStarted  || FindAnyObjectByType<DialogueManager>().extraActive))
            return;
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

        // Vertical (camera)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal (player)
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, mouseX, 0f));


        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

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

    }

    private void MovePlayer()
    {
        if (FindAnyObjectByType<FirstDayManager>() != null && FindAnyObjectByType<FirstDayManager>().isShowingScreen && GameManager.Instance.currentDay == 0) // Prevents player from moving during first day tutorial screens
        {
            print("returning");
            return;
        }
        if (FindAnyObjectByType<DialogueManager>() != null && (FindAnyObjectByType<DialogueManager>().conversationStarted || FindAnyObjectByType<DialogueManager>().extraActive))
            return;

        //finds the movement direction
        movementDirection = transform.forward * verticalInput + transform.right * horizontalInput;

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

        if (!audioSource.isPlaying && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            audioSource.clip = walking[Random.Range(0, walking.Count)];
            audioSource.Play();
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

        int layerMask = Physics.DefaultRaycastLayers;

        if (GetComponent<HeldItem>().currentItem != null)
        {
            int ignoreLayer = LayerMask.NameToLayer("Organs");
            layerMask &= ~(1 << ignoreLayer);
        }

        if (Physics.Raycast(ray, out RaycastHit hit, 3.75f, layerMask, QueryTriggerInteraction.Ignore))
        {
            //print(hit.collider.gameObject);
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


}

[System.Serializable]
public struct PlayerSaveData
{
    // Create additional variables here to store player data
    public float testFloat;
}
