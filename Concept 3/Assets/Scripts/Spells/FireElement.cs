using UnityEngine;

[CreateAssetMenu(fileName = "FireElement", menuName = "Magic/Fire Element")]
public class FireElement : Element
{
    public GameObject ProjectilePrefab;
    public float Damage;

    public override void ApplyElement(SpellData spell)
    {
        spell.ProjectilePrefab.Add(ProjectilePrefab);
        spell.Damage += Damage;
    }
}
