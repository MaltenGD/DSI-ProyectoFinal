using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillToggle : MonoBehaviour
{
    public UIDocument uiDocument;
    private SkillPointsControl pointsControl;

    void OnEnable()
    {
        var document = uiDocument != null ? uiDocument : GetComponent<UIDocument>();
        if (document == null)
        {
            Debug.LogWarning("SkillToggle: No UIDocument assigned or found on the GameObject.");
            return;
        }

        var root = document.rootVisualElement;

        pointsControl = root.Q<SkillPointsControl>("SkillPoints");
        if (pointsControl == null)
        {
            Debug.LogWarning("SkillToggle: No se encontró SkillPointsControl con nombre 'SkillPoints'");
        }

        RegisterSkillElements(root);
    }

    void RegisterSkillElements(VisualElement root)
    {

        var skillImages = root.Query<VisualElement>()
            .Where(v => v.ClassListContains("skill-image"))
            .ToList();

        foreach (var img in skillImages)
        {
            var slot = img.parent;
            if (slot == null)
                continue;

            img.RegisterCallback<PointerDownEvent>(evt =>
            {
                ToggleSkill(slot);
                evt.StopImmediatePropagation();
            });
        }
    }

    void ToggleSkill(VisualElement slot)
    {

        if (pointsControl == null)
        {
            ToggleSkillVisual(slot);
            return;
        }

        bool isCurrentlyActivated = slot.ClassListContains("skill-activated");

        if (isCurrentlyActivated)
        {
            pointsControl.AddPoints(1);
            ToggleSkillVisual(slot);
        }
        else
        {

            if (pointsControl.SpendPoints(1))
            {
                ToggleSkillVisual(slot);
            }
            else
            {
                Debug.Log("No tienes suficientes puntos para desbloquear esta habilidad.");
            }
        }
    }

    void ToggleSkillVisual(VisualElement slot)
    {
        bool currentlyActivated = slot.ClassListContains("skill-activated");

        if (currentlyActivated)
        {
            slot.RemoveFromClassList("skill-activated");
            slot.AddToClassList("skill-deactivated");
        }
        else
        {
            slot.RemoveFromClassList("skill-deactivated");
            slot.AddToClassList("skill-activated");
        }

        ToggleSkillImages(slot,!currentlyActivated);
    }

    void ToggleSkillImages(VisualElement slot, bool activate)
    {
        var desactivadoImage = slot.Q<VisualElement>("Desactivado");
        var activadoImage = slot.Q<VisualElement>("Activado");

        if (desactivadoImage != null)
            desactivadoImage.style.display = activate ? DisplayStyle.None : DisplayStyle.Flex;

        if (activadoImage != null)
            activadoImage.style.display = activate ? DisplayStyle.Flex : DisplayStyle.None;
    }
}