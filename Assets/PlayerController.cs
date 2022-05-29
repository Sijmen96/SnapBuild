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
    [SerializeField] AnimationCurve springCurve;
    [Space]
    [SerializeField] private float cameraSmoothing;
    [SerializeField] private float cameraSensivity;

    private float playerSpeed;
    private float jumpVelocityMultiplier;
    private Vector3 cameraRotationTarget;
    private Transform cameraPivot;
    public PlayerMovementState movementState = PlayerMovementState.idle;
    public PlayerJumpState jumpState = PlayerJumpState.grounded;
    private bool grounded;
    private bool jumpKey;
    private bool jumpKeyDown;

    private float rayDistance = 1;

    RaycastHit hit;

    void Start()
    {
        cameraPivot = Instantiate(cameraPivotPrefab, transform.position, transform.rotation).transform;
        this.playerSpeed = this.walkingSpeed;
    }

    void Update()
    {
        receivePlayerInput();
        checkJumpState();

        Vector3 targetPosition = transform.position;
        if (this.jumpState == PlayerJumpState.grounded)
        {
            if (Physics.SphereCast(transform.position, 0.1f, Vector3.down, out hit, 1.5f))
            {
                targetPosition.y = hit.point.y + 1f;
            }
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 8);
        }
        //Debug.Log(transform.position.y + " " + (targetPosition.y) + " " + jumpState);
    }

    void FixedUpdate()
    {
        jumpHandler();

        playerDirectionVector = (cameraPivot.rotation * playerInputVector);
        playerDirectionVector.Normalize();
        playerDirectionVector = new Vector3(playerDirectionVector.x * playerSpeed, 0, playerDirectionVector.z * playerSpeed);

        //Player rotation
        if (playerDirectionVector != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(playerDirectionVector), Time.deltaTime * 10f);
        }

        if (this.jumpState != PlayerJumpState.grounded)
        {
            playerDirectionVector.y = playerRigidbody.velocity.y;
        }

        playerVelocityVector = Vector3.Lerp(playerRigidbody.velocity, playerDirectionVector, Time.fixedDeltaTime * velocityRamp);

        playerRigidbody.velocity = playerVelocityVector * jumpVelocityMultiplier;

        //Camera rotation
        cameraPivot.rotation = Quaternion.Lerp(cameraPivot.rotation, Quaternion.Euler(cameraRotationTarget), Time.fixedDeltaTime * cameraSmoothing);
        cameraPivot.position = Vector3.Lerp(cameraPivot.position, transform.position, Time.fixedDeltaTime * cameraSmoothing);
    }

    void checkJumpState()
    {
        //Determine jumpstate
        if (Physics.CheckSphere(transform.position + new Vector3(0, -.6f, 0), .5f, LayerMask.GetMask("Default")))
        {
            this.jumpState = PlayerJumpState.grounded;
            playerRigidbody.useGravity = false;
            this.jumpVelocityMultiplier = 1f;
        }
        else if (playerRigidbody.velocity.y > 0)
        {
            this.jumpState = PlayerJumpState.rising;
            playerRigidbody.useGravity = true;
            this.jumpVelocityMultiplier = 1f;
        }
        else if (playerRigidbody.velocity.y <= 0)
        {
            this.jumpState = PlayerJumpState.falling;
            playerRigidbody.useGravity = true;
            this.jumpVelocityMultiplier = 1f;
        }
    }

    void jumpHandler()
    {
        if (this.jumpState == PlayerJumpState.falling)
        {
            playerRigidbody.velocity += Vector3.up * Physics.gravity.y * jumpFallMultiplier * Time.deltaTime;
        }
        else if (this.jumpState == PlayerJumpState.rising && !jumpKey)
        {
            playerRigidbody.velocity += Vector3.up * Physics.gravity.y * jumpForceMultiplier * Time.deltaTime;
        }

        //Debug.Log(jumpKeyDown);
        if (this.jumpState == PlayerJumpState.grounded && jumpKeyDown)
        {
            jumpKeyDown = false;
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }


    void receivePlayerInput()
    {
        //Keybinding
        playerInputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        jumpKey = Input.GetKey(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyDown = true;
        }


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
        //Debug.Log(this.movementState);
    }



    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(hit.point, .3f);
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawSphere(this.transform.position, .3f);
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawSphere(transform.position + new Vector3(0, -.6f, 0), .5f);
    // }
}

public enum PlayerMovementState
{
    idle = 0, walking = 1, running = 2, crouching = 3, jumping = 4, blocked = 5, falling = 6
}

public enum PlayerJumpState
{
    rising = 0, falling = 1, grounded = 2
}

