using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private bool _isDead;

    public delegate void HealthReduced(float currentHealth);
    public HealthReduced OnHealthReduced;

    [SerializeField]
    private float _health = 100;

    private void Start()
    {
        
    }

    public void GetHit(float damage)
    {
        if (_isDead)
            return;

        _health -= damage;

        OnHealthReduced?.Invoke(_health);
    }

    public void Heal(float amount)
    {
        if (_isDead)
            return;

        _health += amount;
    }

    public void Dead()
    {
        _isDead = true;
    }
}




        //OnHealthReduced?.Invoke(_health);
    //public delegate void HealthReduced(float currentHealth);
    //public HealthReduced OnHealthReduced;