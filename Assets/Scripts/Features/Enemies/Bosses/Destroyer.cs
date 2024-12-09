using Core.abstracts;
using Features.Player;
using UnityEngine;

namespace Features.Enemies.Bosses
{
    public class Destroyer : BossBase
    {
        [SerializeField] private float explosionCooldown = 5f; // 폭발 쿨타임
        [SerializeField] private float explosionRange = 3f; // 폭발 범위
        [SerializeField] private float explosionDamage = 20f; // 폭발 데미지

        private float _explosionTimer; // 폭발 쿨타임 타이머
        private readonly Collider2D[] _targetsBuffer = new Collider2D[20]; // 충돌 처리 버퍼

        protected override void Update()
        {
            base.Update();

            _explosionTimer += Time.deltaTime;
            if (_explosionTimer < explosionCooldown) return;

            PerformExplosion();
            _explosionTimer = 0f;
        }

        private void PerformExplosion()
        {
            Debug.Log("Destroyer: 광역 폭발!");
            
            // 폭발 범위 내의 모든 적과 플레이어에게 데미지를 입힘
            var hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, explosionRange, _targetsBuffer);

            for (var i = 0; i < hitCount; i++)
            {
                var target = _targetsBuffer[i];
                if (target == null) continue;

                if (target.CompareTag("Player"))
                {
                    var playerHealth = target.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(explosionDamage);
                        Debug.Log($"Destroyer: 플레이어에게 {explosionDamage} 데미지!");
                    }
                }
                else if (target.CompareTag("Boss"))
                {
                    var bossBase = target.GetComponent<BossBase>();
                    if (bossBase != null)
                    {
                        bossBase.TakeDamage(explosionDamage);
                        Debug.Log($"Destroyer: 보스에게 {explosionDamage} 데미지!");
                    }
                }
            }

            Debug.Log($"{hitCount}개 대상에게 폭발 데미지 적용");
            
            // 폭발 효과 (이펙트 추가 예정)
        }
        
        public override void OnBossStart()
        {
            
            Debug.Log("Destroyer: 보스 시작!");
            explosionRange *= 1.2f; // 초기화 로직
        }

        protected override void OnPhaseTransition()
        {
            explosionRange *= 1.5f;
            Debug.Log("Destroyer: 페이즈 2 전환 - 폭발 범위 증가!");
            // 추후 추가 전환 로직
        }
    }
}