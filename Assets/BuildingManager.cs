using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject[] objects;
    private GameObject selectedObject;
    private Color selectedColor;
    private Vector3 rotation = Vector3.zero;
    public Vector3 snappedPoint;

    void Update()
    {
        if (selectedObject != null)
        {
            updateRotation();
            selectedObject.GetComponent<BuildableV2>().updatePositionRotation(GetMouseWorldPosistionSnapped(), rotation);

            //rotateObject();

            if (Input.GetMouseButtonDown(0))
            {
                placeObject();
            }
            if (Input.GetMouseButtonDown(1))
            {
                cancelObject();
            }
        }

        if (snappedPoint != Vector3.zero && Vector3.Distance(snappedPoint, GetMouseWorldPosistion()) > 1f)
        {
            snappedPoint = Vector3.zero;
        }

    }

    public void setSnappedPoint(Vector3 snappedPoint)
    {
        this.snappedPoint = snappedPoint;
    }

    public void selectObject(int index)
    {
        selectedObject = Instantiate(objects[index], GetMouseWorldPosistionSnapped(), objects[index].transform.rotation);
        selectedObject.GetComponent<BuildableV2>().isSelectedObject = true;

        selectedObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        selectedColor = selectedObject.gameObject.GetComponent<MeshRenderer>().material.color;

    }

    public void placeObject()
    {
        selectedObject.layer = LayerMask.NameToLayer("Default");
        selectedObject.GetComponent<BuildableV2>().isSelectedObject = false;
        selectedObject = null;
    }

    public void cancelObject()
    {
        makeTransparant(false);
        Destroy(selectedObject);
    }

    public void makeTransparant(bool transparantState)
    {
        Color selectedColorTransparant = new Color(selectedColor.r, selectedColor.g, selectedColor.b, 0.5f);
        if (transparantState)
        {
            selectedObject.gameObject.GetComponent<MeshRenderer>().material.color = selectedColorTransparant;
        }
        else
        {
            selectedObject.gameObject.GetComponent<MeshRenderer>().material.color = selectedColor;
        }
    }

    public Vector3 GetMouseWorldPosistionSnapped()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            // if (raycastHit.collider?.gameObject)
            // {
            //     BuildableV2 obj = raycastHit.collider.gameObject.GetComponent<BuildableV2>();
            //     if (obj != null)
            //     {
            //         return obj.getClosestSnapPoint(raycastHit.point);
            //     }
            // }

            if (snappedPoint != Vector3.zero)
            {
                return snappedPoint;
            }

            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 GetMouseWorldPosistion()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    void updateRotation()
    {
        float angle = 0;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            angle += 22.5f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            angle -= 22.5f;
        }
        rotation.y = angle;
    }
}


