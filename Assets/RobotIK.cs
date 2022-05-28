using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RobotIK : MonoBehaviour
{
    [SerializeField] GameObject targetLeft;
    [SerializeField] GameObject targetRight;

    Vector3 leftPoint, rightPoint;
    Vector3 leftFootPosiston, rightFootPosistion;

    void Start()
    {
        leftFootPosiston = new Vector3(.5f, -1, .6f) + transform.position;
        rightFootPosistion = new Vector3(-.5f, -1, .6f) + transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray leftRay = new Ray(new Vector3(1, 2, .4f) + transform.position, Vector3.down);
        Ray rightRay = new Ray(new Vector3(1, 2, -.4f) + transform.position, Vector3.down);

        RaycastHit leftHit;
        RaycastHit rightHit;

        if (Physics.Raycast(leftRay, out leftHit))
        {
            leftPoint = leftHit.point;
            if (Vector3.Distance(leftHit.point, leftFootPosiston) > 2)
            {
                leftFootPosiston = leftPoint;
            }
        }

        if (Physics.Raycast(rightRay, out rightHit))
        {
            rightPoint = rightHit.point;
            if (Vector3.Distance(rightHit.point, rightFootPosistion) > 2)
            {
                rightFootPosistion = rightPoint;
                //rightFootPosistion = rightPoint - new Vector3(transform.position.x, 0, transform.position.z);
            }
        }



        targetLeft.transform.position = Vector3.Lerp(targetLeft.transform.position, leftFootPosiston, Time.fixedDeltaTime * 15);
        targetRight.transform.position = Vector3.Lerp(targetRight.transform.position, rightFootPosistion, Time.fixedDeltaTime * 15);

    }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(leftPoint, 0.3f);
        Gizmos.DrawSphere(rightPoint, 0.3f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(leftFootPosiston, 0.3f);
        Gizmos.DrawSphere(rightFootPosistion, 0.3f);
    }
}
