using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingSystemUI : MonoBehaviour
{

    [SerializeField] GameObject buildingManagerObj;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] GameObject buildScreen;



    void Start()
    {
        int index = 0;
        BuildableDatabase buildableDatabase = buildingManagerObj.GetComponent<BuildableDatabase>();
        BuildingManager buildingManager = buildingManagerObj.GetComponent<BuildingManager>();

        foreach (var buildable in buildableDatabase.Buildables)
        {
            var i = index;
            GameObject Button = Instantiate(buttonPrefab);
            Button.GetComponent<UnityEngine.UI.Button>().image.sprite = buildable.Sprite;
            Button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ButtonClicked(i));
            Button.transform.SetParent(buildScreen.transform);


            index++;
        }
    }



    void ButtonClicked(int index)
    {
        BuildingManager buildingManager = buildingManagerObj.GetComponent<BuildingManager>();
        buildingManager.selectObject(index);
    }

}
