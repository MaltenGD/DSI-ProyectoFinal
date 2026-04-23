using UnityEngine;
using UnityEngine.UIElements;

public class Lab3Dragger : PointerManipulator
{

    private Vector3 m_Start;
    protected bool m_Active;
    private int m_PointerId;
    private Vector2 m_StartSize;
    private Vector2 m_PointerOffset;

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

        if (CanStartManipulation(evt))
        {
            m_Start = evt.localPosition;
            m_PointerId = evt.pointerId;

            m_Active = true;
            target.CapturePointer(m_PointerId);
            evt.StopPropagation();
        }
    }

    protected void OnPointerMove(PointerMoveEvent evt)
    {
        if (!m_Active || !target.HasPointerCapture(m_PointerId))
            return;

        Vector2 diff = evt.localPosition - m_Start;

        target.style.top = target.layout.y + diff.y;
        target.style.left = target.layout.x + diff.x;

        evt.StopPropagation();
    }

    protected void OnPointerUp(PointerUpEvent evt)
    {
        if (!m_Active || !target.HasPointerCapture(m_PointerId) || !CanStopManipulation(evt))
            return;

        m_Active = false;

        target.ReleaseMouse();
        evt.StopPropagation();
    }
}