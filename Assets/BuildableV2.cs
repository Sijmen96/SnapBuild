using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableV2 : MonoBehaviour
{
    [SerializeField] Vector3[] snapPoints;
    [SerializeField] Vector3 posistion = new Vector3(0, 0, 0);
    [SerializeField] Vector3 rotation = new Vector3(0, 0, 0);

    public bool isSelectedObject = false;
    private BuildingManager buildingManager;

    void Start()
    {
        this.buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
    }

    public void updatePositionRotation(Vector3 posistion, Vector3 rotation)
    {
        //Change gameObject location and rotation
        transform.position = posistion + snapPoints[0];


        transform.RotateAround(posistion + snapPoints[0], Vector3.up, rotation.y);

        //Change snapPoint location
        for (int i = 0; i < snapPoints.Length; i++)
        {
            //snapPoints[i] = snapPoints[i] + transform.position;
            snapPoints[i] = Quaternion.AngleAxis(rotation.y, Vector3.up) * snapPoints[i];
        }
    }


    void OnDrawGizmos()
    {
        foreach (var point in snapPoints)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position + point, 0.1f);
        }
    }


    // public Vector3 getClosestSnapPoint(Vector3 mousePosistion)
    // {
    //     Vector3 closestSnapPoint = Vector3.zero;
    //     float closestDistance = Mathf.Infinity;

    //     //Compare distances
    //     for (int i = 0; i < snapPoints.Length; i++)
    //     {
    //         float distance = Vector3.Distance(mousePosistion, transform.position + snapPoints[i]);
    //         if (distance < closestDistance)
    //         {
    //             closestDistance = distance;
    //             closestSnapPoint = transform.position + snapPoints[i];
    //         }
    //     }
    //     return closestSnapPoint;
    // }

    public Vector3 GetClosestCompared(Vector3[] otherSnapPoints, Vector3 otherPosistion)
    {
        Vector3 closestSnapPoint = Vector3.zero;
        Vector3 ohterclosestSnapPoint = Vector3.zero;
        float closestDistance = Mathf.Infinity;

        //Compare distances
        for (int i = 0; i < snapPoints.Length; i++)
            for (int j = 0; j < otherSnapPoints.Length; j++)
            {
                {
                    float distance = Vector3.Distance(otherPosistion + otherSnapPoints[j], transform.position + snapPoints[i]);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestSnapPoint = transform.position + snapPoints[i];
                        //ohterclosestSnapPoint = otherPosistion + otherSnapPoints[j];
                    }
                }
            }
        return closestSnapPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isSelectedObject && other.CompareTag("Buildables"))
        {
            Debug.Log(other.name);
            buildingManager.setSnappedPoint(GetClosestCompared(other.GetComponent<BuildableV2>().getSnapPoints(), other.transform.position));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isSelectedObject && other.CompareTag("Buildables"))
        {
            //Debug.Log(other.name);
            buildingManager.setSnappedPoint(Vector3.zero);
        }
    }

    public Vector3[] getSnapPoints()
    {
        return snapPoints;
    }
}
