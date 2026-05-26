using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellInputHandler : MonoBehaviour
{
    public Transform StartPoint;
    public LayerMask GroundMask;

    public GameObject GenericProjectile;
    public FireElementCombineable FireElement;
    public WaterElementCombineable WaterElement;
    public WindElement WindElement;
    public EarthElement EarthElement;

    [Space(10), Header("Particle effects")]
    public ParticleSystem CastingParticles;
    public ParticleSystem AuraParticles;

    private PlayerMovement _playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSpellSlot1(InputValue inputValue)
    {
        SpellManager.Instance.AddElementToQueue(FireElement);
        ToggleCastingParticles(true);
    }

    public void OnSpellSlot2(InputValue inputValue)
    {
        SpellManager.Instance.AddElementToQueue(WaterElement);
        ToggleCastingParticles(true);
    }

    public void OnSpellSlot3(InputValue inputValue)
    {
        SpellManager.Instance.AddElementToQueue(WindElement);
        ToggleCastingParticles(true);
    }

    public void OnSpellSlot4(InputValue inputValue)
    {
        SpellManager.Instance.AddElementToQueue(EarthElement);
        ToggleCastingParticles(true);
    }

    public void OnAttack(InputValue inputValue)
    {
        if (SpellManager.Instance.IsElementQueueEmpty)
            return;

        SpellData spell = SpellManager.Instance.CastElementQueue();
        ToggleCastingParticles(false);

        // No projectile - apply all buffs to player
        if (spell.ProjectilePrefab == null)
        {
            var buffs = spell.GetAllPlayerBuffs();
            foreach (var buff in buffs)
            {
                ApplyPlayerBuff(buff.Key, buff.Value);
            }
            return;
        }

        // Has projectile - spawn and fire it
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);
        Vector3 targetPos = Vector3.zero;

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, GroundMask))
        {
            targetPos = hitInfo.point;
        }

        GameObject projectile = Instantiate(spell.ProjectilePrefab);
        SpellProjectile spellProjectile = projectile.GetComponent<SpellProjectile>();
        spellProjectile.SpellData = spell;
        projectile.transform.position = StartPoint.position;
        if (spell.ProjectileScale > 1)
            projectile.transform.localScale *= spell.ProjectileScale;

        Vector3 direction = (targetPos - projectile.transform.position).normalized;
        direction.y = 0f;
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.transform.forward = direction;
        rb.AddForce(direction * spell.Speed, ForceMode.VelocityChange);
        AudioManager.Instance.PlayCastingSound();
    }

    private void ApplyPlayerBuff(string buffType, float value)
    {
        switch (buffType.ToLower())
        {
            case "speed":
                StartCoroutine(_playerMovement.ApplySpeedBuff(value, 5f));
                break;
            case "damage":
                // TODO: Implement damage buff if you have a player combat system
                break;
            case "health":
                // TODO: Implement health buff if needed
                break;
            default:
                Debug.LogWarning($"Unknown buff type: {buffType}");
                break;
        }
    }

    private void ToggleCastingParticles(bool isPlaying)
    {
        if (isPlaying)
        {
            AuraParticles.Play();
            CastingParticles.Play();
        }
        else
        {
            AuraParticles.Stop();
            CastingParticles.Stop();
        }
    }
}
