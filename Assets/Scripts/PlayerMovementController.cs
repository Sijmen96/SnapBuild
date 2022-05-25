using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {

    private Vector3 playerInput;
    private Vector3 directionVector;
    private bool canJump;

    [SerializeField] private Transform CameraPivot;
    [SerializeField] private Rigidbody PlayerBody;
    [Space]
    [SerializeField] private float speed;
    [SerializeField] private float jumpforce;

    private void Update() {
        canJump = Physics.CheckSphere(transform.position + new Vector3(0,-0.6f,0), .5f, LayerMask.GetMask("Default"));
        if (canJump && Input.GetKeyDown(KeyCode.Space)) {
            PlayerBody.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        }

        if (PlayerBody.velocity.y < 0f && !canJump) {
            PlayerBody.velocity += Vector3.up * Physics.gravity.y * 2f * Time.deltaTime;
        } else if (PlayerBody.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) {
            PlayerBody.velocity += Vector3.up * Physics.gravity.y * 1f * Time.deltaTime;
        }


    }

    private void FixedUpdate() {
        playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        //offset Player input with playerInput
        directionVector = CameraPivot.rotation * playerInput;
        directionVector.Normalize();

        Vector3 velocitySmooth = Vector3.Lerp(PlayerBody.velocity, new Vector3(directionVector.x * speed, PlayerBody.velocity.y, directionVector.z * speed), Time.deltaTime * 7);

        if (!canJump) {
            PlayerBody.velocity = velocitySmooth * 0.92f;
        } else {
            PlayerBody.velocity = velocitySmooth;
        }

        if (directionVector != Vector3.zero) {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionVector), Time.deltaTime * 10f);
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + new Vector3(0,-0.6f,0), .5f);
    }
}
