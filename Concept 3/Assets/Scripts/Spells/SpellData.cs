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

    // Buff system for non-projectile spells
    private Dictionary<string, float> _playerBuffs = new Dictionary<string, float>();

    public void SetSpellProjectile(GameObject projectilePrefab, int priority)
    {
        if(priority > _projectilePriority)
        {
            _projectilePriority = priority;
            ProjectilePrefab = projectilePrefab;
        }
    }

    /// <summary>
    /// Adds or accumulates a buff effect to be applied to the player.
    /// </summary>
    /// <param name="buffType">The type of buff (e.g., "speed", "damage")</param>
    /// <param name="value">The buff value to add</param>
    public void AddPlayerBuff(string buffType, float value)
    {
        if (_playerBuffs.ContainsKey(buffType))
        {
            _playerBuffs[buffType] += value;
        }
        else
        {
            _playerBuffs[buffType] = value;
        }
    }

    /// <summary>
    /// Gets a buff value if it exists, otherwise returns 0.
    /// </summary>
    public float GetPlayerBuff(string buffType)
    {
        return _playerBuffs.ContainsKey(buffType) ? _playerBuffs[buffType] : 0f;
    }

    /// <summary>
    /// Returns all player buffs in the spell.
    /// </summary>
    public Dictionary<string, float> GetAllPlayerBuffs()
    {
        return new Dictionary<string, float>(_playerBuffs);
    }
}
