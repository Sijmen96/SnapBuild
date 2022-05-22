using UnityEngine;

public class BuildableV2 : MonoBehaviour {
    [SerializeField] Vector3[] snapPoints;
    [SerializeField] Vector3 posistion = new Vector3(0, 0, 0);
    [SerializeField] Vector3 rotation = new Vector3(0, 0, 0);

    public bool isSelectedObject = false;
    private Vector3 closestSnap;
    private Vector3 closestOtherSnap;
    private BuildingManager buildingManager;
    bool snapActive = false;

    Vector3 snap = new Vector3();
    Vector3 otherSnap = new Vector3();
    Vector3 tempMouse = new Vector3();
    Vector3 mousePosition = new Vector3();

    float snapStart = 1f;
    float snapStop = 0.5f;

    void Start() {
        this.buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
    }

    public void updatePositionRotation(Vector3 posistion, Vector3 rotation) {
        mousePosition = posistion;
        //Debug.Log(Vector3.Distance(posistion, transform.position + snapPoints[0]));
        if (Vector3.Distance(posistion, tempMouse) > snapStop) {
            snap = new Vector3();
            otherSnap = new Vector3();
            snapActive = false;
        }


        //Change gameObject location and rotation
        if (!snapActive) {
            transform.position = posistion + snapPoints[0];
            transform.RotateAround(posistion + snapPoints[0], Vector3.up, rotation.y);
        } else {
            transform.RotateAround(otherSnap, Vector3.up, rotation.y);
        }



        //Change snapPoint location
        for (int i = 0; i < snapPoints.Length; i++) {
            //snapPoints[i] = snapPoints[i] + transform.position;
            snapPoints[i] = Quaternion.AngleAxis(rotation.y, Vector3.up) * snapPoints[i];
        }
    }

    void OnDrawGizmos() {
        foreach (var point in snapPoints) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position + point, 0.1f);
        }

        // if (isSelectedObject)
        // {
        //     Gizmos.color = Color.green;
        //     Gizmos.DrawSphere(closestOtherSnap, 0.2f);
        //     Gizmos.DrawSphere(closestSnap, 0.2f);
        // }

        if (isSelectedObject) {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(snap, 0.2f);
            Gizmos.DrawSphere(otherSnap, 0.2f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(snapPoints[0] + transform.position, 0.1f);
        }


    }


    // public Vector3 GetClosestCompared(Vector3[] otherSnapPoints, Vector3 otherPosistion)
    // {
    //     Vector3 closestSnapPoint = Vector3.zero;
    //     Vector3 ohterclosestSnapPoint = Vector3.zero;
    //     float closestDistance = Mathf.Infinity;

    //     //Compare distances
    //     for (int i = 0; i < snapPoints.Length; i++)
    //         for (int j = 0; j < otherSnapPoints.Length; j++)
    //         {
    //             {
    //                 float distance = Vector3.Distance(otherPosistion + otherSnapPoints[j], transform.position + snapPoints[i]);
    //                 if (distance < closestDistance)
    //                 {
    //                     closestDistance = distance;
    //                     closestSnapPoint = transform.position + snapPoints[i];
    //                     this.closestSnap = closestSnapPoint;
    //                     this.closestOtherSnap = otherPosistion + otherSnapPoints[j];
    //                     //ohterclosestSnapPoint = otherPosistion + otherSnapPoints[j];
    //                 }
    //             }
    //         }
    //     Debug.Log(this.closestOtherSnap);
    //     return this.closestOtherSnap;
    // }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (isSelectedObject && other.CompareTag("Buildables"))
    //     {
    //         Debug.Log(other.name);
    //         buildingManager.setSnappedPoint(GetClosestCompared(other.GetComponent<BuildableV2>().getSnapPoints(), other.transform.position));
    //     }
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (isSelectedObject && other.CompareTag("Buildables"))
    //     {
    //         //Debug.Log(other.name);
    //         buildingManager.setSnappedPoint(Vector3.zero);
    //     }
    // }

    public Vector3[] getSnapPoints() {
        return snapPoints;
    }

    private void OnTriggerEnter(Collider other) {
        //check if buildables are colliding
        if (isSelectedObject && other.CompareTag("Buildables")) {
            //Debug.Log(other.name);
            Vector3[] otherSnapPoints = other.GetComponent<BuildableV2>().snapPoints;
            Vector3 otherPosition = other.transform.position;
            float closestDistance = Mathf.Infinity;

            for (int i = 0; i < snapPoints.Length; i++)
                for (int j = 0; j < otherSnapPoints.Length; j++) {
                    {
                        float distance = Vector3.Distance(otherPosition + otherSnapPoints[j], transform.position + snapPoints[i]);

                        if (distance < closestDistance) {
                            closestDistance = distance;
                            snap = transform.position + snapPoints[i];
                            otherSnap = otherPosition + otherSnapPoints[j];
                            //Debug.Log(snap + " " + otherSnap);
                        }
                    }
                }
            if (closestDistance < snapStart) {
                Vector3 DeltaSnap = snap - otherSnap;
                transform.position = transform.position - DeltaSnap;
                snapActive = true;
                tempMouse = mousePosition;
                Debug.Log(tempMouse);

            }
        }

    }

    // private void OnTriggerExit(Collider other) {
    //     if (isSelectedObject && other.CompareTag("Buildables")) {
    //         snap = new Vector3();
    //         otherSnap = new Vector3();
    //         snapActive = false;
    //     }
    // }





}
