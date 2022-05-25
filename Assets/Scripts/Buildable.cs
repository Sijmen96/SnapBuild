using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildable : MonoBehaviour {
    [SerializeField] public Vector3[] snapPoints;

    private Vector3 rotation;


    void Start() {
    }


    void Update() {
    }


    Vector3 getSnapPointInWorld(Vector3 point) {
        return RotatePointAroundPivot(gameObject.transform.position + point, gameObject.transform.position, gameObject.transform.localRotation.eulerAngles);
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }


    public Vector3 getClosestSnapPoint(Vector3 point) {
        Vector3 closePoint = new Vector3();
        float closestDistance = Mathf.Infinity;
        foreach (Vector3 item in snapPoints) {
            Vector3 snapPoint = getSnapPointInWorld(item);
            float sqrTarget = (snapPoint - point).sqrMagnitude;
            if (sqrTarget < closestDistance) {
                closestDistance = sqrTarget;
                closePoint = snapPoint;
            }

        }
        return closePoint;
    }

    void Rotate(Vector3 pivot, Vector3 rotation) {
        this.rotation = rotation;
        RotatePointAroundPivot(this.transform.position, pivot, rotation);
        //gameObject.transform.rotation = RotatePointAroundPivot(this.transform.position, pivot, rotation);

    }

    void OnDrawGizmos() {
        foreach (var point in snapPoints) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(getSnapPointInWorld(point), 0.1f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(getSnapPointInWorld(snapPoints[0]), 0.05f);
        }
    }
}