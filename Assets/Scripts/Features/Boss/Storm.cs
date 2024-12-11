using Core.abstracts;
using Features.Player;
using Features.Player.components;
using UnityEngine;

namespace Features.Boss
{
    public class Storm : BossBase
    {
        [SerializeField] private int lightningCount = 1; // 번개 공격 횟수
        [SerializeField] private float lightningCooldown = 3f; // 번개 공격 쿨타임
        [SerializeField] private float lightningDamage = 20f; // 번개 데미지
        [SerializeField] private float lightningRange = 5f; // 번개 범위

        private float _lightningTimer; // 번개 공격 타이머
        private readonly Collider2D[] _targetsBuffer = new Collider2D[20]; // 충돌 처리 버퍼

        protected override void Update()
        {
            base.Update();

            _lightningTimer += Time.deltaTime;
            if (_lightningTimer < lightningCooldown) return;

            CastLightning();
            _lightningTimer = 0f;
        }

        // 번개 공격
        private void CastLightning()
        {
            var hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, lightningRange, _targetsBuffer);

            for (var i = 0; i < hitCount; i++)
            {
                var target = _targetsBuffer[i];
                if (target == null || !target.CompareTag("Player")) continue;

                var playerHealth = target.GetComponent<PlayerStats>();
                playerHealth?.TakeDamage(lightningDamage);
            }

            Debug.Log($"{hitCount}개의 대상에게 번개 데미지 적용");
        }
        
        protected override void OnBossStart()
        {
            lightningCount = 3;
        }

        protected override void OnPhaseTransition()
        {
            lightningCount += 2;
            Debug.Log("Storm: 페이즈 2 전환 - 번개 개수 증가!");
        }
    }
}