using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public Action onOutOfHealth;
    public Action onDamage;

    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private Slider _healthSlider;
    private float _currentHealth;

    void Awake()
    {
        _currentHealth = _maxHealth;
        _healthSlider.maxValue = _maxHealth;
        _healthSlider.value = _currentHealth;
    }

    public void Damage(float damage)
    {
        _currentHealth -= damage;
        _healthSlider.value = _currentHealth;
        if(_currentHealth <= 0)
        {
            onOutOfHealth?.Invoke();
        }
        else
        {
            onDamage?.Invoke();
        }
    }

    public void AddHealth(float health)
    {
        _currentHealth += health;
        if(_currentHealth >= _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
        _healthSlider.value = _currentHealth;
    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }
}
