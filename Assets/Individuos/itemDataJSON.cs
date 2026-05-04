[System.Serializable]
public class ItemDataJSON
{
    public string id;
    public string name;
    public string description;
    public string iconName;
}

[System.Serializable]
public class ItemListWrapper
{
    public ItemDataJSON[] items;
}