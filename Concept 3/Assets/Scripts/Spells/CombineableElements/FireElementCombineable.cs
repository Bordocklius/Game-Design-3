using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "FireElementCombine", menuName = "Magic/Fire Element Combine")]
public class FireElementCombineable : ElementCombinable
{
    public GameObject ProjectilePrefab;
    public int Priority;
    public float Damage;

    public override void ApplyElement(SpellData spell)
    {
        base.ApplyElement(spell);
        spell.SetSpellProjectile(ProjectilePrefab, Priority, this);
        spell.Damage += Damage;
    }
}
