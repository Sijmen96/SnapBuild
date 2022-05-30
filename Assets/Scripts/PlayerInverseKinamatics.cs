using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInverseKinamatics : MonoBehaviour
{

    [SerializeField] PlayerController playerController;
    [Space]
    [SerializeField] GameObject targetLeft;
    [SerializeField] GameObject targetRight;

    private Vector3 leftRayOrigin;
    private Vector3 rightRayOrigin;

    float leftLerp = 0;
    float rightLerp = 0;

    public float stepHeight = .25f;
    public float stepSpeed = 15f;
    public float stepDistance = .5f;

    private bool walkState = false;

    Ray leftRay, rightRay;
    RaycastHit leftHit, rightHit;
    Vector3 leftPoint, rightPoint;
    Vector3 leftFootPosition, rightFootPosition;

    void Update()
    {
        if (playerController.jumpState == PlayerJumpState.grounded)
        {
            leftRayOrigin = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * new Vector3(-.3f, 0, .5f) + transform.position;
            rightRayOrigin = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * new Vector3(.3f, 0, .5f) + transform.position;


            leftRay = new Ray(leftRayOrigin, Vector3.down);
            rightRay = new Ray(rightRayOrigin, Vector3.down);
        }
        else
        {
            leftFootPosition = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * new Vector3(-.3f, -.5f, 0) + transform.position;
            rightFootPosition = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * new Vector3(.3f, -.5f, 0) + transform.position;
        }

        Debug.Log(playerController.jumpState);


        if (Physics.Raycast(leftRay, out leftHit))
        {
            leftPoint = leftHit.point;
            if (Vector3.Distance(leftHit.point, leftFootPosition) > stepDistance && Vector3.Distance(leftHit.point, rightFootPosition) > (stepDistance * 0.9))
            {
                leftLerp = 0;
                leftFootPosition = leftPoint;
            }
        }

        if (Physics.Raycast(rightRay, out rightHit))
        {
            rightPoint = rightHit.point;
            if (Vector3.Distance(rightHit.point, rightFootPosition) > stepDistance && Vector3.Distance(rightHit.point, leftFootPosition) > (stepDistance * 0.9))
            {
                rightLerp = 0;
                rightFootPosition = rightPoint;
            }
        }

        if (leftLerp < 1)
        {
            Vector3 left = Vector3.Lerp(targetLeft.transform.position, leftFootPosition, leftLerp);

            if (walkState)
            {
                left.y += Mathf.Sin(leftLerp * Mathf.PI) * stepHeight;
            }

            targetLeft.transform.position = left;
            leftLerp += Time.fixedDeltaTime * stepSpeed;
            /*targetLeft.transform.position = Vector3.Lerp(targetLeft.transform.position, leftFootPosition, Time.fixedDeltaTime * 15);
            targetRight.transform.position = Vector3.Lerp(targetRight.transform.position, rightFootPosition, Time.fixedDeltaTime * 15);*/
        }
        else
        {
            targetLeft.transform.position = leftFootPosition;
        }


        if (rightLerp < 1)
        {
            Vector3 right = Vector3.Lerp(targetRight.transform.position, rightFootPosition, rightLerp);
            if (walkState)
            {
                right.y += Mathf.Sin(rightLerp * Mathf.PI) * stepHeight;
            }
            targetRight.transform.position = right;
            rightLerp += Time.fixedDeltaTime * stepSpeed;
        }
        else
        {
            targetRight.transform.position = rightFootPosition;
        }
    }




    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(leftRayOrigin, 0.3f);
        Gizmos.DrawSphere(rightRayOrigin, 0.3f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(leftFootPosition, 0.3f);
        Gizmos.DrawSphere(rightFootPosition, 0.3f);
    }
}
