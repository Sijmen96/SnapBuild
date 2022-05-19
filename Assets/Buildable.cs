using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildable : MonoBehaviour
{

    [SerializeField] public Vector3[] snapPoints;

    void Start()
    {

    }


    void Update()
    {




    }


    Vector3 getSnapPointInWorld(Vector3 point)
    {
        return gameObject.transform.position + point;
    }

    void OnDrawGizmos()
    {
        foreach (var point in snapPoints)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(getSnapPointInWorld(point), 0.1f);
            Debug.Log("hi");
        }

    }

}






// public class snapPoint
// {
//     Vector3 direction;
//     Vector3 position;
// }
