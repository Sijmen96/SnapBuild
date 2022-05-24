using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {

    private Vector3 playerInput;
    private Vector3 rotatedVector;

    [SerializeField] private Transform CameraPivot;
    [SerializeField] private Rigidbody PlayerBody;
    [Space]
    [SerializeField] private float invisibilityRange = 10;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float sensitivity;
    [SerializeField] private float jumpforce;
    Quaternion rotation;
    private List<GameObject> invisibleGameObjects = new List<GameObject>();



    void Start() {

    }

    private void FixedUpdate() {
        GetObjectsAbovePlayer();
    }

    void Update() {
        playerInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        rotation = CameraPivot.rotation;
        rotatedVector = rotation * playerInput;
        MovePlayer();

    }

    private void GetObjectsAbovePlayer() {
        List<GameObject> gameObjectsAbovePlayer = new List<GameObject>();
        List<Collider> hitColliders = Physics.OverlapBox(transform.position - (rotation * new Vector3(0, 0, 3)), new Vector3(invisibilityRange,invisibilityRange,invisibilityRange)).ToList();

        // add all gameobjects that are buildables and above player to a list
        foreach (var hitCollider in hitColliders) {
            if (hitCollider.gameObject.CompareTag("Buildables") && hitCollider.transform.position.y >= transform.position.y + 1) {
                gameObjectsAbovePlayer.Add(hitCollider.gameObject);
            }
        }
        // check if gameobjects are already invisible, else make invisable and add to list
        foreach (var gameObject in gameObjectsAbovePlayer) {
            if (invisibleGameObjects.IndexOf(gameObject) == -1) {
                invisibleGameObjects.Add(gameObject);
                gameObject.GetComponent<BuildableV3>().setVisibility(false);
            }
        }
        // check if any invisible gameobjects are no longer under the player
        for (int i = 0; i < invisibleGameObjects.Count; i++) {
            GameObject gameObject = invisibleGameObjects[i];
            if (gameObjectsAbovePlayer.IndexOf(gameObject) == -1) {
                gameObject.GetComponent<BuildableV3>().setVisibility(true);
                invisibleGameObjects.Remove(gameObject);
            }
        }
    }

    private void MovePlayer() {
        Vector3 MoveVector = transform.TransformDirection(rotatedVector) * speed;
        PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z);

        if (Input.GetKeyDown(KeyCode.Space)) {
            PlayerBody.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
        }

    }
}
