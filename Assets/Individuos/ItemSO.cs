using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;

    // Opcional (muy útil a futuro)
    public ItemType type;
}

public enum ItemType
{
    Rosary,
    Relic,
    Quest,
    Weapon,
    Prayer,
    Ability
}