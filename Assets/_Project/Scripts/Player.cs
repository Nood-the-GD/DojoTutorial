using System;
using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class Player : MonoBehaviour, IBattler
{
    [SerializeField] private float _damage = 10;
    private HealthSystem _healthSystem;
    private Animator _animator;
    private bool _isDead;


    void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();
        _animator = GetComponentInChildren<Animator>();
    }    

    void Start()
    {
        BattleManager.Instance.Prepare(BattleSide.Player, this);
        _healthSystem.onOutOfHealth += () => OnOutOfHealth();
        UIManager.Instance.onPlayerAttack += () => Attack();
        UIManager.Instance.onPlayerSkill += () => Skill();
        UIManager.Instance.onPlayerHeal += () => Heal();
    }

    private void OnOutOfHealth()
    {
        // Animation
        _animator.Play("Death");

        // Logic
        _isDead = true;
        NoodyCustomCode.StartDelayFunction(() =>
        {
            BattleManager.Instance.EndBattle(BattleSide.Player);
        }, 1f);
    }

    private void Heal()
    {
        // Animation
        _animator.Play("Heal");
        PlayIdleAfterDelay(_animator.GetCurrentAnimatorStateInfo(0).length);

        // Logic
        _healthSystem.AddHealth(30);

        // EndTurn
        BattleManager.Instance.EndTurn(BattleSide.Player);
    }

    public void Damage(float damage)
    {
        // Animation
        _animator.Play("Hurt");
        PlayIdleAfterDelay(_animator.GetCurrentAnimatorStateInfo(0).length);

        // Logic
        _healthSystem.Damage(damage);
        Debug.Log("Player_Damage: " + damage);
    }

    public void Attack()
    {
        // Attack animation
        _animator.Play("Attack");
        PlayIdleAfterDelay(_animator.GetCurrentAnimatorStateInfo(0).length);

        // Attack logic
        IBattler target = BattleManager.Instance.GetOpponent(BattleSide.Player)[0];
        target.Damage(_damage);

        // EndTurn
        BattleManager.Instance.EndTurn(BattleSide.Player);
    }

    public void Skill()
    {
        // Animation
        _animator.Play("Skill");
        PlayIdleAfterDelay(_animator.GetCurrentAnimatorStateInfo(0).length);

        // Logic
        IBattler target = BattleManager.Instance.GetOpponent(BattleSide.Player)[0];
        target.Damage(_damage * 2);

        // EndTurn
        BattleManager.Instance.EndTurn(BattleSide.Player);
    }
    
    private void PlayIdleAfterDelay(float delay)
    {
        NoodyCustomCode.StartDelayFunction(() =>
        {
            if(_isDead == true) return;

            _animator.Play("Idle");
        }, delay);
    }
}
