using Core.abstracts;
using Features.Player.components;
using UnityEngine;

namespace Features.Boss
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

        // 폭발 스킬
        private void PerformExplosion()
        {
            var hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, explosionRange, _targetsBuffer);

            for (var i = 0; i < hitCount; i++)
            {
                var target = _targetsBuffer[i];
                if (target == null) continue;

                if (target.CompareTag("Player"))
                {
                    target.GetComponent<PlayerStats>()?.TakeDamage(explosionDamage);
                }
                else if (target.CompareTag("Boss"))
                {
                    target.GetComponent<BossBase>()?.TakeDamage(explosionDamage);
                }
            }

            Debug.Log($"Destroyer: {hitCount}개 대상에게 폭발 데미지 적용");
        }
        
        protected override void OnBossStart()
        {
            explosionRange *= 1.2f;
        }

        protected override void OnPhaseTransition()
        {
            Debug.Log("Destroyer: 페이즈 2 전환 - 폭발 범위 증가!");
            explosionRange *= 1.5f;
        }
    }
}