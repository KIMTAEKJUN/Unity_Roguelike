using System.Collections;
using Core.abstracts;
using Features.Player;
using UnityEngine;

namespace Features.Enemies.Types
{
    public class NormalEnemy : EnemyBase
    {
        [SerializeField] private float enhancedAttackChance = 0.2f; // 
        [SerializeField] private int enhancedDamage = 3; // 강화 공격 데미지
        [SerializeField] private Color enhancedAttackColor = Color.blue; // 강화 공격 시 적용할 색상
        [SerializeField] private float effectDuration = 0.5f; // 강화 공격 색상 효과 지속 시간
        
        private SpriteRenderer _spriteRenderer;

        protected override void Start()
        {
            base.Start();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void AttackPlayer()
        {
            var actualDamage = damage;

            // 강화 공격 확률 계산
            if (Random.value < enhancedAttackChance)
            {
                actualDamage = enhancedDamage;
                Debug.Log("NormalEnemy 스킬 사용");
                
                // 색상 이펙트 코루틴 실행
                if (_spriteRenderer != null)
                {
                    StartCoroutine(EnhancedAttackEffect());
                }
            }

            var playerHealth = Player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(actualDamage);
            }
        }

        private IEnumerator EnhancedAttackEffect()
        {
            var originalColor = _spriteRenderer.color; // 기존 색상 저장
            _spriteRenderer.color = enhancedAttackColor; // 강화 공격 색상으로 변경

            yield return new WaitForSeconds(effectDuration); // 효과 지속 시간만큼 대기

            _spriteRenderer.color = originalColor; // 원래 색상으로 복원
        }
    }
}