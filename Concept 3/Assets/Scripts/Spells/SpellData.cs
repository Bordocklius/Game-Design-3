using System.Collections.Generic;
using UnityEngine;

public class SpellData
{
    public GameObject ProjectilePrefab;
    private int _projectilePriority = 0;
    public float ProjectileScale = 1f;
    public float Damage;
    public float Speed = 10f;
    public float SpellCost;


    public void SetSpellProjectile(GameObject projectilePrefab, int priority)
    {
        if(priority > _projectilePriority)
        {
            _projectilePriority = priority;
            ProjectilePrefab = projectilePrefab;
        }
    }
}
