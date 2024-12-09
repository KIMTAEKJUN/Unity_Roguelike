using Core.abstracts;
using Features.Player;
using UnityEngine;

namespace Features.Enemies.Bosses
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

        private void CastLightning()
        {
            Debug.Log("Storm: 번개 공격!");
            
            // 번개를 플레이어 주변에 떨어뜨림
            var hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, lightningRange, _targetsBuffer);

            for (var i = 0; i < hitCount; i++)
            {
                var target = _targetsBuffer[i];

                if (target.CompareTag("Player"))
                {
                    var playerHealth = target.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(lightningDamage);
                    }
                }
            }

            Debug.Log($"{hitCount}개 대상에게 번개 데미지 적용");
        }
        
        public override void OnBossStart()
        {
            Debug.Log("Storm: 전투 시작과 함께 폭풍우 생성!");
            lightningCount = 3; // 초기화 로직
        }

        protected override void OnPhaseTransition()
        {
            lightningCount += 2;
            Debug.Log("Storm: 페이즈 2 전환 - 번개 개수 증가!");
            // 추후 추가 전환 로직
        }
    }
}