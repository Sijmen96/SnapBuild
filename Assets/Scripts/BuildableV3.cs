using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableV3 : MonoBehaviour {

    [SerializeField] public Vector3[] snapPoints;
    public bool isSelectedObject = false;
    private BuildingManager buildingManager;

    void Start() {
        this.buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
    }

    public Vector3[] GetSnapPoints() {
        return snapPoints;
    }

    public Vector3[] GetSnapPointsWorld() {
        Vector3[] snapPointsWorld = new Vector3[snapPoints.Length];
        for (int i = 0; i < snapPoints.Length; i++) {
            snapPointsWorld[i] = transform.position + snapPoints[i];
        }
        return snapPointsWorld;
    }

    private void OnTriggerEnter(Collider other) {
        if (isSelectedObject && other.CompareTag("Buildables")) {
            GameObject otherobj = other.gameObject;
            buildingManager.BuildableIsColliding(otherobj);
        } else {
            //buildingManager.BuildableIsColliding(null);
        }
    }

    void OnDrawGizmos() {
        foreach (var point in snapPoints) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position + point, 0.1f);
        }
    }

}
