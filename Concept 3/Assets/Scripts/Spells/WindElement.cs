using UnityEngine;

[CreateAssetMenu(fileName = "WindElement", menuName = "Magic/Wind Element")]
public class WindElement: Element
{
    public float Speed;

    public override void ApplyElement(SpellData spell)
    {
        base.ApplyElement(spell);
        spell.Speed += Speed;
        spell.AddPlayerBuff("speed", Speed);
    }
}