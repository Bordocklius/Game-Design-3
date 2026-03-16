
public class WindElement: Element
{
    public float Speed;

    public override void ApplyElement(SpellData spell)
    {
        spell.Speed += Speed;
    }
}