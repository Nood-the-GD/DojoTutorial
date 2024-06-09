using System;
using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class Enemy : MonoBehaviour, IBattler
{
    [SerializeField] private float _damage = 20;
    private HealthSystem _healthSystem;
    private Animator _animator;
    private bool _isDead;

    void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _animator = GetComponentInChildren<Animator>();
        _isDead = false;
    }

    void Start()
    {
        BattleManager.Instance.Prepare(BattleSide.Enemy, this);
        BattleManager.Instance.onEnemyTurn += () => 
        { 
            if(!_isDead)
                Attack(); 
        };
        _healthSystem.onOutOfHealth += () => OnOutOfHealth();
    }

    private void OnOutOfHealth()
    {
        // Dead animation
        _animator.Play("Enemy Death");

        // EndTurn
        NoodyCustomCode.StartDelayFunction(() => BattleManager.Instance.EndBattle(BattleSide.Enemy), 1f);

        _isDead = true;
    }

    public void Attack()
    {
        // Attack animation
        _animator.Play("Enemy Attack 1");
        ReturnToIdle_Delay(_animator.GetCurrentAnimatorStateInfo(0).length);

        // Attack logic
        IBattler target = BattleManager.Instance.GetOpponent(BattleSide.Enemy)[0];
        target.Damage(_damage);
    }

    public void Skill()
    {
        Attack();
    }

    public void Damage(float damage)
    {
        // Animation
        _animator.Play("Enemy Hit");
        ReturnToIdle_Delay(_animator.GetCurrentAnimatorStateInfo(0).length);
        // Logic
        _healthSystem.Damage(damage);
    }
    private void ReturnToIdle_Delay(float delay)
    {
        Debug.Log("Return to Idle");
        NoodyCustomCode.StartDelayFunction(() =>
        {
            if (_isDead == true) return;
            
            _animator.Play("Enemy Idle");
            // EndTurn
            BattleManager.Instance.EndTurn(BattleSide.Enemy);
        }, 0.9f);
    }
}
