using Core.Manager;
using Features.Player;
using UnityEngine;

namespace Features.Enemies.components
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 3f;
        [SerializeField] private float expReward = 20f; // 적 처치 시 제공하는 경험치
        private float _currentHealth;
        
        public delegate void DestroyedAction();
        public event DestroyedAction OnDestroyed;


        private void Start()
        {
            _currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            GameManager.Instance.AddExperience(expReward);
            Debug.Log($"플레이어가 {expReward} 경험치를 획득했습니다.");
            
            OnDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }
}