using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Lab3Manipulator : MouseManipulator
{
    public Lab3Manipulator()
    {
        //activators.Add(new ManipulatorActivationFilter { button = MouseButton.RightMouse });
    }

    // Start is called before the first frame update
    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
    }

    // Update is called once per frame
    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseEnterEvent>(OnMouseEnter);
    }

    private void OnMouseEnter(MouseEnterEvent mev)
    {
        Debug.Log(target.name + ": Hover en Elemento");
        if (target.parent != null)
        {
            foreach (var child in target.parent.Children())
            {
                child.style.borderBottomColor = Color.black;
                child.style.borderLeftColor = Color.black;
                child.style.borderRightColor = Color.black;
                child.style.borderTopColor = Color.black;
                //mev.StopPropagation();
            }
        }
        target.style.borderBottomColor = Color.white;
        target.style.borderLeftColor = Color.white;
        target.style.borderRightColor = Color.white;
        target.style.borderTopColor = Color.white;
    }
}
