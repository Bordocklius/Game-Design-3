using System.Collections.Generic;
using UnityEngine;

public class GroundArea : MonoBehaviour
{
    public float TTL;
    public LayerMask Layermask;
    public List<Element> TriggerableElements;
    public ParticleSystem Shockwave;
    public ParticleSystem MagicCircle;
    public Element Element;
    public float Damage;

    private float _timer = 0f;
    
    private void Update()
    {
        _timer += Time.deltaTime;
        if(_timer >= TTL)
        {
            Destroy(this.gameObject);
        }    
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        if(obj.TryGetComponent<SpellProjectile>(out SpellProjectile projectile))
        {
            projectile.TriggeredZone = true;
            if(TriggerableElements != null && TriggerableElements.Contains(projectile.SpellData.Element))
            {
                TriggerArea();
                Destroy(obj);
            }
        }
    }

    private void TriggerArea()
    {
        Collider[] inside = Physics.OverlapSphere(transform.position, 8, Layermask);

        if(inside.Length > 0 )
        {
            foreach(Collider c in inside)
            {
                GameObject obj = c.gameObject;
                if(obj.TryGetComponent<EnemyBase>(out EnemyBase enemy))
                {
                    enemy.TakeDamage(Damage, Element);
                }
            }
        }

        MagicCircle.Stop();
        MagicCircle.Clear();
        Shockwave.Play();
        Destroy(this.gameObject, 1.2f);
    } 
}
