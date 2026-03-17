using UnityEngine;
using UnityEngine.InputSystem;

public class SpellInputHandler : MonoBehaviour
{
    public FireElement FireElement;
    public WindElement WindElement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSpellSlot1(InputValue inputValue)
    {
        SpellManager.Instance.AddElementToQueue(FireElement);
    }

    public void OnSpellSlot2(InputValue inputValue)
    {
        SpellManager.Instance.AddElementToQueue(WindElement);
    }

    public void OnAttack(InputValue inputValue)
    {
        SpellData spell = SpellManager.Instance.CastElementQueue();
    }
}
