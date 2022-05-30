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
    [SerializeField] float rayVelocityMultiplier;
    [Space]
    [SerializeField] GameObject targetLeft;
    [SerializeField] GameObject targetRight;

    private Vector3 leftRayOrigin, rightRayOrigin;
    private Vector3 leftFootTarget, rightFootTarget;
    private Vector3 leftFootPosition, rightFootPosition;
    private float leftLerp, rightLerp;
    private float stepFloat, speedFloat;

    void Update()
    {
        stepFloat = stepDistance * playerRigidbody.velocity.magnitude * 0.3f;
        speedFloat = stepSpeed + (playerRigidbody.velocity.magnitude * .05f);
        //Debug.Log(speedFloat);

        if (playerController.jumpState == PlayerJumpState.grounded)
        {
            CalculateRayOrigins();

            leftFootTarget = getRay(new Ray(leftRayOrigin, Vector3.down));
            rightFootTarget = getRay(new Ray(rightRayOrigin, Vector3.down));

            if (Vector3.Distance(leftFootTarget, leftFootPosition) > stepFloat && Vector3.Distance(leftFootTarget, rightFootPosition) > (stepFloat * 0.8))
            {
                leftLerp = 0;
                leftFootPosition = leftFootTarget;
            }

            if (Vector3.Distance(rightFootTarget, rightFootPosition) > stepFloat && Vector3.Distance(rightFootTarget, leftFootPosition) > (stepFloat * 0.8))
            {
                rightLerp = 0;
                rightFootPosition = rightFootTarget;
            }

            AnimateLeg(leftFootPosition, ref targetLeft, ref leftLerp);
            AnimateLeg(rightFootPosition, ref targetRight, ref rightLerp);


        }
        else if (playerController.jumpState == PlayerJumpState.rising)
        {


        }
        else if (playerController.jumpState == PlayerJumpState.falling)
        {


        }

    }

    void AnimateLeg(Vector3 targetPosition, ref GameObject targetLeg, ref float lerp)
    {
        if (lerp < 1)
        {
            Vector3 target = Vector3.Lerp(targetLeg.transform.position, targetPosition, lerp);
            target.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            targetLeg.transform.position = target;
            lerp += Time.deltaTime * speedFloat;
            Debug.Log(lerp);
        }
        else
        {
            targetLeg.transform.position = targetPosition;
        }
    }

    void CalculateRayOrigins()
    {
        float speedStepDistance = playerRigidbody.velocity.magnitude * rayVelocityMultiplier;

        leftRayOrigin = RotateVector(new Vector3(-.3f, 0, speedStepDistance + 0.3f));
        rightRayOrigin = RotateVector(new Vector3(.3f, 0, speedStepDistance + 0.3f));
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
