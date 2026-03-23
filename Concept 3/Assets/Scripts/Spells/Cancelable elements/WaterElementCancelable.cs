using UnityEngine;

[CreateAssetMenu(fileName = "WaterElementCancelable", menuName = "Magic/Water Element cancelable")]
public class WaterElementCancelable : ElementCancelable
{
    public GameObject ProjectilePrefab;
    public int Priority;
    public float Damage;
    public float Speed;

    public override void ApplyElement(SpellData spell)
    {
        base.ApplyElement(spell);
        spell.SetSpellProjectile(ProjectilePrefab, Priority);
        spell.Damage += Damage;
        spell.Speed += Speed;
    }
}
