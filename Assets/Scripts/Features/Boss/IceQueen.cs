using Core.abstracts;
using Features.Player;
using UnityEngine;

namespace Features.Boss
{
    public class IceQueen : BossBase
    {
        [SerializeField] private float iceExplosionCooldown = 6f; // 얼음 폭발 쿨타임
        [SerializeField] private float explosionRange = 3f; // 얼음 폭발 범위
        [SerializeField] private float freezeDuration = 2f; // 얼림 지속 시간
        
        private float _explosionTimer; // 얼음 폭발 타이머
        private readonly Collider2D[] _targetsBuffer = new Collider2D[20]; // 충돌 처리 버퍼

        protected override void Update()
        {
            base.Update();

            _explosionTimer += Time.deltaTime;
            if (_explosionTimer < iceExplosionCooldown) return;

            IceExplosion();
            _explosionTimer = 0f;
        }

        // 얼음 폭발 스킬
        private void IceExplosion()
        {
            var hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, explosionRange, _targetsBuffer);

            for (var i = 0; i < hitCount; i++)
            {
                var target = _targetsBuffer[i];

                if (target.CompareTag("Player"))
                {
                    target.GetComponent<PlayerController>()?.ApplyFreeze(freezeDuration);
                }
                else if (target.CompareTag("Enemy"))
                {
                    target.GetComponent<EnemyBase>()?.ApplyFreeze(freezeDuration);
                }
            }

            Debug.Log($"IceQueen: {hitCount}개 대상에게 얼음 효과 적용");
        }
        
        protected override void OnBossStart()
        {
            explosionRange *= 1.2f;
        }

        protected override void OnPhaseTransition()
        {
            Debug.Log("IceQueen: 페이즈 2 전환 - 폭발 범위 증가!");
            explosionRange += 1f;
        }
    }
}