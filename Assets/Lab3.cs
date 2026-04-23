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

        VisualElement izda = root.Q("ItemInfo");
        VisualElement dcha = root.Q("Equipped");

        izda.AddManipulator(new Lab3Manipulator());
        dcha.AddManipulator(new Lab3Manipulator());

        List<VisualElement> lveizda = izda.Children().ToList();
        List<VisualElement> lvedcha = dcha.Children().ToList();

        lveizda.ForEach(elem => elem.AddManipulator(new Lab3Manipulator()));
        lveizda.ForEach(elem => elem.AddManipulator(new Lab3Dragger()));
        lvedcha.ForEach(elem => elem.AddManipulator(new Lab3Manipulator()));
        //lvedcha.ForEach(elem => elem.AddManipulator(new Lab3Resizer()));
    }
}
