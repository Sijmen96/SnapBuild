using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class testS : MonoBehaviour
{
    [SerializeField] List<Item> Items = new List<Item>();


}

[System.Serializable]
public class Item
{
    public string Name;
    public Sprite Sprite;
    public GameObject Prefab;
}


