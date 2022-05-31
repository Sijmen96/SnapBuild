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
    [SerializeField] float stepOffset;
    [Space]
    [SerializeField] GameObject targetLeft;
    [SerializeField] GameObject targetRight;
    [Space]
    [SerializeField] public IKState idle;
    [SerializeField] public IKState walking;
    [SerializeField] public IKState running;
    [SerializeField] public IKState crouching;
    [SerializeField] public IKState rising;
    [SerializeField] public IKState falling;

    private IKState currentIKState;

    private Vector3 leftRayOrigin, rightRayOrigin;
    private Vector3 leftFootTarget, rightFootTarget;
    private Vector3 leftFootPosition, rightFootPosition;

    private float leftLerp, rightLerp;

    private float tempRotation;
    private bool lockRising, lockFalling, lockGrounded;

    void Start()
    {
        currentIKState = idle;
    }

    void Update()
    {
        if (playerController.jumpState == PlayerJumpState.grounded)
        {
            CalculateRayOrigins();

            lockRising = false;
            lockFalling = false;

            leftFootTarget = getRay(new Ray(leftRayOrigin, Vector3.down));
            rightFootTarget = getRay(new Ray(rightRayOrigin, Vector3.down));

            if (Vector3.Distance(leftFootTarget, leftFootPosition) > currentIKState.stepDistance && Vector3.Distance(leftFootTarget, rightFootPosition) > (currentIKState.stepDistance * .95))
            {
                leftLerp = 0;
                leftFootPosition = leftFootTarget;
            }

            if (Vector3.Distance(rightFootTarget, rightFootPosition) > currentIKState.stepDistance && Vector3.Distance(rightFootTarget, leftFootPosition) > (currentIKState.stepDistance * .95))
            {
                rightLerp = 0;
                rightFootPosition = rightFootTarget;
            }
            if (!lockGrounded)
            {
                leftFootTarget = getRay(new Ray(leftRayOrigin, Vector3.down));
                rightFootTarget = getRay(new Ray(rightRayOrigin, Vector3.down));
                lockGrounded = true;
                leftFootPosition = leftFootTarget;
                rightFootPosition = rightFootTarget;
            }


        }
        else if (playerController.jumpState == PlayerJumpState.rising)
        {
            currentIKState = rising;
            if (!lockRising)
            {
                lockRising = true;
                currentIKState = rising;
            }
            leftFootPosition = RotateVector(currentIKState.leftLegPosition);
            rightFootPosition = RotateVector(currentIKState.rightLegPosition);
        }
        else if (playerController.jumpState == PlayerJumpState.falling)
        {
            currentIKState = falling;
            if (!lockFalling)
            {
                lockFalling = true;
                lockGrounded = false;
                leftLerp = 0f;
                rightLerp = 0f;
            }
            leftFootPosition = RotateVector(currentIKState.leftLegPosition);
            rightFootPosition = RotateVector(currentIKState.rightLegPosition);
        }

        if (playerController.movementState == PlayerMovementState.idle)
        {
            currentIKState = idle;
        }
        else if (playerController.movementState == PlayerMovementState.walking)
        {
            currentIKState = walking;
        }
        else if (playerController.movementState == PlayerMovementState.running)
        {
            currentIKState = running;
        }
        else if (playerController.movementState == PlayerMovementState.crouching)
        {
            currentIKState = crouching;
        }





    }

    private void FixedUpdate()
    {
        AnimateLeg(leftFootPosition, ref targetLeft, ref leftLerp);
        AnimateLeg(rightFootPosition, ref targetRight, ref rightLerp);
    }

    void AnimateLeg(Vector3 targetPosition, ref GameObject targetLeg, ref float lerp)
    {
        if (lerp < 1)
        {
            Debug.Log(lerp);
            Vector3 target = Vector3.Lerp(targetLeg.transform.position, targetPosition, lerp);
            target.y += Mathf.Sin(lerp * Mathf.PI) * currentIKState.stepHeight;

            targetLeg.transform.position = target;
            lerp += Time.fixedDeltaTime * currentIKState.stepSpeed;
        }
        else
        {
            targetLeg.transform.position = targetPosition;
        }
    }

    void CalculateRayOrigins()
    {
        leftRayOrigin = RotateVector(new Vector3(-.3f, 0, currentIKState.stepOffset));
        rightRayOrigin = RotateVector(new Vector3(.3f, 0, currentIKState.stepOffset));
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
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(leftFootTarget, 0.1f);
        Gizmos.DrawSphere(rightFootTarget, 0.1f);
    }
}
