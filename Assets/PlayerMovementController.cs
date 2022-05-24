using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {

    private Vector3 playerInput;
    private Vector3 directionVector;

    [SerializeField] private Transform CameraPivot;
    [SerializeField] private Rigidbody PlayerBody;
    [Space]
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float sensitivity;
    [SerializeField] private float jumpforce;

    void Update() {
        playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        //offset Player input with playerInput
        directionVector = CameraPivot.rotation * playerInput;
        directionVector.Normalize();


        PlayerBody.velocity = new Vector3(directionVector.x * speed, PlayerBody.velocity.y, directionVector.z * speed);

        if (Input.GetKeyDown(KeyCode.Space)) {
            PlayerBody.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        }

        if (directionVector != Vector3.zero) {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionVector), Time.deltaTime * 5f);

        }

    }


}
