using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Buildable", menuName = "Buildable")]
public class Buildable : ScriptableObject
{
    public string Name;
    public Sprite Sprite;
    public GameObject Prefab;
}
