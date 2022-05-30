using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInverseKinamaticsV2 : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Rigidbody playerRigidbody;
    [Space]
    [SerializeField] float stepDistance;
    [SerializeField] float stepHeight;
    [SerializeField] float stepSpeed;
    [Space]
    [SerializeField] GameObject targetLeft;
    [SerializeField] GameObject targetRight;

    private Vector3 leftRayOrigin, rightRayOrigin;
    private Vector3 leftFootTarget, rightFootTarget;
    private Vector3 leftFootPosition, rightFootPosition;
    private float leftLerp, rightLerp;

    void Update()
    {
        if (playerController.jumpState == PlayerJumpState.grounded)
        {
            CalculateRayOrigins();

            leftFootTarget = getRay(new Ray(leftRayOrigin, Vector3.down));
            rightFootTarget = getRay(new Ray(rightRayOrigin, Vector3.down));

            if (Vector3.Distance(leftFootTarget, leftFootPosition) > stepDistance && Vector3.Distance(leftFootTarget, rightFootPosition) > (stepDistance * 0.8))
            {
                Debug.Log("set step");
                leftLerp = 0;
                leftFootPosition = leftFootTarget;
            }

            if (Vector3.Distance(rightFootTarget, rightFootPosition) > stepDistance && Vector3.Distance(rightFootTarget, leftFootPosition) > (stepDistance * 0.8))
            {
                rightLerp = 0;
                rightFootPosition = rightFootTarget;
            }

            AnimateLeg(leftFootPosition, targetLeft, leftLerp);
            AnimateLeg(rightFootPosition, targetRight, rightLerp);


        }
        else if (playerController.jumpState == PlayerJumpState.rising)
        {


        }
        else if (playerController.jumpState == PlayerJumpState.falling)
        {


        }

    }

    void AnimateLeg(Vector3 targetPosition, GameObject targetLeg, float lerp)
    {
        if (lerp < 1)
        {
            Vector3 left = Vector3.Lerp(targetLeg.transform.position, targetPosition, lerp);
            left.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            targetLeg.transform.position = left;
            lerp += Time.deltaTime * stepSpeed;
            //Debug.Log(lerp);
        }
        else
        {
            targetLeg.transform.position = targetPosition;
        }
    }

    void CalculateRayOrigins()
    {
        float speedStepDistance = playerRigidbody.velocity.magnitude * (stepDistance / 5);

        leftRayOrigin = RotateVector(new Vector3(-.3f, 0, speedStepDistance));
        rightRayOrigin = RotateVector(new Vector3(.3f, 0, speedStepDistance));
    }

    private Vector3 RotateVector(Vector3 vector)
    {
        return Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * vector + transform.position;
    }

    Vector3 getRay(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(leftRayOrigin, 0.1f);
        Gizmos.DrawSphere(rightRayOrigin, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(leftFootPosition, 0.1f);
        Gizmos.DrawSphere(rightFootPosition, 0.1f);
    }
}
