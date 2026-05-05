using Assets.Scripts.Interfaces;
using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    [Space(10), Header("ProjectileSettings")]
    public SpellData SpellData;
    public float TTL;

    private float _timer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
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
        }

        Destroy(gameObject);
    }
}
