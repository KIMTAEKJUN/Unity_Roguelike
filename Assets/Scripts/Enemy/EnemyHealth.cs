using UnityEngine;

namespace Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 3;
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
            OnDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }
}