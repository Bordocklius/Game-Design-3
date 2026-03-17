using UnityEngine;

public abstract class Element : ScriptableObject
{
    [Space(10), Header("Element settings")]
    public string ElementName;
    public float ManaCost;
    public Sprite ElementSprite;

    public virtual void ApplyElement(SpellData spell)
    {
        spell.SpellCost += ManaCost;
    }
}
