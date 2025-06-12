using UnityEngine;

public class OutlineToggle : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Material normalMaterial;
    [SerializeField] Material outlineMaterial;

    public void SetOutlineEnabled(bool on)
    {
        sr.material = on
            ? outlineMaterial
            : normalMaterial;
    }
}
