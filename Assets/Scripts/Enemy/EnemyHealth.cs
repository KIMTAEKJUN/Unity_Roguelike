using UnityEngine;

namespace Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        public int maxHealth = 3;
        private int _currentHealth;
        
        public delegate void DestroyedAction();
        public event DestroyedAction OnDestroyed;


        private void Start()
        {
            _currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            // 파괴될 때 이벤트를 통해 SpawnManager에 알림
            OnDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }
}