using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject[] objects;
    private GameObject selectedObject;
    private Color selectedColor;

    void Update()
    {
        if (selectedObject != null)
        {
            selectedObject.transform.position = GetMouseWorldPosistion();
            if (Input.GetMouseButtonDown(0))
            {
                placeObject();
            }
            if (Input.GetMouseButtonDown(1))
            {
                cancelObject();
            }
        }

        rotateObject();
    }


    public void selectObject(int index)
    {
        selectedObject = Instantiate(objects[index], GetMouseWorldPosistion(), objects[index].transform.rotation);
        selectedObject.GetComponent<BoxCollider>().enabled = false;
        selectedColor = selectedObject.gameObject.GetComponent<MeshRenderer>().material.color;
        makeTransparant(true);
    }

    public void placeObject()
    {
        makeTransparant(false);
        selectedObject.GetComponent<BoxCollider>().enabled = true;
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



    public Vector3 GetMouseWorldPosistion()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (raycastHit.collider?.gameObject)
            {
                Buildable obj = raycastHit.collider.gameObject.GetComponent<Buildable>();
                if (obj)
                {
                    return obj.getClosestSnapPoint(raycastHit.point) + gameObject.transform.position + (raycastHit.normal - Vector3.up);
                }
            }
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public void rotateObject()
    {
        float angle;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            angle = +360 / 16;
            selectedObject.transform.Rotate(Vector3.up, angle);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            angle = -360 / 16;
            selectedObject.transform.Rotate(Vector3.up, angle);

        }
    }
}


