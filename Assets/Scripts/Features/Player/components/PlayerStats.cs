using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Player.components
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private Image imageScreen;
        [SerializeField] private float maxHealth = 100; // 최대 체력
        [SerializeField] private float moveSpeed = 5f; // 이동 속도
        [SerializeField] private float attackSpeed = 1f; // 공격 속도
        [SerializeField] private float attackRange = 10f; // 공격 범위
        [SerializeField] private float attackCooldown = 1f; // 공격 쿨다운
        [SerializeField] private float attackDamage = 10f; // 공격력
        [SerializeField] private int projectileCount = 1; // 발사체 개수
        
        private float _currentHealth;

        private void Start()
        {
            InitializeStats();
        }

        private void InitializeStats()
        {
            _currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            Debug.Log($"플레이어가 {damage}의 피해를 입었습니다. 현재 체력: {_currentHealth}");
            
            StopCoroutine(HitAlphaAnimation());
            StartCoroutine(HitAlphaAnimation());

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        public void IncreaseMaxHealth(float amount)
        {
            maxHealth += amount;
            _currentHealth += amount;
            Debug.Log($"플레이어 최대 체력이 {maxHealth}로 증가했습니다. 현재 체력: {_currentHealth}");
        }

        public void IncreaseAttackSpeed(float multiplier)
        {
            attackSpeed *= multiplier;
            Debug.Log($"플레이어 공격 속도가 {attackSpeed}로 증가했습니다.");
        }

        public void IncreaseMoveSpeed(float amount)
        {
            moveSpeed += amount;
            Debug.Log($"플레이어 이동 속도가 {moveSpeed}로 증가했습니다.");
        }

        public void IncreaseProjectileCount(int count)
        {
            projectileCount += count;
            Debug.Log($"플레이어 발사체 개수가 {projectileCount}로 증가했습니다.");
        }

        public void IncreaseDamage(float amount)
        {
            attackDamage += amount;
            Debug.Log($"플레이어 공격력이 {attackDamage}로 증가했습니다.");
        }

        private static void Die()
        {
            Debug.Log("플레이어가 사망했습니다!");
            Time.timeScale = 0f;
        }
        
        private IEnumerator HitAlphaAnimation()
        {
            if (imageScreen == null) yield break;

            var color = imageScreen.color;
            color.a = 0.4f;
            imageScreen.color = color;

            while (color.a > 0.0f)
            {
                color.a -= Time.deltaTime;
                imageScreen.color = color;
                
                yield return null;
            }
        }

        public float GetMoveSpeed() => moveSpeed;
        public float GetAttackRange() => attackRange;
        public float GetAttackCooldown() => attackCooldown / attackSpeed;
        public float GetAttackDamage() => attackDamage;
        public int GetProjectileCount() => projectileCount;
    }
}