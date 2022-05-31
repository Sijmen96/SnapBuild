using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "IKState", menuName = "IKState")]
public class IKState : ScriptableObject
{
    public string Name;
    public float stepDistance;
    public float stepSpeed;
    public float stepHeight;
    public float stepOffset;
    public Vector3 leftLegPosition;
    public Vector3 rightLegPosition;


}

