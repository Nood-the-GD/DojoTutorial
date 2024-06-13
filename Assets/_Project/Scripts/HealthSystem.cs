using System;
using System.Collections;
using System.Collections.Generic;
using NOOD;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class HealthSystem : MonoBehaviour
    {
        public Action onOutOfHealth;
        public Action onDamage;

        [SerializeField] private Slider _healthSlider;
        private IBlockChainObject _blockchainObject; // This is interface for both player and enemy
        private Health _healthComponent;
        private float _maxHealth = 100f;
        private float _currentHealth;

        void Awake()
        {
            _blockchainObject = GetComponentInParent<IBlockChainObject>();
        }
        IEnumerator Start()
        {
            yield return new WaitForSeconds(0.2f);
            _healthComponent = EntityManager.Instance.GetEntity<Health>(_blockchainObject.hexCode, _blockchainObject.gameId);
            _healthComponent.OnUpdated.RemoveAllListeners(); // Just in case it has some old listeners
            _healthComponent.OnUpdated.AddListener(() => UpdateData());
            if(_blockchainObject is Enemy)
            {
                _healthComponent.OnUpdated.AddListener(() => UpdateHealth());
            }
            _healthSlider.maxValue = _maxHealth;
            _currentHealth = _healthComponent.health;
            _healthSlider.value = _currentHealth;
        }

        void OnDisable()
        {
        }

        // Update heal on visual (slider and text)
        public void UpdateHealth()
        {
            _healthSlider.value = _healthComponent.health;
            if(_currentHealth < _healthComponent.health) // Heal
            {
                NumberTextManager.Instance.SpawnText((_healthComponent.health - _currentHealth).ToString("0"), Color.green, this.transform.position); 
            }
            if(_currentHealth > _healthComponent.health) // Lose heal
            {
                NumberTextManager.Instance.SpawnText((_currentHealth - _healthComponent.health).ToString("0"), Color.red, this.transform.position); 
            }
            _currentHealth = _healthComponent.health;
        }
        // Update heal data and check if it dead
        private void UpdateData()
        {
            if(_healthComponent.health <= 0)
            {
                onOutOfHealth?.Invoke();
                UpdateHealth();
            }
        }

        public float GetCurrentHealth()
        {
            return _currentHealth;
        }
    }

}
