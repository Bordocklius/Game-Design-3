using UnityEngine;

[CreateAssetMenu(fileName = "SteamElement", menuName = "Magic/Steam Element")]
public class SteamElement : Element
{
    public GameObject ProjectilePrefab;
    public int Priority;
    public float Damage;
    public float Speed;

    public override void ApplyElement(SpellData spell)
    {
        base.ApplyElement(spell);
        spell.SetSpellProjectile(ProjectilePrefab, Priority, this);
        spell.Damage += Damage;
        spell.Speed += Speed;
    }
}
