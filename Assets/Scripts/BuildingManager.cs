using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildingManager : MonoBehaviour
{
    public GameObject[] objects;
    public GameObject BuildMenu;
    public GameObject mainMenu;
    private GameObject selectedObject;
    private Vector3 selectedRotation;

    private Vector3 selectedSnap = new Vector3();
    private Vector3 otherSnap = new Vector3();
    private Vector3 tempMouse = new Vector3();
    private Vector3 mousePosition = new Vector3();

    private bool snapActive = false;

    private void Start()
    {
        mainMenu.transform.GetComponentInChildren<Button>().onClick.AddListener(onMainMenuClick);
    }

    void Update()
    {
        BuildingManagerMenu();
        MainMenu();

        if (selectedObject != null)
        {
            UpdateRotation();

            mousePosition = GetMouseWorldPosistion();

            if (Vector3.Distance(mousePosition, tempMouse) > 1f)
            {
                selectedSnap = new Vector3();
                otherSnap = new Vector3();
                snapActive = false;
            }

            if (snapActive)
            {
                selectedObject.transform.RotateAround(otherSnap, Vector3.up, selectedRotation.y);
            }
            else
            {
                selectedObject.transform.position = GetMouseWorldPosistion() + selectedObject.GetComponent<BuildableV3>().snapPoints[0]; //snap point?
                selectedObject.transform.RotateAround(selectedSnap, Vector3.up, selectedRotation.y);
            }

            //Rotate snapPoints
            for (int i = 0; i < selectedObject.GetComponent<BuildableV3>().snapPoints.Length; i++)
            {
                selectedObject.GetComponent<BuildableV3>().snapPoints[i] = Quaternion.AngleAxis(selectedRotation.y, Vector3.up) * selectedObject.GetComponent<BuildableV3>().snapPoints[i];
            }


            if (Input.GetMouseButtonDown(0))
            {
                placeObject();
            }
            if (Input.GetMouseButtonDown(1))
            {
                cancelObject();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            destroyObject();
        }
    }

    public void selectObject(int index)
    {
        selectedObject = Instantiate(objects[index], GetMouseWorldPosistion(), objects[index].transform.rotation);
        selectedObject.GetComponent<BuildableV3>().isSelectedObject = true;
        selectedObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    public void placeObject()
    {
        selectedObject.layer = LayerMask.NameToLayer("Default");
        selectedObject.GetComponent<BuildableV3>().isSelectedObject = false;
        selectedObject = null;
    }

    public void cancelObject()
    {
        Destroy(selectedObject);
    }

    public void destroyObject()
    {
        GameObject tempObject = GetGameObject();
        if (tempObject.CompareTag("Buildables"))
        {
            Destroy(tempObject);
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

    public void UpdateRotation()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            selectedRotation.y = 22.5f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            selectedRotation.y = -22.5f;
        }
        else
        {
            selectedRotation.y = 0;
        }
    }

    public GameObject GetGameObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.transform.gameObject;
        }
        else
        {
            return null;
        }
    }

    //Get ontriggerEnter other
    public void BuildableIsColliding(GameObject otherGameobject)
    {
        if (snapActive == false)
        {
            BuildableV3 other = otherGameobject.GetComponent<BuildableV3>();
            BuildableV3 selected = selectedObject.GetComponent<BuildableV3>();

            Vector3[] otherSnapPoints = other.GetSnapPointsWorld();
            Vector3[] selectedSnapPoints = selected.GetSnapPointsWorld();

            float closestDistance = Mathf.Infinity;
            for (int i = 0; i < selectedSnapPoints.Length; i++)
            {
                for (int j = 0; j < otherSnapPoints.Length; j++)
                {
                    {
                        float distance = Vector3.Distance(otherSnapPoints[j], selectedSnapPoints[i]);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;

                            selectedSnap = selectedSnapPoints[i];
                            otherSnap = otherSnapPoints[j]; //Is dit goed?
                        }
                    }
                }
            }
            if (closestDistance < 1f)
            {
                Vector3 DeltaSnap = selectedSnap - otherSnap;
                selectedObject.transform.position = selectedObject.transform.position - DeltaSnap;
                snapActive = true;
                tempMouse = mousePosition;
            }
        }
    }



    void BuildingManagerMenu()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            BuildMenu.SetActive(true);
        }
        else
        {
            BuildMenu.SetActive(false);
        }

    }

    void MainMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainMenu.SetActive(!mainMenu.activeSelf);
        }

    }

    private void onMainMenuClick()
    {
        SceneManager.LoadScene("MainMenu");
    }




    void OnDrawGizmos()
    {
        if (snapActive)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(selectedSnap, 0.2f);
            Gizmos.DrawSphere(otherSnap, 0.2f);
        }
    }
}


