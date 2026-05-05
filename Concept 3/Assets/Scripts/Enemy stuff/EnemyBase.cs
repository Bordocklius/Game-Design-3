using Assets.Scripts;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour, IDamagable, IHealthBar
{
    public event PropertyChangedEventHandler PropertyChanged;

    [Space(10), Header("Enemy Settings")]
    [SerializeField] private float _maxHealth;
    [SerializeField] private Image _hbImage;

    private float _health;
    public float Health
    {
        get { return _health; }
        set 
        { 
            if(_health == value)
                return;

            _health = value;
            OnPropertyChanged();

            if(_health <= 0f)
                OnHealthDepleted();
        }
    }

    public float HealthProgress => _health / _maxHealth;

    public List<Element> ElementWeaknesses;
    public List<Element> ElementResistances;

    [Space(10), Header("Target Settings")]
    public Transform PlayerTarget;
    public float Speed = 10f;

    private HealthBar _healthBar;

    private void OnPropertyChanged([CallerMemberName] String propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void Awake()
    {
        if(PlayerTarget == null)
        {
            PlayerTarget = FindFirstObjectByType<PlayerMovement>().gameObject.transform;
        }        
    }

    private void Start()
    {
        _healthBar = new(this, _hbImage);
        Health = _maxHealth;
    }

    void Update()
    {
        Vector3 movement = (PlayerTarget.position - transform.position).normalized;
        movement = movement * Speed;

        transform.rotation = Quaternion.LookRotation(movement);

        transform.position += movement * Time.deltaTime;
    }

    public void TakeDamage(float damage, Element element)
    {
        if(ElementWeaknesses.Contains(element)) 
        {
            Health -= damage * 2;
        }
        if(ElementResistances.Contains(element))
        {
            Health -= damage * 0.5f;
        }
        else
        {
            Health -= damage;
        }        
    }

    private void OnHealthDepleted()
    {
        _healthBar?.Dispose();
        EnemySpawner.Instance.EnemyKilled();
        Destroy(this.gameObject);
    }

}
