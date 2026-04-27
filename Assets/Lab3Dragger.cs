using UnityEngine;
using UnityEngine.UIElements;

public class Lab3Dragger : PointerManipulator
{

    private Vector3 m_Start;
    protected bool m_Active;
    private int m_PointerId;
    private Vector2 m_StartSize;
    private Vector2 m_PointerOffset;
    private VisualElement m_OriginalParent;
    private int m_OriginalIndex;
    private VisualElement m_DragLayer;

    private const string ItemSlotClassName = "item-slot";
    private const string ItemSlotImageClassName = "item-slot-image";

    public Lab3Dragger()
    {
        m_PointerId = -1;
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        m_Active = false;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(OnPointerDown);
        target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        target.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
        target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
    }

    protected void OnPointerDown(PointerDownEvent evt)
    {
        if (m_Active)
        {
            evt.StopImmediatePropagation();
            return;
        }

        if (!CanStartManipulation(evt))
            return;

        if (target == null || !target.ClassListContains(ItemSlotImageClassName))
            return;

        m_Start = evt.localPosition;
        m_PointerOffset = evt.localPosition;
        m_PointerId = evt.pointerId;
        m_OriginalParent = target.parent;
        m_OriginalIndex = m_OriginalParent != null ? m_OriginalParent.IndexOf(target) : -1;
        m_DragLayer = target.panel?.visualTree;

        m_Active = true;
        if (m_DragLayer != null && target.parent != m_DragLayer)
        {
            m_DragLayer.Add(target);
        }

        target.style.position = Position.Absolute;
        target.style.left = target.worldBound.xMin;
        target.style.top = target.worldBound.yMin;
        target.BringToFront();
        target.CapturePointer(m_PointerId);
        evt.StopPropagation();
    }

    protected void OnPointerMove(PointerMoveEvent evt)
    {
        if (!m_Active || !target.HasPointerCapture(m_PointerId))
            return;

        if (target.parent == null)
            return;

        Vector2 pointerPosition = target.parent.WorldToLocal(evt.position);
        target.style.left = pointerPosition.x - m_PointerOffset.x;
        target.style.top = pointerPosition.y - m_PointerOffset.y;

        evt.StopPropagation();
    }

    protected void OnPointerUp(PointerUpEvent evt)
    {
        if (!m_Active || !target.HasPointerCapture(m_PointerId) || !CanStopManipulation(evt))
            return;

        m_Active = false;

        VisualElement dropSlot = GetSlotUnderPointer(evt.position);
        bool canDrop = dropSlot != null && dropSlot != m_OriginalParent && dropSlot.childCount == 0;

        if (canDrop)
        {
            dropSlot.Add(target);
        }
        else if (m_OriginalParent != null)
        {
            m_OriginalParent.Add(target);
            if (m_OriginalIndex >= 0 && m_OriginalIndex < m_OriginalParent.childCount)
            {
                target.PlaceInFront(m_OriginalParent.ElementAt(m_OriginalIndex));
            }
        }

        target.style.left = StyleKeyword.Null;
        target.style.top = StyleKeyword.Null;
        target.style.position = Position.Relative;

        target.ReleaseMouse();
        evt.StopPropagation();
    }

    private VisualElement GetSlotUnderPointer(Vector2 pointerPosition)
    {
        if (target.panel == null)
            return null;

        PickingMode previousPickingMode = target.pickingMode;
        target.pickingMode = PickingMode.Ignore;

        VisualElement picked = target.panel.Pick(pointerPosition);
        VisualElement slot = FindSlotAncestor(picked);

        target.pickingMode = previousPickingMode;
        return slot;
    }

    private VisualElement FindSlotAncestor(VisualElement element)
    {
        while (element != null)
        {
            if (element.ClassListContains(ItemSlotClassName))
                return element;

            element = element.parent;
        }

        return null;
    }
}