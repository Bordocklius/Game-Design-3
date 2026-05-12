using Assets.Scripts.Interfaces;
using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    [Space(10), Header("ProjectileSettings")]
    public SpellData SpellData;
    public float TTL;
    public GameObject GroundAreaObj;
    public LayerMask LayerMask;

    public bool TriggeredZone = false;

    private float _timer = 0f;


    void Start()
    {

    }

    void Update()
    {
        _timer += Time.deltaTime;
        if(_timer >= TTL)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            damagable.TakeDamage(SpellData.Damage, SpellData.Element);
            SpawnGroundAura();
            Destroy(gameObject);
        }        
    }

    private void SpawnGroundAura()
    {
        // Raycast to ground
        Ray ray = new(transform.position, Vector3.down);
        if(!TriggeredZone && Physics.Raycast(ray, out RaycastHit hit,100f, LayerMask, QueryTriggerInteraction.Ignore))
        {
            Vector3 spawnpos = hit.point + new Vector3(0, 0.1f, 0);
            if(GroundAreaObj != null)
                Instantiate(GroundAreaObj, spawnpos, Quaternion.identity);
        }
    }
}
