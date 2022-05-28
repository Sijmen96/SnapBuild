using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RobotIK : MonoBehaviour
{
    [SerializeField] GameObject targetLeft;
    [SerializeField] GameObject targetRight;
    [SerializeField] Rigidbody rigidBody;

    public float stepHeight = 2f;
    public float stepSpeed = 15f;
    public float stepDistance = 2f;

    private bool walkState = false;

    Vector3 leftPoint, rightPoint;
    Vector3 leftFootPosition, rightFootPosition;
    float leftLerp = 0;
    float rightLerp = 0;
    void Start()
    {
        leftFootPosition = new Vector3(.5f, -1, .6f) + transform.position;
        rightFootPosition = new Vector3(-.5f, -1, .6f) + transform.position;
    }

    private void Update()
    {
        rigidBody.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * 5, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray leftRay;
        Ray rightRay;
       

        if (rigidBody.velocity.x > .1f || rigidBody.velocity.z > .1f)
        {
            this.walkState = true;
            leftRay = new Ray(new Vector3(stepDistance / 2, 2, .4f) + transform.position, Vector3.down);
            rightRay = new Ray(new Vector3(stepDistance / 2, 2, -.4f) + transform.position, Vector3.down);
        }
        else if (rigidBody.velocity.x < -.1f || rigidBody.velocity.z < -.1f)
        {
            this.walkState = true;
            leftRay = new Ray(new Vector3(-(stepDistance / 1.5f), 2, .4f) + transform.position, Vector3.down);
            rightRay = new Ray(new Vector3(-(stepDistance / 1.5f), 2, -.4f) + transform.position, Vector3.down);
        }
        else
        {

            leftRay = new Ray(new Vector3(0, 2, .4f) + transform.position, Vector3.down);
            rightRay = new Ray(new Vector3(0, 2, -.4f) + transform.position, Vector3.down);
            leftFootPosition = leftPoint;
            rightFootPosition = rightPoint;

            if (walkState)
            {
                leftLerp = 0;
                rightLerp = 0;
                this.walkState = false;
            }

        }

        RaycastHit leftHit;
        RaycastHit rightHit;

        if (Physics.Raycast(leftRay, out leftHit))
        {
            leftPoint = leftHit.point;
            if (Vector3.Distance(leftHit.point, leftFootPosition) > stepDistance && Vector3.Distance(leftHit.point, rightFootPosition) > (stepDistance * 0.8))
            {
                leftLerp = 0;
                leftFootPosition = leftPoint;
            }
        }

        if (Physics.Raycast(rightRay, out rightHit))
        {
            rightPoint = rightHit.point;
            if (Vector3.Distance(rightHit.point, rightFootPosition) > stepDistance && Vector3.Distance(rightHit.point, leftFootPosition) > (stepDistance * 0.8))
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
        Gizmos.DrawSphere(leftPoint, 0.3f);
        Gizmos.DrawSphere(rightPoint, 0.3f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(leftFootPosition, 0.3f);
        Gizmos.DrawSphere(rightFootPosition, 0.3f);
    }
}
