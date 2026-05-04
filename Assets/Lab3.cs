using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public class Lab3 : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        VisualElement izda = root.Q("ItemSlots");
        VisualElement dcha = root.Q("Equipped");

        //if (izda != null)
        //{
        //    izda.AddManipulator(new Lab3Manipulator());
        //}

        //if (dcha != null)
        //{
        //    dcha.AddManipulator(new Lab3Manipulator());
        //}

        //List<VisualElement> lveizda = izda != null ? izda.Children().ToList() : new List<VisualElement>();
        //List<VisualElement> lvedcha = dcha != null ? dcha.Children().ToList() : new List<VisualElement>();

        //lveizda.ForEach(elem => elem.AddManipulator(new Lab3Manipulator()));
        //lvedcha.ForEach(elem => elem.AddManipulator(new Lab3Manipulator()));

        root.Query<VisualElement>(className: "item-slot-image").ForEach(elem => elem.AddManipulator(new Lab3Dragger()));
        //lvedcha.ForEach(elem => elem.AddManipulator(new Lab3Resizer()));
    }
}
