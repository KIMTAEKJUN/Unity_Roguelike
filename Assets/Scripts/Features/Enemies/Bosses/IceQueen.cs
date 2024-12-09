using Core.abstracts;
using Features.Player;
using UnityEngine;

namespace Features.Enemies.Bosses
{
    public class IceQueen : BossBase
    {
        [SerializeField] private float iceExplosionCooldown = 6f; // 얼음 폭발 쿨타임
        [SerializeField] private float explosionRange = 3f; // 얼음 폭발 범위
        [SerializeField] private float freezeDuration = 2f; // 얼림 지속 시간
        
        private float _explosionTimer;
        private readonly Collider2D[] _targetsBuffer = new Collider2D[20]; // 충돌 처리 버퍼

        protected override void Update()
        {
            base.Update();

            _explosionTimer += Time.deltaTime;
            if (_explosionTimer < iceExplosionCooldown) return;

            IceExplosion();
            _explosionTimer = 0f;
        }

        private void IceExplosion()
        {
            Debug.Log("IceQueen: 얼음 폭발!");
            
            // 폭발 범위 내 적과 플레이어를 얼림
            var hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, explosionRange, _targetsBuffer);

            for (var i = 0; i < hitCount; i++)
            {
                var target = _targetsBuffer[i];

                if (target.CompareTag("Player"))
                {
                    var playerController = target.GetComponent<PlayerController>();
                    if (playerController != null)
                    {
                        playerController.ApplyFreeze(freezeDuration);
                    }
                }

                if (target.CompareTag("Enemy"))
                {
                    var enemyController = target.GetComponent<EnemyBase>();
                    if (enemyController != null)
                    {
                        enemyController.ApplyFreeze(freezeDuration);
                    }
                }
            }

            Debug.Log($"{hitCount}개 대상에게 얼음 효과 적용");
        }
        
        public override void OnBossStart()
        {
            Debug.Log("IceQueen: 전투 시작과 함께 얼음 장벽 생성!");
            explosionRange *= 1.2f; // 초기화 로직
        }

        protected override void OnPhaseTransition()
        {
            explosionRange += 1f;
            Debug.Log("IceQueen: 페이즈 2 전환 - 폭발 범위 증가!");
            // 추후 추가 전환 로직
        }
    }
}