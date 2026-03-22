using UnityEngine;

[CreateAssetMenu(fileName = "WaterElementCombine", menuName = "Magic/Water Element Combine")]
public class WaterElementCombineable : ElementCombinable
{
    public GameObject ProjectilePrefab;
    public int Priority;
    public float Damage;

    public override void ApplyElement(SpellData spell)
    {
        base.ApplyElement(spell);
        spell.SetSpellProjectile(ProjectilePrefab, Priority);
        spell.Damage += Damage;
    }
}
