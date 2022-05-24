using UnityEngine;

public class BuildableV3 : MonoBehaviour {

    [SerializeField] public Vector3[] snapPoints;
    public bool isSelectedObject = false;
    public Material visibleMaterial;
    public Material invisibleMaterial;
    private BuildingManager buildingManager;
    private bool isVisible = false;

    void Start() {
        this.buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
    }

    public void setVisibility(bool isVisible) {
             Debug.Log("set visibility: " + isVisible);
        if (this.isVisible != isVisible) {
            gameObject.GetComponent<Renderer>().material = isVisible ? visibleMaterial : invisibleMaterial;
            var color = invisibleMaterial.color;
            invisibleMaterial.color = new Color(color.r, color.g, color.b, 0.05f);
        }
        this.isVisible = isVisible;
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
