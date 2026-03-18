using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "FireElementCombine", menuName = "Magic/Fire Element Combine")]
public class FireElementCombineable : ElementCombinable
{
    public GameObject ProjectilePrefab;
    public float Damage;

    public override void ApplyElement(SpellData spell)
    {
        base.ApplyElement(spell);
        spell.ProjectilePrefab = ProjectilePrefab;
        spell.Damage += Damage;
    }
}
