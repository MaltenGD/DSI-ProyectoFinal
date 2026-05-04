using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemsData : ScriptableObject
{
    public string itemName;

    [TextArea]
    public string description;

    public Sprite icon;
}