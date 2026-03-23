using UnityEngine;

[CreateAssetMenu(fileName = "FireElementCancelable", menuName = "Magic/Fire Element cancelable")]
public class FireElementCancelable : ElementCancelable
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
