using UnityEngine;

[CreateAssetMenu(fileName = "EarthElement", menuName = "Magic/Earth Element")]
public class EarthElement : Element
{
    public float Scale;

    public override void ApplyElement(SpellData spell)
    {
        base.ApplyElement(spell);
        spell.ProjectileScale += Scale;
    }
}
