using UnityEngine;
using UnityEngine.UIElements;

public class SkillPointsControl : VisualElement
{
    // Factory para poder usarlo en UXML
    public new class UxmlFactory : UxmlFactory<SkillPointsControl, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription m_Points = new UxmlIntAttributeDescription { name = "points", defaultValue = 0 };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var control = ve as SkillPointsControl;
            control.Points = m_Points.GetValueFromBag(bag, cc);
        }
    }

    private Label m_Label;
    private int m_Points;

    public int Points
    {
        get => m_Points;
        set
        {
            if (m_Points != value)
            {
                m_Points = Mathf.Max(0, value);
                UpdateLabel();
            }
        }
    }

    public SkillPointsControl()
    {
        m_Label = new Label("0");
        m_Label.name = "PuntosLabel";
        m_Label.AddToClassList("item-name");
        Add(m_Label);

        style.flexDirection = FlexDirection.Row;
        style.alignItems = Align.Center;
        style.justifyContent = Justify.Center;
        
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        if (m_Label != null)
            m_Label.text = m_Points.ToString();
    }


    public bool SpendPoints(int amount)
    {
        if (m_Points >= amount)
        {
            Points -= amount;
            return true;
        }
        return false;
    }

    public void AddPoints(int amount)
    {
        Points += amount;
    }
}