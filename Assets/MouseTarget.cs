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
        }
    }


    void selectObject(int index)
    {
        selectedObject = Instantiate(objects[index], GetMouseWorldPosistion(), transform.rotation);
    }

    void placeObject()
    {
        selectedObject = null;

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
}


