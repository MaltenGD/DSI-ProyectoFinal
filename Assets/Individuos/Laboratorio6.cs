using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class Laboratorio6 : MonoBehaviour
{
    public UIDocument uiDocument;

    private Label itemNameLabel;
    private VisualElement itemIcon;
    private Label descriptionLabel;

    private List<ItemDataJSON> items;
    private Dictionary<string, Sprite> spriteDict;

    void Start()
    {
        var root = uiDocument.rootVisualElement;
        var rosaryPanel = root.Q<VisualElement>("Rosary_Beads-content");

        itemNameLabel = rosaryPanel.Q<Label>("item-name");
        itemIcon = rosaryPanel.Q<VisualElement>("ItemIco");
        descriptionLabel = rosaryPanel.Q<Label>("LabelDescr");

        LoadSprites();
        LoadItemsFromJSON();
        BindSlots(root);
    }

    void LoadSprites()
    {
        var sprites = Resources.LoadAll<Sprite>("Icons/items-icons-spritesheet");

        spriteDict = new Dictionary<string, Sprite>();

        foreach (var s in sprites)
        {
            spriteDict[s.name] = s;
        }
        //foreach (var s in sprites)
        //{
        //    Debug.Log(s.name);
        //}
    }

    void LoadItemsFromJSON()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("items");
        
        if (jsonFile == null)
        {
            Debug.LogError("No se encontró el JSON");
            return;
        }
        
        ItemListWrapper wrapper =
            JsonUtility.FromJson<ItemListWrapper>(jsonFile.text);

        items = new List<ItemDataJSON>(wrapper.items);
    }

    void BindSlots(VisualElement root)
    {
        var container = root.Q<VisualElement>("ItemSlots");
        var slots = container.Query<VisualElement>(className: "item-slot").ToList();

        for (int i = 0; i < slots.Count; i++)
        {
            if (i >= items.Count) break;

            var slot = slots[i];
            var item = items[i];

            var img = slot.Q<Image>("Draggeable_Item");
            img.pickingMode = PickingMode.Position; // 
            if (img == null) continue;

            //  guardamos el item en la imagen (no en el slot)
            img.userData = item;

            if (spriteDict.TryGetValue(item.iconName, out Sprite icon))
            {
                img.style.backgroundImage = new StyleBackground(icon);
            }

            //  click directo en el item

            img.AddManipulator(new Lab3Manipulator(this));
            //img.RegisterCallback<ClickEvent>(_ =>
            //{
            //    OnItemClicked(img);
            //});
        }
    }

    public void OnItemClicked(VisualElement element)
    {
        var item = element.userData as ItemDataJSON;

        if (item == null)
        {
            Debug.Log("Item NULL");
            return;
        }

        Debug.Log("CLICK en: " + item.name);

        itemNameLabel.text = item.name;
        descriptionLabel.text = item.description;
        if (spriteDict.TryGetValue(item.iconName, out Sprite icon))
        {
            itemIcon.style.backgroundImage = new StyleBackground(icon);
        }
    }
}