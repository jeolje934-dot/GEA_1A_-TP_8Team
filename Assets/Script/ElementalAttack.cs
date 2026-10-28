
using UnityEngine;

public enum ProjectileElement
{
    None,
    Fire,
    Ice,
    Lightning
}

public class ElementalAttack : MonoBehaviour
{
    public ProjectileElement SelectedElement { get; private set; }
        = ProjectileElement.None;

    public bool HasElement => SelectedElement != ProjectileElement.None;

    public void SelectElement(ProjectileElement element)
    {
        SelectedElement = element;
        Debug.Log("선택한 침 속성: " + element);
    }
}
