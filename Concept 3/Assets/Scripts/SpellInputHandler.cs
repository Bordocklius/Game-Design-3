using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellInputHandler : MonoBehaviour
{
    public Transform StartPoint;
    public LayerMask GroundMask;

    public GameObject GenericProjectile;
    public FireElementCancelable FireElement;
    public WaterElementCancelable WaterElement;
    public WindElement WindElement;
    public EarthElement EarthElement;

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
        SpellManager.Instance.AddElementToQueue(WaterElement);
    }

    public void OnSpellSlot3(InputValue inputValue)
    {
        SpellManager.Instance.AddElementToQueue(WindElement);
    }

    public void OnSpellSlot4(InputValue inputValue)
    {
        SpellManager.Instance.AddElementToQueue(EarthElement);
    }

    public void OnAttack(InputValue inputValue)
    {
        if (SpellManager.Instance.IsElementQueueEmpty)
            return;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);
        Vector3 targetPos = Vector3.zero;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, GroundMask))
        {
            targetPos = hitInfo.point;
        }

        SpellData spell = SpellManager.Instance.CastElementQueue();
        GameObject projectile = Instantiate(spell.ProjectilePrefab);
        projectile.transform.position = StartPoint.position;
        if (spell.ProjectileScale > 1)
            projectile.transform.localScale *= spell.ProjectileScale;

        Vector3 direction = (targetPos - projectile.transform.position).normalized;
        projectile.GetComponent<Rigidbody>().AddForce(direction * spell.Speed, ForceMode.VelocityChange);        
    }
}
