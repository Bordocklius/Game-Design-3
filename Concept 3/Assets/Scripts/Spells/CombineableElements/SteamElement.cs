using UnityEngine;

[CreateAssetMenu(fileName = "SteamElement", menuName = "Magic/Steam Element")]
public class SteamElement : Element
{
    public GameObject ProjectilePrefab;
    public float Damage;
    public float Speed;

    public override void ApplyElement(SpellData spell)
    {
        base.ApplyElement(spell);
        spell.ProjectilePrefab = ProjectilePrefab;
        spell.Damage += Damage;
        spell.Speed += Speed;
    }
}
