using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    [SerializeField] private GameObject Player;
    [Space]
    [SerializeField] private float cameraSmoothing;
    [SerializeField] private float cameraSensivity;

    private Vector3 targetRotation;

    void Start() {

    }

    void Update() {
        transform.position = Vector3.Lerp(transform.position, Player.transform.position, Time.deltaTime * cameraSmoothing);

        if (Input.GetMouseButton(2)) {

            targetRotation += new Vector3(0, Input.GetAxis("Mouse X") * cameraSensivity, 0);
            // Debug.Log(targetRotation);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * cameraSmoothing);
    }
}
