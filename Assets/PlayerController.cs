using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 playerInputVector;
    Vector3 playerDirectionVector;
    Vector3 playerVelocityVector;



    [SerializeField] GameObject cameraPivotPrefab;
    [SerializeField] Rigidbody playerRigidbody;
    [Space]
    [SerializeField] float walkingSpeed;
    [SerializeField] float runningSpeed;
    [SerializeField] float chrouchingSpeed;
    [SerializeField] float velocityRamp;
    [Space]
    [SerializeField] float jumpForce;
    [SerializeField] float jumpForceMultiplier;
    [SerializeField] float jumpFallMultiplier;
    [Space]
    [SerializeField] private float cameraSmoothing;
    [SerializeField] private float cameraSensivity;

    private float playerSpeed;
    private Vector3 cameraRotationTarget;
    private Transform cameraPivot;
    private PlayerMovementState movementState = PlayerMovementState.idle;
    private bool grounded;

    void Start()
    {
        cameraPivot = Instantiate(cameraPivotPrefab, transform.position, transform.rotation).transform;
        this.playerSpeed = this.walkingSpeed;
    }

    void Update()
    {
        receivePlayerInput();

        //Check if grounded
        grounded = Physics.CheckSphere(transform.position + new Vector3(0, -0.6f, 0), .5f, LayerMask.GetMask("Default"));

        if (grounded && this.movementState == PlayerMovementState.jumping)
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("jumping");
        }


    }

    void FixedUpdate()
    {
        playerDirectionVector = (cameraPivot.rotation * playerInputVector);
        playerDirectionVector.Normalize();
        playerDirectionVector = new Vector3(playerDirectionVector.x * playerSpeed, playerRigidbody.velocity.y, playerDirectionVector.z * playerSpeed);

        playerVelocityVector = Vector3.Lerp(playerRigidbody.velocity, playerDirectionVector, Time.deltaTime * velocityRamp);

        playerRigidbody.velocity = playerVelocityVector;

        //Camera rotation
        cameraPivot.rotation = Quaternion.Lerp(cameraPivot.rotation, Quaternion.Euler(cameraRotationTarget), Time.fixedDeltaTime * cameraSmoothing);
        cameraPivot.position = Vector3.Lerp(cameraPivot.position, transform.position, Time.fixedDeltaTime * cameraSmoothing);
    }

    void receivePlayerInput()
    {
        //Keybinding
        playerInputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.movementState = PlayerMovementState.jumping;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            this.playerSpeed = this.chrouchingSpeed;
            this.movementState = PlayerMovementState.crouching;
        }
        else if (playerInputVector == Vector3.zero)
        {
            this.movementState = PlayerMovementState.idle;
            this.playerSpeed = this.walkingSpeed;
        }
        else if (playerRigidbody.velocity.magnitude < 0.3f)
        {
            this.movementState = PlayerMovementState.blocked;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            this.playerSpeed = this.runningSpeed;
            this.movementState = PlayerMovementState.running;
        }
        else
        {
            this.playerSpeed = this.walkingSpeed;
            this.movementState = PlayerMovementState.walking;
        }

        //Camera controls
        if (Input.GetMouseButton(2))
        {
            cameraRotationTarget += new Vector3(0, Input.GetAxis("Mouse X") * cameraSensivity, 0);
        }
    }
}

public enum PlayerMovementState
{
    idle = 0, walking = 1, running = 2, crouching = 3, jumping = 4, blocked = 5
}