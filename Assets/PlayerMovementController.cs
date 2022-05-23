using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {

    private Vector3 playerInput;
    private Vector3 rotatedVector;

    [SerializeField] private Transform CameraPivot;
    [SerializeField] private Rigidbody PlayerBody;
    [Space]
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float sensitivity;
    [SerializeField] private float jumpforce;
    Quaternion rotation;



    void Start() {

    }

    void Update() {
        playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));


        rotation = CameraPivot.rotation;
        rotatedVector = rotation * playerInput;
        MovePlayer();

    }

    private void MovePlayer() {
        Vector3 MoveVector = transform.TransformDirection(rotatedVector) * speed;
        PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z);

        if (Input.GetKeyDown(KeyCode.Space)) {
            PlayerBody.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        }

    }


}
