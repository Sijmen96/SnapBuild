using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GameObject[] objects;
    private GameObject selectedObject;

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
    }

    public void placeObject()
    {
        selectedObject.GetComponent<BoxCollider>().enabled = true;
        selectedObject = null;
    }

    public void cancelObject()
    {
        Destroy(selectedObject);
    }


    public static Vector3 GetMouseWorldPosistion()
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


