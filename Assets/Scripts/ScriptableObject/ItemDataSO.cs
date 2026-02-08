using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemDataSO", order = 2)]
public class ItemDataSO : ScriptableObject
{

}
public class ItemData
{
    public ObjectType itemType;
    public string itemName;
    public int itemValue;
}
public enum ObjectTypeByLevel
{
    Red,
    Blue,
    Green,
    Yellow,
    Purple
}