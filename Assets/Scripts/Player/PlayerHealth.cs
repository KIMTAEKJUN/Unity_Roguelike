using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        public int maxHealth = 5;
        private int _currentHealth;

        void Start()
        {
            _currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            Debug.Log("플레이어 캐릭터의 체력이 " + _currentHealth + "로 감소했습니다.");

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            Debug.Log("플레이어 캐릭터가 죽었습니다!");
            // 게임 오버 처리 로직 추가 가능
        }
    }
}